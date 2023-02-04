using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerState playerState;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PickupTargetSensor pickupTargetSensor;
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private float maxThrowDistance = 10f;
    [SerializeField] private float durationToReachMaxDistance = 2f;

    private Vector2 moveDir = Vector2.zero;
    private float currentThrowCharge;
    private Coroutine chargeThrowRoutine;

    private void OnEnable()
    {
        playerInput.onActionTriggered += EventHandler;
    }

    private void OnDisable()
    {
        playerInput.onActionTriggered -= EventHandler;
    }

    private void EventHandler(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "move":
                OnMove(context);
                break;
            case "pickup":
                OnPickup(context);
                break;
            case "throw":
                OnThrow(context);
                break;
        }
    }

    private void Update()
    {
        if (playerState.CanWalk)
        {
            rigidbody.velocity += moveDir * (Time.fixedDeltaTime * speedMultiplier);
            playerState.IsWalking = moveDir.magnitude > 0;
        }
        else
        {
            rigidbody.velocity = Vector2.zero;
            playerState.IsWalking = false;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
    }

    private void OnPickup(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
            {
                if (playerState.CanPickUp && pickupTargetSensor.HasPickupTarget)
                {
                    playerState.CurrentAction = PlayerAction.PickingUp;
                }

                break;
            }
            case InputActionPhase.Canceled:
            {
                if (playerState.CurrentAction == PlayerAction.PickingUp)
                {
                    playerState.CurrentAction = PlayerAction.None;
                }

                break;
            }
            case InputActionPhase.Performed:
            {
                if (!playerState.CanPickUp || !pickupTargetSensor.HasPickupTarget)
                {
                    return;
                }

                playerState.CurrentAction = PlayerAction.Carrying;

                //Pair carrot to player
                var target = pickupTargetSensor.CurrentPickupTarget.transform;
                target.SetParent(transform);
                target.localPosition = new Vector2(0, 0.5f);
                Debug.Log($"Picking up: {target}");
                playerState.ObjectCarrying = target;
                break;
            }
        }
    }

    private void OnThrow(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
            {
                if (playerState.CurrentAction != PlayerAction.Carrying || playerState.ObjectCarrying == null) return;

                Debug.Log($"Start throwing: {playerState.ObjectCarrying}");

                playerState.CurrentAction = PlayerAction.Throwing;

                chargeThrowRoutine = StartCoroutine(ChargeThrow());
                break;
            }
            case InputActionPhase.Performed:
            {
                if (playerState.CurrentAction != PlayerAction.Throwing || playerState.ObjectCarrying == null) return;

                if (chargeThrowRoutine != null)
                {
                    StopCoroutine(chargeThrowRoutine);
                }

                Debug.Log($"Performing throw: {playerState.ObjectCarrying}");

                playerState.ObjectCarrying.SetParent(null);

                var projectile = playerState.ObjectCarrying.GetComponent<ProjectileStateController>();
                var direction = (int)Mathf.Sign(transform.forward.x);
                projectile.FireProjectile(currentThrowCharge * maxThrowDistance, direction);

                playerState.ObjectCarrying = null;
                playerState.CurrentAction = PlayerAction.None;
                break;
            }
        }
    }

    private IEnumerator ChargeThrow()
    {
        var totalChargeDuration = 0f;
        currentThrowCharge = 0f;

        while (true)
        {
            yield return null;

            totalChargeDuration += Time.deltaTime;
            currentThrowCharge = Mathf.PingPong(totalChargeDuration, durationToReachMaxDistance);
            Debug.Log($"Current Throw Charge: {currentThrowCharge}");
        }
    }
}
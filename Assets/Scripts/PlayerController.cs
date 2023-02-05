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
    private SoundManager soundManager;
    private AudioSource pullingLoop;

    private void Awake()
    {
        soundManager = GameObject.FindWithTag("Sound")?.GetComponent<SoundManager>();
    }

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

    private void FixedUpdate()
    {
        if (playerState.CanWalk)
        {
            rigidbody.velocity += moveDir * (Time.fixedDeltaTime * speedMultiplier);
            playerState.WalkDirection = moveDir;

            if (Mathf.Abs(moveDir.x) > 0)
            {
                playerState.PlayerOrientation = (int)Mathf.Sign(moveDir.x);
            }
        }
        else
        {
            rigidbody.velocity = Vector2.zero;
            playerState.WalkDirection = Vector2.zero;
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
                    pullingLoop = soundManager.PlaySfxLoop(SoundManager.Sfx.Pulling);
                }

                break;
            }
            case InputActionPhase.Canceled:
            {
                if (playerState.CurrentAction == PlayerAction.PickingUp)
                {
                    playerState.CurrentAction = PlayerAction.None;
                    soundManager.StopSfxLoop(pullingLoop);
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
                target.gameObject.SetActive(false);
                target.localPosition = new Vector2(0, 0.5f);
                Debug.Log($"Picking up: {target}");
                playerState.ObjectCarrying = target;
                soundManager.StopSfxLoop(pullingLoop);
                soundManager.PlaySfx(SoundManager.Sfx.Pulled);
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
                playerState.ObjectCarrying.gameObject.SetActive(true);

                var projectile = playerState.ObjectCarrying.GetComponent<ProjectileStateController>();
                var direction = playerState.PlayerOrientation;
                projectile.FireProjectile(currentThrowCharge * maxThrowDistance, direction);

                playerState.ObjectCarrying = null;
                playerState.CurrentAction = PlayerAction.None;
                soundManager.PlaySfx(SoundManager.Sfx.Throw);
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
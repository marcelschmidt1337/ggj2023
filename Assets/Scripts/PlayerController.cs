using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerState playerState;
    [SerializeField] private PickupTargetSensor pickupTargetSensor;
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private float speedMultiplier = 1f;

    private Vector2 moveDir = Vector2.zero;


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

    public void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }

    #region Pickup

    private void OnStartPickup(InputAction.CallbackContext obj)
    {
        if (playerState.CanPickUp && pickupTargetSensor.HasPickupTarget)
        {
            playerState.CurrentAction = PlayerAction.PickingUp;
        }
    }

    private void OnCancelPickup(InputAction.CallbackContext obj)
    {
        if (playerState.CurrentAction == PlayerAction.PickingUp)
        {
            playerState.CurrentAction = PlayerAction.None;
        }
    }

    private void OnPerformedPickup(InputAction.CallbackContext obj)
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
    }

    #endregion

    private void OnStartThrow(InputAction.CallbackContext obj)
    {
        if (playerState.CurrentAction != PlayerAction.Carrying || playerState.ObjectCarrying == null) return;

        Debug.Log($"Start throwing up: {playerState.ObjectCarrying}");

        playerState.CurrentAction = PlayerAction.Throwing;
    }


    private void OnPerformedThrow(InputAction.CallbackContext obj)
    {
        if (playerState.CurrentAction != PlayerAction.Throwing || playerState.ObjectCarrying == null) return;

        Debug.Log($"Performing throw: {playerState.ObjectCarrying}");

        //Should execute throw, but drop carrot for now
        playerState.ObjectCarrying.SetParent(null);
        playerState.ObjectCarrying = null;
        playerState.CurrentAction = PlayerAction.None;
    }
}
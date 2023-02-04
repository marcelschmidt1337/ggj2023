using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PickupTargetSensor pickupTargetSensor;
    public PlayerState playerState;

    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction pickupAction;
    [SerializeField] private InputAction throwAction;
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private float speedMultiplier = 1f;


    private void OnEnable()
    {
        moveAction.Enable();
        pickupAction.Enable();
        throwAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        pickupAction.Disable();
        throwAction.Disable();
    }

    private void Awake()
    {
        pickupAction.started += OnStartPickup;
        pickupAction.canceled += OnCancelPickup;
        pickupAction.performed += OnPickupPerformed;
        throwAction.performed += OnThrow;
    }

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

    private void OnPickupPerformed(InputAction.CallbackContext obj)
    {
        if (!pickupTargetSensor.HasPickupTarget)
        {
            Debug.LogWarning("Pickup performed but, no target in range");
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

    private void Update()
    {
        if (playerState.CanWalk)
        {
            var moveAmount = moveAction.ReadValue<Vector2>();
            rigidbody.velocity += moveAmount * (Time.fixedDeltaTime * speedMultiplier);
            playerState.IsWalking = true;
        }
    }

    private void OnThrow(InputAction.CallbackContext obj)
    {
        if (playerState.CurrentAction != PlayerAction.Carrying || playerState.ObjectCarrying == null) return;

        //Drop carrot for now
        playerState.ObjectCarrying.SetParent(null);
        playerState.ObjectCarrying = null;
        playerState.CurrentAction = PlayerAction.None;
    }
}
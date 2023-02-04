using System;
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
        Debug.Log($"Picking up: {target}");
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
    }
}
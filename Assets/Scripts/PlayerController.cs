using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public event Action OnPickedUp;
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
        // TODO: Check if in range of carrot! && playerState.CanPickup
        playerState.CurrentAction = PlayerAction.PickingUp;
    }

    private void OnPickupPerformed(InputAction.CallbackContext obj)
    {
        playerState.CurrentAction = PlayerAction.Carrying;
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
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public event Action OnPickedUp;

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
        pickupAction.performed += OnPickup;
        throwAction.performed += OnThrow;
    }

    private void Update()
    {
        var moveAmount = moveAction.ReadValue<Vector2>();
        rigidbody.velocity += moveAmount * (Time.fixedDeltaTime * speedMultiplier);
    }

    private void OnPickup(InputAction.CallbackContext obj)
    {
    }

    private void OnThrow(InputAction.CallbackContext obj)
    {
    }
}
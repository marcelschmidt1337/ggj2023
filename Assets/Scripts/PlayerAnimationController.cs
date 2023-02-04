using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerState playerState;
    [SerializeField] private Animator animator;
    
    private static readonly int Walking = Animator.StringToHash("Walking");

    private void Awake()
    {
        playerState.OnActionChanged += OnPlayerActionChanged;
    }

    private void LateUpdate()
    {
        animator.SetBool(Walking, playerState.IsWalking);
    }

    private void OnPlayerActionChanged(PlayerAction action)
    {
        animator.SetTrigger(action.ToString());
    }
}

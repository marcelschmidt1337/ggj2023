using System;
using UnityEngine;

public enum PlayerAction
{
    None,
    PickingUp,
    Carrying,
    Throwing,
    Stunned,
}

public class PlayerState : MonoBehaviour
{
    public event Action<PlayerAction> OnActionChanged;

    private PlayerAction currentAction = PlayerAction.None;

    public PlayerAction CurrentAction
    {
        get => currentAction;
        set
        {
            OnActionChanged?.Invoke(value);
            currentAction = value;
        }
    }

    public bool IsWalking => WalkDirection.magnitude > 0f;
    public Vector2 WalkDirection { get; set; }
    
    public Transform ObjectCarrying { get; set; }
    
    public bool CanWalk => CurrentAction != PlayerAction.Stunned && CurrentAction != PlayerAction.PickingUp && CurrentAction != PlayerAction.Throwing;
    public bool CanPickUp => CurrentAction != PlayerAction.Stunned && CurrentAction != PlayerAction.Carrying && CurrentAction != PlayerAction.Throwing;
}

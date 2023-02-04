using System;
using UnityEngine;
public enum ProjectileState
{
    Grounded,
    Idle,
    Fired
}

public class ProjectileStateController : MonoBehaviour
{
    [HideInInspector] public event Action<ProjectileState> OnStateChanged;
    [SerializeField] private ProjectileFiredStateController ProjectileFiredStateController;
    [SerializeField] private ProjectileGroundedStateController ProjectileGroundedStateController;

    [SerializeField] private Rigidbody2D rigigBody;
    [SerializeField] private Collider2D collider;

    private AProjectileState selectedState;
    private ProjectileState projectileState;

    public void Start()
    {
        ActivateState(ProjectileState.Idle);
    }
    public void FireProjectile(float strength, int direction)
    {
        ActivateState(ProjectileState.Fired);
        ProjectileFiredStateController.Fire(strength, direction);
        ProjectileFiredStateController.OnLandedAction += () => ActivateState(ProjectileState.Grounded);
    }
    private void ActivateState(ProjectileState newState)
    {
        projectileState = newState;
        OnStateChanged?.Invoke(projectileState);
        switch (projectileState)
        {
            case ProjectileState.Grounded:
                Debug.Log("Grounded!");
                selectedState = ProjectileGroundedStateController;
                break;
            case ProjectileState.Idle:
                Debug.Log("Idle!");
                break;
            case ProjectileState.Fired:
                Debug.Log("Fire!");
                selectedState = ProjectileFiredStateController;
                break;
        }
    }

    public void Update()
    {
        selectedState?.Update();
    }
    public void OnDrawGizmos()
    {
        selectedState?.OnDrawGizmos();
    }

}
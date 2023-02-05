using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
public enum ProjectileState
{
    Grounded,
    Idle,
    Fired,
    Broken
}


public class ProjectileStateController : MonoBehaviour
{
    [HideInInspector] public event Action<ProjectileState> OnStateChanged;

    [SerializeField] private Rigidbody2D rigigBody;
    [SerializeField] private CircleCollider2D collider2D;
    [SerializeField] private Animator projectileAnimator;

    [SerializeField] private ProjectileFiredStateController ProjectileFiredStateController;
    [SerializeField] private ProjectileGroundedStateController ProjectileGroundedStateController;

    private AProjectileStateController selectedStateController;
    private ProjectileState projectileState;

    private SoundManager soundManager;

    public void Start()
    {
        ActivateGround();
        soundManager = GameObject.FindWithTag("Sound")?.GetComponent<SoundManager>();
    }

    public void FireProjectile(float strength, int direction)
    {
        ActivateState(ProjectileState.Fired);
        ProjectileFiredStateController.Fire(strength, direction);
        ProjectileFiredStateController.OnLandedAction -= ActivateGround;
        ProjectileFiredStateController.OnLandedAction += ActivateGround;
    }

    private void ActivateGround()
    {
        ActivateState(ProjectileState.Grounded);
    }

    private void ActivateState(ProjectileState newState)
    {
        if (projectileState == ProjectileState.Fired && newState == ProjectileState.Grounded)
        {
            soundManager.PlaySfx(SoundManager.Sfx.Landing);
        }

        projectileState = newState;
        OnStateChanged?.Invoke(projectileState);
        switch (projectileState)
        {
            case ProjectileState.Grounded:
                Debug.Log("grounded");
                selectedStateController = ProjectileGroundedStateController;
                break;
            case ProjectileState.Fired:
                Debug.Log("fired");

                selectedStateController = ProjectileFiredStateController;
                break;
        }

        selectedStateController.Init(this.transform, this.projectileAnimator, this.collider2D);
    }

    public void Update()
    {
        selectedStateController?.Update();
    }

    public void OnDrawGizmos()
    {
        selectedStateController?.OnDrawGizmos();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        selectedStateController?.OnTriggerEnter2D(other);
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        selectedStateController?.OnTriggerStay2D(other);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        selectedStateController?.OnTriggerExit2D(other);
    }
}
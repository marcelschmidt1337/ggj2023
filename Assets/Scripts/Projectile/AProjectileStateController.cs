using System.Security.Cryptography.X509Certificates;
using System;
using UnityEngine;


public abstract class AProjectileStateController
{
    protected Transform transform;
    protected Animator animationController;
    protected CircleCollider2D collider;
    protected abstract ProjectileState AnimationState { get; set; }

    public void Init(Transform t, Animator a, CircleCollider2D c)
    {
        this.transform = t;
        this.animationController = a;
        this.collider = c;
        this.animationController.Play(AnimationState.ToString());
    }

    public abstract void Update();
    public abstract void OnDrawGizmos();
    public abstract void OnTriggerEnter2D(Collider2D other);
    public virtual void OnTriggerStay2D(Collider2D other)
    {

    }
    public virtual void OnTriggerExit2D(Collider2D other)
    {

    }

}
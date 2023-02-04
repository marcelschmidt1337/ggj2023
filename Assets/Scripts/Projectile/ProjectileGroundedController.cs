using UnityEngine;
using System;

[Serializable]
public class ProjectileGroundedStateController : AProjectileStateController
{

    protected override ProjectileState AnimationState { get; set; } = ProjectileState.Grounded;

    public override void Update()
    {

    }
    public override void OnDrawGizmos()
    {
    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("enter " + other.gameObject.layer);
            SpeedUpAnimation(other);
        }

    }

    public override void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("stay " + other.gameObject.layer);
            SpeedUpAnimation(other);


        }

    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
        }

    }

    private void SpeedUpAnimation(Collider2D other)
    {
        float maxLength =  ((Vector2)other.bounds.size).magnitude/2.0f +  this.collider.radius;
        float distancePercent = (other.transform.position - this.transform.position).magnitude / maxLength;
        Debug.Log(distancePercent);
        distancePercent = (1 - distancePercent) * 2;
        this.animationController.speed = 1;
    }
}
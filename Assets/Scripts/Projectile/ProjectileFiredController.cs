using System;
using UnityEngine;

[Serializable]
public class ProjectileFiredStateController : AProjectileStateController
{
    public event Action OnLandedAction;

    [Range(0.1f, 5)] public float TimeOfTravel;
    protected override ProjectileState AnimationState { get; set; } = ProjectileState.Fired;
    [SerializeField] private AnimationCurve ScaleComponent;
    [SerializeField] private float extraScale;
    [SerializeField] private float collisionActivationThreshold;


    private double travelFinishTime;
    private float velocity = 0;
    private Vector3 initialScale;

    public override void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(this.transform.position, ComputeLapsedTimePercent() * Vector3.one);
    }
    public override void Init(Transform t, Animator a, CircleCollider2D c)
    {
        base.Init(t, a, c);
        this.initialScale = this.transform.localScale;

    }
    public override void Update()
    {
        if (velocity != 0)
        {
            float progress = ComputeLapsedTimePercent();

            if (progress == 1)
            {
                Stop();
                return;
            }
            MoveProjectile();
            this.transform.localScale = Vector3.Lerp(initialScale, initialScale + Vector3.one * extraScale, ScaleComponent.Evaluate(progress));
            this.animationController.Play(AnimationState.ToString(), 0, progress);
        }
    }
    public void Fire(float travelDistance, int direction)
    {
        velocity = direction * (travelDistance / TimeOfTravel);
        travelFinishTime = Time.time + TimeOfTravel;


    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (ComputeLapsedTimePercent() >= collisionActivationThreshold && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<PlayerController>().StunPlayer();
        }

    }
    private float ComputeLapsedTimePercent()
    {
        return 1.0f - Mathf.Max(0, (float)(travelFinishTime - Time.timeAsDouble) / TimeOfTravel);
    }

    private void MoveProjectile()
    {

        var position = this.transform.position;
        position.x = this.transform.position.x + velocity * Time.deltaTime;
        position.x = Camera.main.WorldToViewportPoint(position).x % 1.0f;
        if (Mathf.Sign(position.x) < 1)
        {
            position.x = 1.0f + position.x;
        }
        position.x = Camera.main.ViewportToWorldPoint(position).x;
        this.transform.position = position;

    }
    private void Stop()
    {
        velocity = 0;
        travelFinishTime = Time.timeAsDouble;
        this.transform.localScale = initialScale;
        Debug.Log("landed");
        OnLandedAction?.Invoke();
    }

}
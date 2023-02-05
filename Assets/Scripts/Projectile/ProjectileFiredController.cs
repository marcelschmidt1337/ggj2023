using System;
using UnityEngine;

[Serializable]
public class ProjectileFiredStateController : AProjectileStateController
{
    public event Action OnLandedAction;
    public event Action<string> OnBounce;

    [Range(0.1f, 5)] public float TimeOfTravel;
    protected override ProjectileState AnimationState { get; set; } = ProjectileState.Fired;
    [SerializeField] private AnimationCurve ScaleComponent;
    [SerializeField] private float extraScale;
    [SerializeField] private float collisionActivationThreshold;


    private double travelFinishTime;
    private Vector2 velocity;
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
        if (velocity.magnitude != 0)
        {
            float progress = ComputeLapsedTimePercent();

            if (progress == 1)
            {
                Stop();
                OnLandedAction?.Invoke();
                return;
            }
            MoveProjectile();
            this.transform.localScale = Vector3.Lerp(initialScale, initialScale + Vector3.one * extraScale, ScaleComponent.Evaluate(progress));
            this.animationController.Play(AnimationState.ToString(), 0, progress);
        }
    }
    public void Fire(float travelDistance, int direction)
    {
        velocity = Vector2.right * direction * (travelDistance / TimeOfTravel);
        travelFinishTime = Time.time + TimeOfTravel;
    }
    public void Fire(float travelDistance, Vector2 direction)
    {
        velocity = direction * (travelDistance / TimeOfTravel);
        travelFinishTime = Time.time + TimeOfTravel;
    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
    }
    public override void OnTriggerStay2D(Collider2D other)
    {
        if (ComputeLapsedTimePercent() >= collisionActivationThreshold)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                other.GetComponent<PlayerController>().StunPlayer();
                Fire(UnityEngine.Random.Range(2, 4), UnityEngine.Random.insideUnitCircle);
                OnBounce?.Invoke(LayerMask.LayerToName(other.gameObject.layer));
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Water") || other.gameObject.layer == LayerMask.NameToLayer("Root"))
            {
                Fire(UnityEngine.Random.Range(3, 4), UnityEngine.Random.insideUnitCircle);
                OnBounce?.Invoke(LayerMask.LayerToName(other.gameObject.layer));
            }
        }
    }
    private float ComputeLapsedTimePercent()
    {
        return 1.0f - Mathf.Max(0, (float)(travelFinishTime - Time.timeAsDouble) / TimeOfTravel);
    }

    private void MoveProjectile()
    {

        var position = this.transform.position;
        position = this.transform.position + (Vector3)velocity * Time.deltaTime;
        position = Camera.main.WorldToViewportPoint(position);
        position.x %= 1.0f;
        position.y %= 1.0f;
        if (Mathf.Sign(position.x) < 1)
        {
            position.x = 1.0f + position.x;
        }

        if (Mathf.Sign(position.y) < 1)
        {
            position.y = 1.0f + position.y;
        }
        position = Camera.main.ViewportToWorldPoint(position);
        position.z = 0;
        this.transform.position = position;

    }
    private void Stop()
    {
        velocity = Vector2.zero;
        travelFinishTime = Time.timeAsDouble;
        this.transform.localScale = initialScale;
        Debug.Log("landed");
    }

}
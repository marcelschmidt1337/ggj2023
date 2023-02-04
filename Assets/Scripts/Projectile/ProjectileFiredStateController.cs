using System;
using UnityEngine;

[Serializable]
public class ProjectileFiredStateController : AProjectileState
{

    public event Action OnLandedAction;

    [Range(0.1f, 5)] public float TimeOfTravel;
    [SerializeField] private Transform transform;

    private double travelFinishTime;
    private (float initial, float final) travelSegment;
    private float velocity = 0;

    public override void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position, ComputeLapsedTimePercent() * Vector3.one);
    }

    public override void Update()
    {
        if (velocity != 0)
        {
            float progress = ComputeLapsedTimePercent();
            if (progress == 0)
            {
                Stop();
                OnLandedAction?.Invoke();
            }
            MoveProjectile();
        }
    }
    public void Fire(float travelDistance, int direction)
    {
        velocity = direction * (travelDistance / TimeOfTravel);
        travelFinishTime = Time.time + TimeOfTravel;
        travelSegment = (transform.position.x, transform.position.x + direction * travelDistance);

    }
    private float ComputeLapsedTimePercent()
    {
        return Mathf.Max(0, (float)(travelFinishTime - Time.timeAsDouble) / TimeOfTravel);
    }

    private void MoveProjectile()
    {
        var position = this.transform.position;
        position.x = Mathf.Clamp(this.transform.position.x + velocity * Time.deltaTime, travelSegment.initial, travelSegment.final);
        this.transform.position = position;
    }
    private void Stop()
    {
        velocity = 0;
        travelFinishTime = Time.timeAsDouble;
    }

}
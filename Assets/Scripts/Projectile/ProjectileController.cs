using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ProjectileFiredStateController : MonoBehaviour
{

    public Rigidbody2D rigigBody;
    public Collider2D collider;

    [Range(0.1f, 5)] public float TimeOfTravel;

    private double travelFinishTime;
    private (float initial, float final) travelSegment;
    private float velocity = 0;

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position, ComputeLapsedTimePercent() * Vector3.one);
    }

    public void Update()
    {
        if (velocity != 0)
        {
            float progress = ComputeLapsedTimePercent();
            if (progress == 0)
            {
                velocity = 0;
                travelFinishTime = Time.timeAsDouble;
            }
            var position = this.transform.position;
            position.x = Mathf.Clamp(this.transform.position.x + velocity * Time.deltaTime, travelSegment.initial, travelSegment.final);
            this.transform.position = position;
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

}
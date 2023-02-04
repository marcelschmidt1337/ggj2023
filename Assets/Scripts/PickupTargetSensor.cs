using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PickupTargetSensor : MonoBehaviour
{
    public bool HasPickupTarget => CurrentPickupTarget != null;
    
    public Collider2D CurrentPickupTarget { get; private set; }
    
    private readonly List<Collider2D> targetsInRange = new();

    protected void OnValidate()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (targetsInRange.Contains(other)) return;
        
        targetsInRange.Add(other);
        
        UpdateCurrentInteractable();
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (targetsInRange.Contains(other))
        {
            targetsInRange.Remove(other);
        }
        
        UpdateCurrentInteractable();
    }

    private void UpdateCurrentInteractable()
    {
        CurrentPickupTarget = targetsInRange.Count > 0 ? targetsInRange[^1] : null;
   
        Debug.Log($"Current pickup target: {CurrentPickupTarget}");
    }
}

using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Transform carrotSpawnP1;
    [SerializeField] private Transform carrotSpawnP2;
    [SerializeField] private List<GameObject> carrotPattern;

    private void Start()
    {
        var indexP1 = Random.Range(0, carrotPattern.Count);
        var carrotsP1 = carrotPattern[indexP1];
        var p = Instantiate(carrotsP1, carrotSpawnP1.position, carrotSpawnP1.rotation, transform);
        
        //Rotate individual carrots back so they face the right direction
        foreach (var c in p.GetComponentsInChildren<Transform>())
        {
            c.localEulerAngles = -carrotSpawnP1.rotation.eulerAngles;
        }

        var indexP2 = Random.Range(0, carrotPattern.Count);
        var carrotsP2 = carrotPattern[indexP2];
        Instantiate(carrotsP2, carrotSpawnP2.position, carrotSpawnP2.rotation, transform);
    }
}
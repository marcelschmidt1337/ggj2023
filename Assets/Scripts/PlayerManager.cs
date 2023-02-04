using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform p1Spawn;
    [SerializeField] private Transform p2Spawn;

    private void Start()
    {
        var p1 = PlayerInput.Instantiate(playerPrefab, playerIndex: 1, controlScheme: "Keyboard Left", pairWithDevice: Keyboard.current);
        var p1Transform = p1.transform;
        p1Transform.position = p1Spawn.position;
        p1Transform.rotation = p1Spawn.rotation;

        var p2 = PlayerInput.Instantiate(playerPrefab, playerIndex: 2, controlScheme: "Keyboard Right", pairWithDevice: Keyboard.current);
        var p2Transform = p2.transform;
        p2Transform.position = p2Spawn.position;
        p2Transform.rotation = p2Spawn.rotation;
    }
}
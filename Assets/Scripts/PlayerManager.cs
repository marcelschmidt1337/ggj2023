using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private void Start()
    {
        PlayerInput.Instantiate(playerPrefab, playerIndex: 1, controlScheme: "Keyboard Left", pairWithDevice: Keyboard.current);
        PlayerInput.Instantiate(playerPrefab, playerIndex: 2, controlScheme: "Keyboard Right", pairWithDevice: Keyboard.current);
    }
}
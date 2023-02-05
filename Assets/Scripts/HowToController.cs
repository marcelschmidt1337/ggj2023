using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToController : MonoBehaviour
{
    [UsedImplicitly]
    public void OnReturnToMainMenuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

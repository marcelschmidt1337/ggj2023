using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    [UsedImplicitly]
    public void OnReturnToMainMenuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

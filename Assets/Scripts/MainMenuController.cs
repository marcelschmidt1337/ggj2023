using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    //Linked through the inspector
    [UsedImplicitly]
    public void OnStartGameButtonClicked()
    {
        SceneManager.LoadScene("SampleScene");
    }
    
    [UsedImplicitly]
    public void OnCreditsButtonClicked()
    {
        SceneManager.LoadScene("Credits");
    }
}

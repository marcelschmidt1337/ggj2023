using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    //Linked through the inspector
    [UsedImplicitly]
    public void OnStartGameButtonClicked()
    {
        SceneManager.LoadScene("SampleScene");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    private static readonly int ShowHash = Animator.StringToHash("Show");
    private static readonly int Won = Animator.StringToHash("Won");

    public void Show(bool hasWon)
    {
        gameObject.SetActive(true);

        animator.SetBool(ShowHash, true);
        animator.SetBool(Won, hasWon);
    }

    public void Replay()
    {
        // animator.SetBool(ShowHash, false);
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
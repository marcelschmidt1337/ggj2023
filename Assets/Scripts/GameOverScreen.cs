using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    private static readonly int ShowHash = Animator.StringToHash("Show");
    private static readonly int Player1Won = Animator.StringToHash("Player1Won");
    private static readonly int Player2Won = Animator.StringToHash("Player2Won");
    private static readonly int Draw = Animator.StringToHash("Draw");

    public void Show(GameResult result)
    {
        gameObject.SetActive(true);

        animator.SetBool(ShowHash, true);
        animator.SetBool(Player1Won, result == GameResult.Player1Wins);
        animator.SetBool(Player2Won, result == GameResult.Player2Wins);
        animator.SetBool(Draw, result == GameResult.Draw);
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
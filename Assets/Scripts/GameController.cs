using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private int matchDurationSeconds = 180;

    private float matchTimeElapsed;

    private void Awake()
    {
        StartCoroutine(StartMatch());
    }

    private IEnumerator StartMatch()
    {
        while (true)
        {
            // Debug.Log($"Time elapsed: {matchTimeElapsed} / {matchDurationSeconds} s");
        
            matchTimeElapsed += Time.deltaTime;

            if (matchTimeElapsed > matchDurationSeconds)
            {
                GameOver();
            
                yield break;
            }

            yield return null;
        }
    }

    public void GameOver()
    {
        var player1Won = false;
        gameOverScreen.Show(player1Won);
    }
}
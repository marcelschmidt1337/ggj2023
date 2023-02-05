using System.Collections;
using UnityEngine;

public enum GameResult
{
    Player1Wins,
    Player2Wins,
    Draw
}

public class GameController : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;
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
        //TODO: Stop game and players!

        gameOverScreen.Show(mapManager.GetGameResult());
    }
}
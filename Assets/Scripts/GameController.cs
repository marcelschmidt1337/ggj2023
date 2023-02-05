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
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private int matchDurationSeconds = 180;
    [SerializeField] private int musicSpeedUpThreshold = 60;

    private float matchTimeElapsed;
    private bool isSpeedUpTriggered;

    private void Awake()
    {
        isSpeedUpTriggered = false;
        StartCoroutine(StartMatch());
    }

    private IEnumerator StartMatch()
    {
        while (true)
        {
            var timeLeft = matchDurationSeconds - matchTimeElapsed;
            countdownTimer.UpdateTimeLeft(timeLeft);

            matchTimeElapsed += Time.deltaTime;

            if (!isSpeedUpTriggered && matchDurationSeconds - matchTimeElapsed <= musicSpeedUpThreshold)
            {
                isSpeedUpTriggered = true;
                soundManager.SpeedUpMusic();
            }

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
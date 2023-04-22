using UnityEngine;
using System;

public class GameManager : Singleton<GameManager>
{
    public event Action<bool> OnPauseStatusChange;
    public event Action OnGameOver;
    public event Action OnDead;
    public event Action OnGameWin;

    [Header("Bool Puzzle?")]
    public bool Puzzle1; //da caixa
    public bool Puzzle2;// Miranha
    public bool Puzzle3;// Click

    public bool Paused { get; private set; }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    public bool CanPause = true;

    private void PauseGame()
    {
        Paused = true;
        Time.timeScale = 0;
        OnPauseStatusChange?.Invoke(Paused);
    }

    public void ResumeGame()
    {
        Paused = false;
        Time.timeScale = 1;
        OnPauseStatusChange?.Invoke(Paused);
    }

    public void GameOver()
    {
        Paused = true;
        Time.timeScale = 0;
        OnPauseStatusChange?.Invoke(Paused);
        OnGameOver?.Invoke();
    }

    public void PauseResume()
    {
        if (!CanPause)
            return;

        if (Paused)
            ResumeGame();
        else
            PauseGame();
    }

    public void GameWin()
    {
        OnGameWin?.Invoke();
    }

    public void TemporaryPause()
    {
        Paused = true;
        Time.timeScale = 0;
    }

    public void ResumeTemporaryPause()
    {
        Paused = false;
        Time.timeScale = 1;
    }
}

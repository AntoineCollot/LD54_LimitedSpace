using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public bool gameIsOver { get; private set; }
    public bool gameHasStarted { get; private set; }
    public bool GameIsPlaying => !gameIsOver && gameHasStarted;
    public bool autoStart = true;

    InputMap inputMap;

    public UnityEvent onGameStart = new UnityEvent();
    public UnityEvent onGameOver = new UnityEvent();
    public UnityEvent onGameWin = new UnityEvent();

    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
        if (autoStart)
            StartGame();
    }

    public void StartGame()
    {
        if (gameHasStarted)
            return;
        gameHasStarted = true;
        onGameStart.Invoke();

        inputMap = new InputMap();
        inputMap.Enable();
        inputMap.Gameplay.Restart.performed += OnRestart;
    }

    private void OnDestroy()
    {
        inputMap.Disable();
        inputMap.Dispose();
    }

    private void OnRestart(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        GameOver();
    }

    public void GameOver()
    {
        if (gameIsOver)
            return;
        gameIsOver = true;
        onGameOver.Invoke();
    }

    public void ClearLevel()
    {
        if (gameIsOver)
            return;

        gameIsOver = true;
        onGameWin.Invoke();
    }
}

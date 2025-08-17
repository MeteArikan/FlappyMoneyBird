using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static Action<int> OnSetScoreUI;
    public static Action OnShowScoreBoard;
    public static Action OnAfterGameOver;
    public static Action OnGameMenuOpened;
    public static Action OnGameStarted;


    private int _score;
    private bool _isGameStarted = false;
    private bool _isNewBestScore = false;
    private GameMode _currentGameMode = GameMode.DefaultMode;
    
    public int GetScore => _score;
    public bool IsNewBestScore => _isNewBestScore;
    //public bool IsGameStarted => _isGameStarted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        //Time.timeScale = 0f;
        //PlayerPrefs.SetInt("highScore", 0);
        //SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.activeSceneChanged += OnNewSceneLoaded;
        
    }



    private void OnDisable()
    {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.activeSceneChanged -= OnNewSceneLoaded;

    }

    private void OnNewSceneLoaded(Scene arg0, Scene arg1)
    {
        _currentGameMode = GameModeController.Instance.GetGameMode();

        // Log the current game mode to the console.
        Debug.Log("Current Game Mode: " + _currentGameMode);
        OpenMenu();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OpenMenu();
    }

    private void OpenMenu()
    {
        //Time.timeScale = 0f;
        OnGameMenuOpened?.Invoke(); // Invoke the new OnGamePaused event
        _isGameStarted = false;
        Debug.Log("Menu screen is opened"); // For debugging
    }

    public void OnGameStart()
    {
        Time.timeScale = 1f;
        _score = 0;
        _isNewBestScore = false;
        OnGameStarted?.Invoke();
        OnSetScoreUI?.Invoke(_score);
        _isGameStarted = true;
        Debug.Log("Game Started");
    }


    private void Update()
    {
        // Check for Space key press only if the game hasn't started yet
        if (!_isGameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            OnGameStart();
        }
    }


    public void IncreaseScore()
    {
        _score++;
        OnSetScoreUI?.Invoke(_score);
    }

    public void OnGameOver()
    {
        if (_score > PlayerPrefs.GetInt("highScore"))
        {
            PlayerPrefs.SetInt("highScore", _score);
            _isNewBestScore = true;
        }
        OnAfterGameOver?.Invoke();
        Invoke(nameof(ShowScoreBoard), 0.5f);
        
        
    }

    public int GetHighScore() => PlayerPrefs.GetInt("highScore");


    public void RestartGame()
    {
        AudioManager.Instance.Play(SoundType.TransitionSound);
        //SceneManager.LoadScene("DefaultMode");
        SceneManager.LoadScene(_currentGameMode.ToString());
    }

    public void NavigateToGameMode(GameMode gameMode)
    {
        if(gameMode == _currentGameMode)
        {
            Debug.Log("Already in the selected game mode: " + gameMode);
            return; // No need to change scene if already in the selected game mode
        }
        GameModeController.Instance.ChangeGameMode(gameMode);
        SceneManager.LoadScene(gameMode.ToString());
    }


    public void ShowScoreBoard()
    {
        //Time.timeScale = 0f;
        OnShowScoreBoard?.Invoke();
    }
}

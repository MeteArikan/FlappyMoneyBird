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
    
    public int GetScore => _score;
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
        Time.timeScale = 0f;
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
        }
        OnAfterGameOver?.Invoke();
        Invoke(nameof(ShowScoreBoard), 0.5f);
        
        
    }

    public int GetHighScore() => PlayerPrefs.GetInt("highScore");


    public void RestartGame()
    {
        AudioManager.Instance.Play(SoundType.TransitionSound);
        SceneManager.LoadScene("SampleScene");
    }


    public void ShowScoreBoard()
    {
        //Time.timeScale = 0f;
        OnShowScoreBoard?.Invoke();
    }
}

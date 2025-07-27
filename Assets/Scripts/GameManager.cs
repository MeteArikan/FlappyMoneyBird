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

    private int _score;

    public int GetScore => _score;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnGameStart();
    }
    public void OnGameStart()
    {
        Time.timeScale = 1f;
        _score = 0;
        OnSetScoreUI?.Invoke(_score);
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

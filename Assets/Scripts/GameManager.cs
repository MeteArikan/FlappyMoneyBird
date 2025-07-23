using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static Action<int> OnSetScoreUI;
    public static Action OnGameOverEvent;

    //private TMP_Text _scoreText;
    private GameObject _deathScreen;
    private TMP_Text _highScoreText;

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

        // Make sure your high score text GameObject has the "HighScoreText" tag
        _highScoreText = GameObject.Find("HighScoreText").GetComponent<TMP_Text>();
        //_scoreText = GameObject.FindWithTag("ScoreText").GetComponent<TMP_Text>();
        _deathScreen = GameObject.Find("DeathScreen");
        OnGameStart();

        // if (_scoreText != null)
        //     _scoreText.text = _score.ToString();
    }
    public void OnGameStart()
    {
        if (_deathScreen != null)
            _deathScreen.SetActive(false);
        Time.timeScale = 1f;
        _score = 0;
        OnSetScoreUI?.Invoke(_score);
        _highScoreText.text = "High Score: " + PlayerPrefs.GetInt("highScore");
    }

    public void IncreaseScore()
    {
        _score++;
        OnSetScoreUI?.Invoke(_score);
        // if (_scoreText != null)
        //     _scoreText.text = _score.ToString();
    }

    public void OnGameOver()
    {
        if (_score > PlayerPrefs.GetInt("highScore"))
        {
            PlayerPrefs.SetInt("highScore", _score);
        }
        _highScoreText.text = "High Score: " + PlayerPrefs.GetInt("highScore");
        OnGameOverEvent?.Invoke();
        Invoke(nameof(StopTime), 1f);
        
    }


    public void RestartGame()
    {
        AudioManager.Instance.Play(SoundType.TransitionSound);
        SceneManager.LoadScene("SampleScene");
    }

    public void StopTime()
    {
        Time.timeScale = 0f;
        _deathScreen.SetActive(true);
    }
}

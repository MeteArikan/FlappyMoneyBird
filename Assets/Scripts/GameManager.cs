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
    public static Action OnComboCountUpdated;
//    public static event Action OnUnlocksUpdated;


    private int _score;
    private int _comboCount = 0;
    private int _maxComboCount = 0;
    private bool _isGameStarted = false;
    private bool _isNewBestScore = false;
    private GameMode _currentGameMode = GameMode.DefaultMode;
    
    public int GetScore => _score;
    public bool IsNewBestScore => _isNewBestScore;


    public int GetComboCount => _comboCount;
    public int GetMaxComboCount => _maxComboCount;
    //public bool IsGameStarted => _isGameStarted;
    // Unlock flags
    public bool IsMoneyModeUnlocked => PlayerPrefs.GetInt("MoneyMode_Unlocked", 0) == 1;
    public bool IsHardModeUnlocked => PlayerPrefs.GetInt("HardMode_Unlocked", 0) == 1;


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
        LockModes();
    }



    private void OnDisable()
    {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.activeSceneChanged -= OnNewSceneLoaded;

    }

    private void OnNewSceneLoaded(Scene arg0, Scene arg1)
    {
        _currentGameMode = GameModeController.Instance.GetGameMode();
        //        CheckUnlocks();


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

        if (GameModeController.Instance.GetGameMode() == GameMode.MoneyMode)
        {
            _comboCount = 0;
            _maxComboCount = 0;
            OnComboCountUpdated?.Invoke();
        }

        Debug.Log("Game Started");
    }


    private void Update()
    {
        // Check for Space key press only if the game hasn't started yet
        // if (!_isGameStarted && Input.GetKeyDown(KeyCode.Space))
        // {
        //     OnGameStart();
        // }
        if (!_isGameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            if (!IsCurrentModeUnlocked())
            {
                Debug.Log("Mode is locked. Cannot start game.");
                return; // ❌ Prevent start
            }

            OnGameStart(); // ✅ Only runs if unlocked
        }
    }


    public void IncreaseScore()
    {
        _score++;
        OnSetScoreUI?.Invoke(_score);
    }
    

    // Money mode specific method : money combo counter
    public void IncreaseComboCount()
    {
        _comboCount++;
        OnComboCountUpdated?.Invoke();
        if (_comboCount > _maxComboCount)
        {
            _maxComboCount = _comboCount;
        }
    }
    // Money mode specific method : RESET money combo counter
    public void ResetComboCount()
    {
        _comboCount = 0;
        OnComboCountUpdated?.Invoke();
    }

    public int GetBonusPoints()
    {
        return _maxComboCount * 3;
    }

    public void OnGameOver()
    {
        CheckHighscore(_score);
        CheckUnlocks(); // <-- Make sure this is called here
        OnAfterGameOver?.Invoke();
        Invoke(nameof(ShowScoreBoard), 0.5f);


    }

    // public void CheckHighscore(int score)
    // {
    //     if (score > PlayerPrefs.GetInt("highScore"))
    //     {
    //         PlayerPrefs.SetInt("highScore", score);
    //         _isNewBestScore = true;
    //     }
    // }

    // public int GetHighScore() => PlayerPrefs.GetInt("highScore");
    public void CheckHighscore(int score)
    {
        string key = $"{_currentGameMode}_highScore"; // e.g. "DefaultMode_highScore"
        if (score > PlayerPrefs.GetInt(key, 0)) // default 0 if not set
        {
            PlayerPrefs.SetInt(key, score);
            _isNewBestScore = true;
        }
        //CheckUnlocks();
    }

    // Check unlock requirements
    public void CheckUnlocks()
    {
        // Unlock MoneyMode if DefaultMode high score >= 10
        if (!IsMoneyModeUnlocked && GetHighScore(GameMode.DefaultMode) >= 5)
        {
            PlayerPrefs.SetInt("MoneyMode_Unlocked", 1);
            //OnUnlocksUpdated?.Invoke();
            Debug.Log("MoneyMode unlocked!");
        }

        // Unlock HardMode if MoneyMode max combo >= 6
        if (!IsHardModeUnlocked && GetHighScore(GameMode.MoneyMode) >= 0 && GetMaxComboCount >= 6)
        {
            PlayerPrefs.SetInt("HardMode_Unlocked", 1);
            //OnUnlocksUpdated?.Invoke();
            Debug.Log("HardMode unlocked!");
        }
    }

    public bool IsCurrentModeUnlocked()
    {
        return _currentGameMode switch
        {
            GameMode.MoneyMode => IsMoneyModeUnlocked,
            GameMode.HardMode => IsHardModeUnlocked,
            _ => true,// DefaultMode is always unlocked
        };
    }

    // test function
    public void LockModes()
    {
        PlayerPrefs.SetInt("MoneyMode_Unlocked", 0);
        PlayerPrefs.SetInt("HardMode_Unlocked", 0);
        //OnUnlocksUpdated?.Invoke();
        Debug.Log("All modes locked for testing.");
    }

    public int GetHighScore(GameMode mode)
    {
        string key = $"{mode}_highScore";
        return PlayerPrefs.GetInt(key, 0);
    }

    public int GetCurrentModeHighScore() => GetHighScore(_currentGameMode);



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

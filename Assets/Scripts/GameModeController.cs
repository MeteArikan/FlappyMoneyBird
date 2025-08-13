using UnityEngine;

public class GameModeController : MonoBehaviour
{

    public static GameModeController Instance { get; private set; }
    private GameMode _currentGameMode = GameMode.DefaultMode;


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

    private void Start()
    {
        ChangeGameMode(GameMode.DefaultMode);
    }
    public void ChangeGameMode(GameMode newGameMode)
    {
        if (_currentGameMode == newGameMode)
        {
            return;
        }
        _currentGameMode = newGameMode;

    }

    public GameMode GetGameMode()
    {
        return _currentGameMode;
    }
    
}

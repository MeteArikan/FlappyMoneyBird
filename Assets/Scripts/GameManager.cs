using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private TMP_Text _scoreText;
    private int _score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        _score = 0;
        _scoreText.text = _score.ToString();
    }

    public void IncreaseScore()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }
}

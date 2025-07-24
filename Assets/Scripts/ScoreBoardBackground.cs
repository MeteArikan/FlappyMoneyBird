using System;
using UnityEngine;

public class ScoreBoardBackground : MonoBehaviour
{
    [SerializeField] private RectTransform _scoreArea;
    [SerializeField] private RectTransform _bestScoreArea;
    [SerializeField] private float _moveAmount = 30f; // Amount to move left per digit increase

    private int _lastDigitCount = 2;
    private ScoreController _bestScoreController;

    private void Start()
    {
        _bestScoreController = _bestScoreArea.GetComponentInChildren<ScoreController>();
        _bestScoreController.enabled = false;
    }

    private void OnEnable()
    {
        GameManager.OnSetScoreUI += GameManager_OnSetScoreUI;
        //_bestScoreArea.GetComponent<ScoreController>().enabled = false;
        GameManager.OnSetBestScore += GameManager_OnSetBestScore;
    }

    private void OnDisable()
    {
        GameManager.OnSetScoreUI -= GameManager_OnSetScoreUI;
        GameManager.OnSetBestScore-= GameManager_OnSetBestScore;
    }

    // private void GameManager_OnSetScoreUIOLDVersion(int score)
    // {
    //     int digitCount = score.ToString().Length;
    //     if (digitCount > _lastDigitCount)
    //     {
    //         // Move left by _moveAmount for each new digit
    //         _scoreArea.anchoredPosition += new Vector2(-_moveAmount, 0);
    //         _lastDigitCount = digitCount;
    //     }
    // }
    
        private void GameManager_OnSetScoreUI(int score)
    {
        int digitCount = score.ToString().Length;
        if (digitCount > _lastDigitCount)
        {
            // Move the digits to the left
            if (digitCount == 3)
            {
                _scoreArea.anchoredPosition += new Vector2(-_moveAmount * (digitCount - 1), 0);
            }
            else
            {
                _scoreArea.anchoredPosition += new Vector2(-_moveAmount, 0);
            }
            _lastDigitCount = digitCount;
        }
    }


    private void GameManager_OnSetBestScore()
    {
        int bestScore = GameManager.Instance.GetHighScore();
        _bestScoreController.enabled = true;
        _bestScoreController.UpdateScoreDisplay(bestScore);
        SetBestScoreUI(bestScore);


    }

    private void SetBestScoreUI(int score)
    {
        int digitCount = score.ToString().Length;
        if (digitCount > 2)
        {
            // Move left by _moveAmount for each new digit
            _bestScoreArea.anchoredPosition += new Vector2(-_moveAmount * (digitCount - 1), 0);
        //     _lastDigitCount = digitCount;
        }
    }

}

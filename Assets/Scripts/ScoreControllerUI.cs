using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private Sprite[] _scoreDigitSpriteList; // 0-9 sprites
    [SerializeField] private GameObject _scoreDigitPrefab;    // Prefab with Image component
    //[SerializeField] private float _digitSpacing = 2000f;       // Space between digits

    private GameObject[] _scoreDigits = new GameObject[0];
    //private int _lastScore = -1;

    private void OnEnable()
    {
        GameManager.OnSetScoreUI += GameManager_OnSetScoreUI;
    }

    void OnDisable()
    {
        GameManager.OnSetScoreUI -= GameManager_OnSetScoreUI;
    }

    private void GameManager_OnSetScoreUI(int score)
    {
        UpdateScoreDisplay(score);
    }



    // void Update()
    // {
    //     int score = GameManager.Instance != null ? GameManager.Instance.GetScore : 0;
    //     if (score != _lastScore)
    //     {
    //         UpdateScoreDisplay(score);
    //         _lastScore = score;
    //     }
    // }

    public void UpdateScoreDisplay(int score)
    {
        // Remove old digits
        foreach (var digitObj in _scoreDigits)
            Destroy(digitObj);
        CaptureScore(score);
    }

    private void CaptureScore(int score)
    {
        string scoreStr = score.ToString();
        _scoreDigits = new GameObject[scoreStr.Length];

        for (int i = 0; i < scoreStr.Length; i++)
        {
            int digit = scoreStr[i] - '0';
            GameObject digitObj = Instantiate(_scoreDigitPrefab, transform);
            digitObj.GetComponent<Image>().sprite = _scoreDigitSpriteList[digit];
            //digitObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * _digitSpacing, 0);
            _scoreDigits[i] = digitObj;
        }
    }


}
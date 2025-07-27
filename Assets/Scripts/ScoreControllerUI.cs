using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private Sprite[] _scoreDigitSpriteList; // 0-9 sprites
    [SerializeField] private GameObject _scoreDigitPrefab;    // Prefab with Image component

    private GameObject[] _scoreDigits = new GameObject[0];

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


    public void UpdateScoreDisplay(int score)
    {
        // Remove old digits
        foreach (var digitObj in _scoreDigits)
            Destroy(digitObj);
        ReplaceNewScoreDigits(score);
    }

    private void ReplaceNewScoreDigits(int score)
    {
        string scoreStr = score.ToString();
        _scoreDigits = new GameObject[scoreStr.Length];

        for (int i = 0; i < scoreStr.Length; i++)
        {
            int digit = scoreStr[i] - '0';
            GameObject digitObj = Instantiate(_scoreDigitPrefab, transform);
            digitObj.GetComponent<Image>().sprite = _scoreDigitSpriteList[digit];
            _scoreDigits[i] = digitObj;
        }
    }


}
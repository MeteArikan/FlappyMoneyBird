using System.Collections;
using UnityEngine;

public class ScoreBoardBackground : MonoBehaviour
{
    [SerializeField] private RectTransform _scoreArea;
    [SerializeField] private RectTransform _bestScoreArea;
    [SerializeField] private float _moveAmount = 30f; // Amount to move left per digit increase

    private ScoreController _bestScoreController;
    private ScoreController _scoreController;

    private int _lastDigitCount = 2;
    const float TOTAL_DURATION = 0.4f;

    private void Start()
    {
        _bestScoreController = _bestScoreArea.GetComponentInChildren<ScoreController>();
        _bestScoreController.enabled = false;

        _scoreController = _scoreArea.GetComponentInChildren<ScoreController>();
        _scoreController.enabled = false;
    }

    private void OnEnable()
    {
        GameManager.OnShowScoreBoard += GameManager_OnShowScoreBoard;
    }

    private void OnDisable()
    {
        GameManager.OnShowScoreBoard -= GameManager_OnShowScoreBoard;
    }


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


    private void GameManager_OnShowScoreBoard()
    {
        // Prepare best score display
        int bestScore = GameManager.Instance.GetHighScore();
        _bestScoreController.enabled = true;
        _bestScoreController.UpdateScoreDisplay(bestScore);
        SetBestScoreUI(bestScore);

        PrepareCurrentScoreDisplay();
    }

    private void SetBestScoreUI(int score)
    {
        int digitCount = score.ToString().Length;
        if (digitCount > 2)
        {
            // Move left by _moveAmount for each new digit
            _bestScoreArea.anchoredPosition += new Vector2(-_moveAmount * (digitCount - 1), 0);
        }
    }

    private void PrepareCurrentScoreDisplay()
    {
        int score = GameManager.Instance.GetScore;
        _scoreController.enabled = true;
        _scoreController.gameObject.SetActive(true);
        CurrentScoreUICounter(score);
    }

    private void CurrentScoreUICounter(int score)
    {
        
        // Decide your per‑step increment:
        int increment = score > 100 ? 10 : 1;
        // Recompute stepTime so the overall duration stays ~TOTAL_DURATION:
        float steps = Mathf.Ceil(score / (float)increment);
        float stepTime = TOTAL_DURATION / steps;

        if (score > 0)
            StartCoroutine(ScoreUICounterCoroutine(score, stepTime, increment));
    }

    private IEnumerator ScoreUICounterCoroutine(int score, float stepTime, int increment)
    {
        int current = 0;
        while (current < score)
        {
            yield return new WaitForSeconds(stepTime);

            // Bump by your chosen increment, but don’t overshoot final score:
            current = Mathf.Min(current + increment, score);

            _scoreController.UpdateScoreDisplay(current);
            GameManager_OnSetScoreUI(current);
        }
    }

}









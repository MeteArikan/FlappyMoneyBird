using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardBackground : MonoBehaviour
{
    public event Action OnActivateReplayButton;

    [SerializeField] private RectTransform _scoreArea;
    [SerializeField] private RectTransform _bestScoreArea;
    [SerializeField] private float _moveAmount = 30f; // Amount to move left per digit increase


    public MedalManager medalManager;
    public Image newBestScoreIcon;
    private ScoreController _bestScoreController;
    private ScoreController _scoreController;

    private int _lastDigitCount = 2;
    const float TOTAL_DURATION = 0.4f;

    private Vector2 _bestScoreStartPos; 
    private int _bestScoreLastDigitCount;



    private void Start()
    {
        // store baseline position and initial digit count
        _bestScoreStartPos = _bestScoreArea.anchoredPosition;
        _bestScoreLastDigitCount = GameManager.Instance.GetHighScore().ToString().Length;
        _bestScoreController = _bestScoreArea.GetComponentInChildren<ScoreController>();
        UpdateBestScoreDisplay(); // Initialize with current highscore
        _bestScoreController.enabled = false;

        _scoreController = _scoreArea.GetComponentInChildren<ScoreController>();
        _scoreController.UpdateScoreDisplay(0); // Initialize with 0
        _scoreController.enabled = false;


    }

    private void OnEnable()
    {
        //GameManager.OnAfterGameOver += UpdateBestScoreDisplay;  // for best score display
        DeathScreenUI.OnPrepareScoreCounter += PrepareCurrentScoreUI; // for current score display
    }

    private void OnDisable()
    {
        //GameManager.OnAfterGameOver -= UpdateBestScoreDisplay;  // for best score display
        DeathScreenUI.OnPrepareScoreCounter -= PrepareCurrentScoreUI; // for current score display

    }



    private void UpdateBestScoreDisplay()
    {
        // Prepare best score display
        int bestScore = GameManager.Instance.GetHighScore();
        _bestScoreController.enabled = true;
        _bestScoreController.UpdateScoreDisplay(bestScore);
        SetBestScoreUI(bestScore);

        //PrepareCurrentScoreDisplay();
    }

    // private void SetBestScoreUI(int score)
    // {
    //     int digitCount = score.ToString().Length;
    //     if (digitCount > 2)
    //     {
    //         // Move left by _moveAmount for each new digit
    //         _bestScoreArea.anchoredPosition += new Vector2(-_moveAmount * (digitCount - 1), 0);
    //     }
    // }

    private void SetBestScoreUI(int score)
    {
    int digitCount = score.ToString().Length;

    int offsetCount = digitCount <= 2 ? 0 : digitCount - 1;
    _bestScoreArea.anchoredPosition = _bestScoreStartPos + new Vector2(-_moveAmount * offsetCount, 0);

    // keep record if you need it elsewhere
    _bestScoreLastDigitCount = digitCount;
    }



    public void PrepareCurrentScoreUI()
    {
        int score = GameManager.Instance.GetScore;
        _scoreController.enabled = true;
        _scoreController.gameObject.SetActive(true);
        SetUpScoreCounterUI(score);
    }

    private void SetUpScoreCounterUI(int score)
    {

        // Decide your per‑step increment:
        int increment = score > 100 ? 10 : 1;
        // Recompute stepTime so the overall duration stays ~TOTAL_DURATION:
        float steps = Mathf.Ceil(score / (float)increment);
        float stepTime = TOTAL_DURATION / steps;

        if (score > 0)
            StartCoroutine(ScoreCounterUICoroutine(score, stepTime, increment));
        else
        {
            ActivateReplayButton();   // No score to count, just activate the replay button
        }
    }

    private IEnumerator ScoreCounterUICoroutine(int score, float stepTime, int increment)
    {
        int current = 0;
        while (current < score)
        {
            yield return new WaitForSeconds(stepTime);

            // Bump by your chosen increment, but don’t overshoot final score:
            current = Mathf.Min(current + increment, score);

            _scoreController.UpdateScoreDisplay(current);
            MoveScoreAreaToLeft(current);
        }

        AfterScoreCountingFinished();
    }

    private void MoveScoreAreaToLeft(int score)
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

    private void ActivateReplayButton()
    {
        OnActivateReplayButton?.Invoke();
    }

    private void AfterScoreCountingFinished()
    {
        if (GameManager.Instance.IsNewBestScore)
        {
            UpdateBestScoreDisplay();
            newBestScoreIcon.gameObject.SetActive(true);
        }
        medalManager.ShowMedal();
        Invoke(nameof(ActivateReplayButton), 0.2f);
    }


}









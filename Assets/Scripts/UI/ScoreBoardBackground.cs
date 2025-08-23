using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ScoreBoardBackground : MonoBehaviour
{
    public event Action OnActivateReplayButton;
    public static event Action OnStartMaxComboAnimation;

    [SerializeField] private RectTransform _scoreArea;
    [SerializeField] private RectTransform _bestScoreArea;
    [SerializeField] private float _moveAmount = 30f; // Amount to move left per digit increase
    [SerializeField] private RectTransform _bonusScoreArea;


    public MedalManager medalManager;
    public Image newBestScoreIcon;
    private ScoreController _bestScoreController;
    private ScoreController _scoreController;
    private TMP_Text _bonusScoreText;

    private int _lastDigitCount = 2;
    const float TOTAL_DURATION = 0.4f;

    private Vector2 _bestScoreStartPos;
    private int _bestScoreLastDigitCount;



    private void Start()
    {
        // store baseline position and initial digit count
        _bestScoreStartPos = _bestScoreArea.anchoredPosition;
        _bestScoreLastDigitCount = GameManager.Instance.GetHighScore(GameModeController.Instance.GetGameMode())
        .ToString().Length;
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
        MaxComboUI.OnComboAnimationFinished += StartAnimateBonusScore; // for max combo animation
        if (_bonusScoreArea != null)
        {
            _bonusScoreText = _bonusScoreArea.GetComponent<TMP_Text>();
            _bonusScoreText.text = "";
            _bonusScoreText.alpha = 0;
        }
    }



    private void OnDisable()
    {
        //GameManager.OnAfterGameOver -= UpdateBestScoreDisplay;  // for best score display
        DeathScreenUI.OnPrepareScoreCounter -= PrepareCurrentScoreUI; // for current score display
        MaxComboUI.OnComboAnimationFinished -= StartAnimateBonusScore; // for max combo animation

    }



    private void UpdateBestScoreDisplay()
    {
        // Prepare best score display
        int bestScore = GameManager.Instance.GetHighScore(GameModeController.Instance.GetGameMode());
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

    // private void AfterScoreCountingFinished()
    // {
    //     // if money mode, add the bonus points to the score
    //     if (GameManager.Instance.IsNewBestScore)
    //     {
    //         UpdateBestScoreDisplay();
    //         newBestScoreIcon.gameObject.SetActive(true);
    //     }
    //     medalManager.ShowMedal();
    //     Invoke(nameof(ActivateReplayButton), 0.2f);
    // }


    private void AfterScoreCountingFinished()
    {
        int bonus = GameManager.Instance.GetBonusPoints();
        if (GameModeController.Instance.GetGameMode() == GameMode.MoneyMode && bonus > 0)
        {
            //_bonusScoreText = _bonusScoreArea.GetComponent<TMP_Text>();
            _bonusScoreText.text = "+" + bonus.ToString();
            // Calculate bonus
            //int bonus = GameManager.Instance.GetMaxComboCount * 3;
            int originalScore = GameManager.Instance.GetScore;
            int newScore = originalScore + bonus;
            GameManager.Instance.CheckHighscore(newScore); // Check if new score is a high score

            // Optionally, show a bonus text here

            OnStartMaxComboAnimation?.Invoke();

            // Animate bonus after a short delay
            //StartCoroutine(AnimateBonusScore(originalScore, newScore, bonus));
        }
        else
        {
            // Normal flow
            CheckAndShowBestScore();
        }
    }

    private void StartAnimateBonusScore()
    {
        
        //.OnComplete(() =>
        //{
            //_bonusScoreArea.DOAnchorPosY(30, 0.5f).SetRelative().SetEase(Ease.OutBack);
            BonusScoreAnimationSeq();
        //});
        //StartCoroutine(AnimateBonusScore());
    }

    private void BonusScoreAnimationSeq()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_scoreArea.DOPunchScale(Vector3.one * 1, 0.4f, 1, 1));
        seq.Join(_bonusScoreArea.DOAnchorPosY(30, 0.5f).SetRelative().SetEase(Ease.OutBack));
        seq.Join(_bonusScoreText.DOFade(1, 0.25f));
        seq.AppendInterval(0.2f);
        seq.Append(_bonusScoreText.DOFade(0, 0.25f).SetEase(Ease.InQuad));
        
        seq.OnComplete(() =>
        {
            StartCoroutine(AnimateBonusScoreCounting());
        });
    }

    private IEnumerator AnimateBonusScoreCounting()
    {
        int fromScore = GameManager.Instance.GetScore;
        int toScore = fromScore + GameManager.Instance.GetBonusPoints();
        //yield return new WaitForSeconds(0.5f); // Delay before bonus counting

        // Optionally, show a "Bonus +X" UI here

        // Animate score from fromScore to toScore
        int increment = (toScore - fromScore) > 100 ? 10 : 1;
        float steps = Mathf.Ceil((toScore - fromScore) / (float)increment);
        float stepTime = TOTAL_DURATION / steps;

        int current = fromScore;
        while (current < toScore)
        {
            //yield return new WaitForSeconds(stepTime);
            current = Mathf.Min(current + increment, toScore);
            _scoreController.UpdateScoreDisplay(current);
            MoveScoreAreaToLeft(current);
            yield return new WaitForSeconds(stepTime);
        }

        // Now check for new best score and show medals etc.
        CheckAndShowBestScore();
    }

private void CheckAndShowBestScore()
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









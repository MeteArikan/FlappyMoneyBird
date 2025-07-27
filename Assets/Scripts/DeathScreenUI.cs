using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class DeathScreenUI : MonoBehaviour
{
    public static Action OnPrepareScoreCounter;
    [Header("References")]
    [SerializeField] private Button _replayButton;
    [SerializeField] private Image _gameTitle;
    [SerializeField] private Image _overTitle;
    [SerializeField] private RectTransform _gameOverTitleTransform;
    [SerializeField] private ScoreBoardBackground _scoreBoard;


    [Header("Settings")]
    [SerializeField] private float _animationDuration = 0.3f;


    private RectTransform _scoreBoardTransform;
    //private ScoreBoardBackground _scoreBoardScript;


    private void Start()
    {
        _replayButton.onClick.AddListener(RestartGame);
        _scoreBoardTransform = _scoreBoard.GetComponent<RectTransform>();
        //_scoreBoardScript = _scoreBoardObject.GetComponent<ScoreBoardBackground>();

    }
    private void OnEnable()
    {

        _replayButton.gameObject.SetActive(false);
        //_gameOverTitleParent.gameObject.SetActive(false);
        //_replayButton.image.DOFade(0f, 0).SetEase(Ease.Linear);
        _gameTitle.DOFade(0f, 0).SetEase(Ease.Linear);
        _overTitle.DOFade(0f, 0).SetEase(Ease.Linear);

        GameManager.OnAfterGameOver += ShowGameOverTitle;
        GameManager.OnShowScoreBoard += ShowScoreBoard;
        _scoreBoard.OnActivateReplayButton += ActivateReplayButton;
    }

    private void OnDisable()
    {
        GameManager.OnAfterGameOver -= ShowGameOverTitle;
        GameManager.OnShowScoreBoard -= ShowScoreBoard;
        _scoreBoard.OnActivateReplayButton -= ActivateReplayButton;
    }



    private void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }

    private void ShowGameOverTitle()
    {

        _gameTitle.DOFade(1f, _animationDuration).SetEase(Ease.OutBack);
        _overTitle.DOFade(1f, _animationDuration).SetEase(Ease.OutBack);

        _gameOverTitleTransform
            .DOAnchorPosY(60, _animationDuration)  // target is “+60” not an absolute value yet
            .SetRelative()                // makes it add to the current value
            .SetEase(Ease.OutElastic)  // (optional) smoothing
            // .OnComplete(() =>
            // {
            //     _replayButton.gameObject.SetActive(true);
            //     //_replayButton.image.DOFade(1f, _animationDuration * 2).SetEase(Ease.OutBack);
            // })
            ;
    }

    private void ShowScoreBoard()
    {
        _scoreBoardTransform.DOAnchorPosY(3000, _animationDuration)
            .SetRelative()
            .SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                OnPrepareScoreCounter?.Invoke();
            });
    }
    

    private void ActivateReplayButton()
    {
        _replayButton.gameObject.SetActive(true);
    }
}

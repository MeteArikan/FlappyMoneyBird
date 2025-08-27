using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MaxComboUI : MonoBehaviour
{
    public static event Action OnComboAnimationFinished;
    [SerializeField] private TMP_Text _maxComboNumber;
    private TMP_Text _maxComboText;
    private RectTransform _maxComboNumberTransform;

    private void OnEnable()
    {
        _maxComboText = GetComponent<TMP_Text>();
        _maxComboNumberTransform = _maxComboNumber.GetComponent<RectTransform>();
        ShowMaxCombo();
        ScoreBoardBackground.OnStartMaxComboAnimation += MaxComboAnimation;
    }



    private void ShowMaxCombo()
    {
        int maxCombo = GameManager.Instance.GetMaxComboCount;
        _maxComboText.text = "Max Combo: " + maxCombo.ToString();
        _maxComboNumber.text = maxCombo.ToString();
    }


    private void MaxComboAnimation()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_maxComboNumberTransform.DOAnchorPos(new Vector2(30, 30), 0.35f)
            .SetRelative()
            .SetEase(Ease.OutExpo));
        seq.Append(_maxComboNumberTransform.DOAnchorPos(new Vector2(-50, -270), 0.5f)
            .SetRelative()
            );
        seq.Join(_maxComboNumber.DOFade(0f, 0.5f)) 
        ;
        seq.OnComplete(() =>
        {
            OnComboAnimationFinished?.Invoke();
        });

    }


    private void OnDisable() {
        ScoreBoardBackground.OnStartMaxComboAnimation -= MaxComboAnimation;
    }
}
    
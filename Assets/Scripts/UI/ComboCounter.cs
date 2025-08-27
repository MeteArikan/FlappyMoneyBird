using DG.Tweening;
using TMPro;
using UnityEngine;

public class ComboCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _comboCounterText;
    [SerializeField] private TMP_Text _comboText;
    [SerializeField] private Ease scaleEase = Ease.OutElastic;
    [SerializeField] private float punchScale = 1.5f;
    [SerializeField] private float animationDuration = 0.5f;
    private void OnEnable()
    {
        GameManager.OnComboCountUpdated += UpdateComboText;
    }

    private void UpdateComboText()
    {
        int comboCount = GameManager.Instance.GetComboCount;
        if (comboCount < 1)
        {
            _comboCounterText.alpha = 0f;
            _comboText.alpha = 0f;
            return;
        }
        _comboCounterText.alpha = 1f;
        _comboText.alpha = 1f;
        _comboCounterText.text = "x" + comboCount.ToString();
        ChangeColor(comboCount);
        if (comboCount > 1)
        {
            ComboAnimation();
        }
        
    }

    private void ChangeColor(int comboCount)
    {
        switch (comboCount)
        {
            case 0:
                _comboCounterText.color = Color.white;
                _comboText.color = Color.white;
                break;
            case 1:
                _comboCounterText.color = Color.white;
                _comboText.color = Color.white;
                break; 
            case 2:
                _comboCounterText.color = Color.yellow;
                _comboText.color = Color.yellow;
                break;
            case 3:
                _comboCounterText.color = Color.red;
                _comboText.color = Color.red;
                break;
            case 4:
                _comboCounterText.color = Color.green;
                _comboText.color = Color.green;
                break;
            case 5:
                _comboCounterText.color = Color.blue;
                _comboText.color = Color.blue;
                break;
            default:
                _comboCounterText.color = Color.magenta;
                _comboText.color = Color.magenta;
                break;
        }
    }
    private void ComboAnimation()
    {
        _comboCounterText.transform.DOPunchScale(Vector3.one * punchScale, animationDuration, 1, 1)
            .SetEase(scaleEase)
            ;
    }

    private void OnDisable()
    {
        GameManager.OnComboCountUpdated -= UpdateComboText;
    }
    

}

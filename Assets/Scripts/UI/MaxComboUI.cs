using TMPro;
using UnityEngine;

public class MaxComboUI : MonoBehaviour
{    
    [SerializeField] private TMP_Text _maxComboNumber;
    private TMP_Text _maxComboText;

    private void OnEnable()
    {
        _maxComboText = GetComponent<TMP_Text>();
        ShowMaxCombo();
    }

    private void ShowMaxCombo()
    {
        int maxCombo = GameManager.Instance.GetMaxComboCount;
        _maxComboText.text = "Max Combo: " + maxCombo.ToString();
        _maxComboNumber.text = maxCombo.ToString();
    }
}

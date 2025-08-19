using System;
using TMPro;
using UnityEngine;

public class ComboCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _comboText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        GameManager.OnComboCountUpdated += UpdateComboText;
    }

    private void UpdateComboText()
    {
        _comboText.text = "x"  + GameManager.Instance.GetComboCount.ToString();
    }

    private void OnDisable() {
        GameManager.OnComboCountUpdated -= UpdateComboText;
    }



}

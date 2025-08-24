using System;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class MedalManager : MonoBehaviour
{
    private Image _medalImage;


    [SerializeField] private MedalSO[] _medalSortedList;    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Awake()
    {
        _medalImage = GetComponent<Image>();
    }


    private void OnEnable()
    {
        _medalImage.enabled = false;
    }
    
    private void OnDisable() {
        _medalImage.enabled = false;
    }

    public void ShowMedal()
    {
        int score = GameModeController.Instance.GetGameMode() == GameMode.MoneyMode ? 
            GameManager.Instance.GetScore + GameManager.Instance.GetBonusPoints() : GameManager.Instance.GetScore;
        MedalSO awarded = _medalSortedList
        .Where(medal => score >= medal.MedalMinScore)
        .LastOrDefault();

        if (awarded != null)
        {
            // set the sprite and name on your UI
            _medalImage.enabled = true;
            _medalImage.sprite = awarded.MedalSprite;
        }
    }
}

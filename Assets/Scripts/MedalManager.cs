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
        MedalSO awarded = _medalSortedList
        .Where(medal => GameManager.Instance.GetScore >= medal.MedalMinScore)
        .LastOrDefault();

        if (awarded != null)
        {
            // set the sprite and name on your UI
            _medalImage.enabled = true;
            _medalImage.sprite = awarded.MedalSprite;
        }
    }
}

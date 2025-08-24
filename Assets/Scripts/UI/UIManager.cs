using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ScoreController _scoreController;
    //[SerializeField] private DeathScreenUI _deathScreenUI;
    //[SerializeField] private GameObject _getReadyImage;
    //private RectTransform _deathScreenRect;
    [SerializeField] private GameObject _menuScreenUI;
    [SerializeField] private GameObject _comboTextArea;
    [SerializeField] private GameObject _maxComboTextArea;
    [SerializeField] private GameObject moneyModePanel;
    [SerializeField] private GameObject hardModePanel;

    private void OnEnable()
    {
        //_getReadyImage.SetActive(true);
        GameManager.OnAfterGameOver += ShowDeathScreen;
        GameManager.OnGameMenuOpened += ShowMenuScreen;
        GameManager.OnGameStarted += HideMenuScreen;
        //GameManager.OnUnlocksUpdated += RefreshUnlockPanels;
        RefreshUnlockPanels();
        //_deathScreenRect = _deathScreenUI.gameObject.GetComponent<RectTransform>();
    }



    private void ShowMenuScreen()
    {
        _menuScreenUI.SetActive(true);
        ShowComboText(false); // Hide combo UI in money mode
    }

    private void HideMenuScreen()
    {
        _menuScreenUI.SetActive(false);
        ShowComboText(true); // Show combo UI in money mode
    }

    private void ShowDeathScreen()
    {
        //_deathScreenRect.anchoredPosition += new Vector2(0, 3000);
        _scoreController.gameObject.SetActive(false);
        if (GameModeController.Instance.GetGameMode() == GameMode.MoneyMode)
        {
            _maxComboTextArea.SetActive(true);
        } 
        ShowComboText(false); // Hide combo UI in money mode

    }

    private void ShowComboText(bool showText)
    {
        if (GameModeController.Instance.GetGameMode() != GameMode.MoneyMode) return;
        //FindAnyObjectByType<ComboCounter>().gameObject.SetActive(showText);
        if (_comboTextArea != null)
        {
            _comboTextArea.SetActive(showText);
        }
    }

    private void RefreshUnlockPanels()
    {
        if (moneyModePanel != null)
            moneyModePanel.SetActive(!GameManager.Instance.IsMoneyModeUnlocked);

        if (hardModePanel != null)
            hardModePanel.SetActive(!GameManager.Instance.IsHardModeUnlocked);
    }

    private void OnDisable()
    {
        GameManager.OnAfterGameOver -= ShowDeathScreen;
        GameManager.OnGameMenuOpened -= ShowMenuScreen;
        GameManager.OnGameStarted -= HideMenuScreen;
        //GameManager.OnUnlocksUpdated -= RefreshUnlockPanels;
    }
}

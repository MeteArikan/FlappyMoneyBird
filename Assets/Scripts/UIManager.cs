using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ScoreController _scoreController;
    //[SerializeField] private DeathScreenUI _deathScreenUI;
    //[SerializeField] private GameObject _getReadyImage;
    //private RectTransform _deathScreenRect;
    [SerializeField] private GameObject _menuScreenUI;

    private void OnEnable()
    {
        //_getReadyImage.SetActive(true);
        GameManager.OnAfterGameOver += ShowDeathScreen;
        GameManager.OnGameMenuOpened += ShowMenuScreen;
        GameManager.OnGameStarted += HideMenuScreen;
        //_deathScreenRect = _deathScreenUI.gameObject.GetComponent<RectTransform>();
    }



    private void ShowMenuScreen()
    {
        _menuScreenUI.SetActive(true);
    }

    private void HideMenuScreen()
    {
        _menuScreenUI.SetActive(false);
    }

    private void ShowDeathScreen()
    {
        //_deathScreenRect.anchoredPosition += new Vector2(0, 3000);
        _scoreController.gameObject.SetActive(false);

    }

    private void OnDisable()
    {
        GameManager.OnAfterGameOver -= ShowDeathScreen;
        GameManager.OnGameMenuOpened -= ShowMenuScreen;
        GameManager.OnGameStarted -= HideMenuScreen;
    }
}

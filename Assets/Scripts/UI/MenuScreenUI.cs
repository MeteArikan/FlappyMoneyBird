using System;
using UnityEngine;

public class MenuScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject _navBar;
    private void OnEnable()
    {
        GameManager.OnGameStarted += HideNavBar;
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= HideNavBar;
    }

    private void HideNavBar()
    {
        _navBar.SetActive(false);
    }
}

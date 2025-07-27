using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ScoreController _scoreController;
    [SerializeField] private DeathScreenUI _deathScreenUI;
    private RectTransform _deathScreenRect;

    private void Start()
    {
        GameManager.OnAfterGameOver += ShowDeathScreen;
        _deathScreenRect = _deathScreenUI.gameObject.GetComponent<RectTransform>();
    }

    private void ShowDeathScreen()
    {
        //_deathScreenRect.anchoredPosition += new Vector2(0, 3000);
        _scoreController.gameObject.SetActive(false);
        
    }
    
    private void OnDestroy() {
        GameManager.OnAfterGameOver -= ShowDeathScreen;
    }
}

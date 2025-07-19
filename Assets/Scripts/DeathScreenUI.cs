using System;
using UnityEngine;
using UnityEngine.UI;
public class DeathScreenUI : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    private void OnEnable()
    {
        _restartButton.onClick.AddListener(RestartGame);
    }

    private void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }
}

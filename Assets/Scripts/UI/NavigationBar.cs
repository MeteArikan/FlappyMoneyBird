using UnityEngine;
using UnityEngine.UI;

public class NavigationBar : MonoBehaviour
{
    [SerializeField] private Button _moneyModeButton;
    [SerializeField] private Button _hardModeButton;
    [SerializeField] private Button _defaultModeButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moneyModeButton.onClick.AddListener(() => GameManager.Instance.NavigateToGameMode(GameMode.MoneyMode));
        _hardModeButton.onClick.AddListener(() => GameManager.Instance.NavigateToGameMode(GameMode.HardMode));
        _defaultModeButton.onClick.AddListener(() => GameManager.Instance.NavigateToGameMode(GameMode.DefaultMode));
    }

}

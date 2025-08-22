using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NavigationBar : MonoBehaviour
{
    [SerializeField] private Transform[] birdIcons; // Assign in Inspector: 0=Default, 1=Money, 2=Hard
    [SerializeField] private Button _moneyModeButton;
    [SerializeField] private Button _hardModeButton;
    [SerializeField] private Button _defaultModeButton;

    private void Start()
    {
        _moneyModeButton.onClick.AddListener(() => GameManager.Instance.NavigateToGameMode(GameMode.MoneyMode));
        _hardModeButton.onClick.AddListener(() => GameManager.Instance.NavigateToGameMode(GameMode.HardMode));
        _defaultModeButton.onClick.AddListener(() => GameManager.Instance.NavigateToGameMode(GameMode.DefaultMode));
    }

    private void OnEnable()
    {
        AnimateForCurrentMode();
    }

    private void OnDisable()
    {
        // Kill all tweens safely when disabled
        foreach (var bird in birdIcons)
        {
            bird.DOKill();
        }
    }

    private void OnDestroy()
    {
        // Double safety: kill tweens if object is destroyed
        foreach (var bird in birdIcons)
        {
            bird.DOKill();
        }
    }

    private void AnimateForCurrentMode()
    {
        int modeIndex = (int)GameModeController.Instance.GetGameMode();
        AnimateSelectedBird(modeIndex);
    }

    public void AnimateSelectedBird(int modeIndex)
    {
        // Reset all birds to default position/rotation
        foreach (var bird in birdIcons)
        {
            bird.DOKill(); // Kill old tweens before making new ones
            bird.DOLocalMoveY(0, 0.2f);
            bird.DOLocalRotate(Vector3.zero, 0.2f);
        }

        // Animate the selected bird
        birdIcons[modeIndex]
            .DOLocalMoveY(30f, 0.3f)
            .SetEase(Ease.OutBack);
        birdIcons[modeIndex]
            .DOLocalRotate(new Vector3(0, 0, 30f), 0.3f)
            .SetEase(Ease.OutBack);

        Debug.Log("Animating bird: " + modeIndex);

        // Update button states
        _defaultModeButton.interactable = modeIndex != 0;
        _moneyModeButton.interactable = modeIndex != 1;
        _hardModeButton.interactable   = modeIndex != 2;
    }
}

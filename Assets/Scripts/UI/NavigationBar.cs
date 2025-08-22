using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class NavigationBar : MonoBehaviour
{
    [SerializeField] private Transform[] birdIcons; // Assign in Inspector: 0=Default, 1=Money, 2=Hard
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

    private void OnEnable()
    {
        AnimateForCurrentMode();
    }

    // private void OnDisable()
    // {
    //     AnimateForCurrentMode();
    // }

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
            bird.DOLocalMoveY(0, 0.2f);
            bird.DOLocalRotate(Vector3.zero, 0.2f);
        }

        // Animate the selected bird
        birdIcons[modeIndex]
            .DOLocalMoveY(30f, 0.3f) // Move up by 30 units
            .SetEase(Ease.OutBack);
        birdIcons[modeIndex]
            .DOLocalRotate(new Vector3(0, 0, 30f), 0.3f)
            .SetEase(Ease.OutBack);

        Debug.Log("Animating bird: " + modeIndex);
        switch (modeIndex)
        {
            case 0:
                _defaultModeButton.interactable = false;
                _moneyModeButton.interactable = true;
                _hardModeButton.interactable = true;
                break;
            case 1:
                _defaultModeButton.interactable = true;
                _moneyModeButton.interactable = false;
                _hardModeButton.interactable = true;
                break;
            case 2:
                _defaultModeButton.interactable = true;
                _moneyModeButton.interactable = true;
                _hardModeButton.interactable = false;
                break;

        }
    }
    

}

using UnityEngine;

public class StateController : MonoBehaviour
{
    private PlayerState _currentPlayerState = PlayerState.Start;

    private void Start()
    {
        ChangePlayerState(PlayerState.Start);
    }

    // private void OnEnable() {
    //     ChangePlayerState(PlayerState.Start);
    // }

    // private void OnDisable() {
    //     ChangePlayerState(PlayerState.Start);
    // }


    public void ChangePlayerState(PlayerState newPlayerState)
    {
        if (_currentPlayerState == newPlayerState)
        {
            return;
        }
        _currentPlayerState = newPlayerState;
        Debug.Log($"Player state changed to: {_currentPlayerState}");

    }

    public PlayerState GetPlayerState()
    {
        return _currentPlayerState;
    }
    
}

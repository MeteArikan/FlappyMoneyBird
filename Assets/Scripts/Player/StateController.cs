using UnityEngine;

public class StateController : MonoBehaviour
{
    private PlayerState _currentPlayerState = PlayerState.Fly;

    private void Start()
    {
        ChangePlayerState(PlayerState.Fly);
    }
    public void ChangePlayerState(PlayerState newPlayerState)
    {
        if (_currentPlayerState == newPlayerState)
        {
            return;
        }
        _currentPlayerState = newPlayerState;

    }

    public PlayerState GetPlayerState()
    {
        return _currentPlayerState;
    }
    
}

using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;
    private StateController _stateController;

    private void Awake()
    {
        _stateController = GetComponent<StateController>();
    }

    private void Update()
    {
        // This is the main animation loop. It runs every frame.
        SetPlayerAnimations();
    }

    private void SetPlayerAnimations()
    {
        var currentState = _stateController.GetPlayerState();

        switch (currentState)
        {
            // case PlayerState.Start:
            //     // Animation for the waiting state
            //     _playerAnimator.SetBool("IsFly", false);
            //     _playerAnimator.SetBool("IsDead", false);
            //     break;
            
            case PlayerState.Fly:
                // Bird just jumped
                _playerAnimator.SetBool("IsJump", true);
                _playerAnimator.SetBool("IsDead", false);
                break;

            case PlayerState.Fall:
                // Bird is falling down
                _playerAnimator.SetBool("IsJump", false);
                _playerAnimator.SetBool("IsDead", false);
                break;

            case PlayerState.Death:
                // Bird has died
                _playerAnimator.SetBool("IsDead", true);
                _playerAnimator.SetBool("IsJump", false);
                break;
        }
    }
}
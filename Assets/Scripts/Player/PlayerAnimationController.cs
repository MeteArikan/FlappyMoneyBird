using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;
    //[SerializeField] private Animator _platformAnimator;

    private PlayerMovement _playerController;
    //private StateController _stateController;


    private void Awake()
    {
        _playerController = GetComponent<PlayerMovement>();
        //_stateController = GetComponent<StateController>();
    }

    private void Start()
    {
        _playerController.OnPlayerDead += PlayerController_OnPlayerDead;
    }


    // private void Update()
    // {
    //     if (GameManager.Instance.GetCurrentGameState() != GameState.Play
    //     && GameManager.Instance.GetCurrentGameState() != GameState.Resume)
    //     {
    //         return;
    //     }
    //     SetPlayerAnimations();
    // }


    private void PlayerController_OnPlayerDead()
    {
        _playerAnimator.SetBool("IsDead", true);
        
        //Invoke(nameof(ResetJumpingAnimation), 0.5f);
    }

    // private void ResetJumpingAnimation()
    // {
    //     _playerAnimator.SetBool(Consts.PlayerAnimations.IS_JUMPING, false);
    // }
    





    // private void SetPlayerAnimations()
    // {
    //     var currentState = _stateController.GetPlayerState();

    //     switch (currentState)
    //     {
    //         case PlayerState.Idle:
    //             _playerAnimator.SetBool(Consts.PlayerAnimations.IS_MOVING, false);
    //             _playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING, false);
    //             break;

    //         case PlayerState.Move:
    //             _playerAnimator.SetBool(Consts.PlayerAnimations.IS_MOVING, true);
    //             _playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING, false);
    //             break;

    //         case PlayerState.SlideIdle:
    //             _playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING_ACTIVE, false);
    //             _playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING, true);
    //             break;

    //         case PlayerState.Slide:
    //             _playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING_ACTIVE, true);
    //             _playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING, true);
    //             break;

    //     }
    // }
}
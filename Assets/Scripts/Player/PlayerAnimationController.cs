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
    }

    private void Start()
    {
        _playerController.OnPlayerDead += PlayerController_OnPlayerDead;
    }

    private void PlayerController_OnPlayerDead()
    {
        _playerAnimator.SetBool("IsDead", true);
    }
}
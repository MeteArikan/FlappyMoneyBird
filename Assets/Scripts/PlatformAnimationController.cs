using System;
using UnityEngine;

public class PlatformAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerController;
    private Animator _platformAnimator;

    private void Awake()
    {
        _platformAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        // _playerController.OnPlayerDead += PlayerController_OnPlayerDead;
        GameManager.OnAfterGameOver += GameManager_OnGameOverEvent;
    }

    private void GameManager_OnGameOverEvent()
    {
        _platformAnimator.enabled = false;
    }

    private void PlayerController_OnPlayerDead()
    {
        _platformAnimator.enabled = false;
    }
    
    private void OnDestroy() {
        GameManager.OnAfterGameOver -= GameManager_OnGameOverEvent;
    }
}

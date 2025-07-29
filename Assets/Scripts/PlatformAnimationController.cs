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
    private void OnEnable()
    {
        _platformAnimator.enabled = false;
        GameManager.OnAfterGameOver += GameManager_OnGameOverEvent;
        GameManager.OnGameStarted += GameManager_OnGameStarted;
    }
    private void OnDisable()
    {
        GameManager.OnAfterGameOver -= GameManager_OnGameOverEvent;
        GameManager.OnGameStarted -= GameManager_OnGameStarted;
    }


    private void GameManager_OnGameStarted()
    {
        _platformAnimator.enabled = true;
    }

    private void GameManager_OnGameOverEvent()
    {
        _platformAnimator.enabled = false;
    }


}

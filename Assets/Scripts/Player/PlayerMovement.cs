using System;
using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public event Action OnPlayerDead;
    private StateController _stateController;
    [SerializeField] private float _jumpSpeed = 3f;
    [SerializeField] private float initialGravityScale = 1f; // Store original gravity

    private Rigidbody2D _playerRigidbody;
    private Animator _playerAnimator;
    private bool _isDead;

    public bool IsDead => _isDead;

    private bool _isModeUnlocked = false;

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _stateController = GetComponent<StateController>();
        _playerAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        initialGravityScale = _playerRigidbody.gravityScale;
        _playerRigidbody.bodyType = RigidbodyType2D.Kinematic;
        _playerRigidbody.linearVelocity = Vector2.zero; 
        _playerRigidbody.gravityScale = 0; // prevent any movement
        _isModeUnlocked = GameManager.Instance.IsCurrentModeUnlocked();
    }

    void OnEnable()
    {
        GameManager.OnGameStarted += GameManager_OnGameStarted;
    }

    void OnDisable()
    {
        GameManager.OnGameStarted -= GameManager_OnGameStarted;
    }

    void Update()
    {

        if (InputHelper.IsTapOrClick() && !_isDead)
        {
            if (_isModeUnlocked)  BirdFly();
        }
        if (_playerRigidbody.linearVelocityY <= 0)
        {
            _playerAnimator.SetBool("IsJumping", false);
        }
    }




    private void BirdFly()
    {

        _playerRigidbody.linearVelocity = Vector2.up * _jumpSpeed;
        _playerRigidbody.linearDamping = 0.5f;
        AudioManager.Instance.Play(SoundType.FlySound);
        _playerAnimator.SetBool("IsJumping", true);
        
    }

    private void GameManager_OnGameStarted()
    {
        _playerRigidbody.bodyType = RigidbodyType2D.Dynamic; // Allow physics to affect the bird
        _playerRigidbody.gravityScale = initialGravityScale; // Restore gravity

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Scoring"))
        {
            GameManager.Instance.IncreaseScore();
            AudioManager.Instance.Play(SoundType.PointSound);

        }
        if (other.gameObject.CompareTag("Money") && !_isDead)
        {
            GameManager.Instance.IncreaseComboCount();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("HitObjects") && !_isDead)
        {
            AudioManager.Instance.Play(SoundType.HitSound);
            _isDead = true;
            BirdDeath();

        }
        if (other.gameObject.CompareTag("Platform") && !_isDead)
        {
            AudioManager.Instance.Play(SoundType.HitSound);
            _isDead = true;
            BirdDeath();

        }
        if (other.gameObject.CompareTag("Platform") && _isDead)
        {
            _playerRigidbody.simulated = false;

        }
        
    }

    private void BirdDeath()
    {

        //_playerRigidbody.simulated = false;
        //_playerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        //_playerRigidbody.bodyType = RigidbodyType2D.Kinematic;
        _playerRigidbody.linearVelocity = Vector2.zero;
        //transform.DOMoveY(-6f, 1f); // Stop physics interactions
        _stateController.ChangePlayerState(PlayerState.Death);
        OnPlayerDead?.Invoke();
        AudioManager.Instance.Play(SoundType.DieSound);
        GameManager.Instance.OnGameOver();
    }

}

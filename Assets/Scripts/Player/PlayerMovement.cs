using System;
using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public event Action OnPlayerDead;
    private StateController _stateController;
    [SerializeField] private float _jumpSpeed = 3f;
    [SerializeField] private float initialGravityScale = 1f; // Store original gravity

    //private bool canFly = false; // Control if the bird can receive input



    private Rigidbody2D _playerRigidbody;
    private Animator _playerAnimator;
    private bool _isJumpTriggered = false;
    private bool _isDead;
    //private bool _queuedFly = false;

    public bool IsDead => _isDead;

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
        _playerRigidbody.linearVelocity = Vector2.zero; // Ensure no lingering velocity
        _playerRigidbody.gravityScale = 0; // Explicitly set gravity to 0 to prevent any slight movement
    }

    void OnEnable()
    {
        // Subscribe to the OnGameStart event
        GameManager.OnGameStarted += GameManager_OnGameStarted;
    }

    void OnDisable()
    {
        // Unsubscribe from the OnGameStart event when this object is disabled or destroyed
        // This is crucial to prevent memory leaks and null reference errors
        GameManager.OnGameStarted -= GameManager_OnGameStarted;
    }


    // void Update()
    // {

    //     if (!_isDead && Input.GetKeyDown(KeyCode.Space))
    //     {
    //         BirdFly();
    //         // _playerRigidbody.linearVelocity = Vector2.up * _jumpSpeed;
    //         // AudioManager.Instance.Play(SoundType.FlySound);
    //     }
    // }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isDead)
        {
            //_queuedFly = true;
            BirdFly();
        }
        if (_playerRigidbody.linearVelocityY <= 0)
        {
            _playerAnimator.SetBool("IsJumping", false);
        }
    }



    // void FixedUpdate()
    // {
    //     if (_queuedFly && !_isDead)
    //     {
    //         BirdFly();
    //         _queuedFly = false;
    //     }
    //     else
    //     {
    //     _queuedFly = false;
    //     }   
    // }



    private void BirdFly()
    {

        //Invoke(nameof(DisableJumpTrigger), 0.32f); // Disable the jump trigger after a short delay

        _playerRigidbody.linearVelocity = Vector2.up * _jumpSpeed;
        _playerRigidbody.linearDamping = 0.5f;
        AudioManager.Instance.Play(SoundType.FlySound);
        _playerAnimator.SetBool("IsJumping", true);
        
    }

    private void DisableJumpTrigger()
    {
        _isJumpTriggered = false;
  
    }

    private void GameManager_OnGameStarted()
    {
        _playerRigidbody.bodyType = RigidbodyType2D.Dynamic; // Allow physics to affect the bird
        //canFly = true; // Allow player input
        _playerRigidbody.gravityScale = initialGravityScale; // Restore gravity

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Scoring"))
        {
            GameManager.Instance.IncreaseScore();
            AudioManager.Instance.Play(SoundType.PointSound);

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
        

        // if (other.gameObject.CompareTag("Platform") && !_isDead)
        // {
        //     AudioManager.Instance.Play(SoundType.HitSound);
        //     BirdFall();

        // }
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

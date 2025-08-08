using System;
using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public event Action OnPlayerDead;
    private StateController _stateController;
    [SerializeField] private float _jumpSpeed = 3f;
    private float initialGravityScale;

    private Rigidbody2D _playerRigidbody;
    private bool _queuedFly = false;
    private bool _isGameStarted = false;
    private bool _isDead;
    private bool _isFalling;
    public bool IsDead => _isDead;

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _stateController = GetComponent<StateController>();
    }

    private void Start()
    {
        initialGravityScale = _playerRigidbody.gravityScale;
        _playerRigidbody.bodyType = RigidbodyType2D.Kinematic;
        _playerRigidbody.linearVelocity = Vector2.zero;
        _playerRigidbody.gravityScale = 0;
        _stateController.ChangePlayerState(PlayerState.Start); // Ensure Start state is set
    }

    void OnEnable()
    {
        _isGameStarted = false;
        GameManager.OnGameStarted += GameManager_OnGameStarted;
    }

    void OnDisable()
    {
        GameManager.OnGameStarted -= GameManager_OnGameStarted;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _queuedFly = true;
        }
    }

    void FixedUpdate()
    {
        if (_queuedFly && !_isDead)
        {
            _isFalling = false;
            BirdFly();
            _queuedFly = false;
        }
        else
        {
            _queuedFly = false;
            // Only change to Fall state if the game has started
            if (_isFalling && !_queuedFly )
            {
                 _stateController.ChangePlayerState(PlayerState.Fall);
            }
        }
    }

    private void BirdFly()
    {
        _playerRigidbody.linearVelocity = Vector2.up * _jumpSpeed;
        _playerRigidbody.linearDamping = 0.5f;
        AudioManager.Instance.Play(SoundType.FlySound);

        // This is the key change: explicitly set the state
        _stateController.ChangePlayerState(PlayerState.Fly);

        Invoke(nameof(StartFalling), 0.25f);
    }

    private void StartFalling()
    {
        _isFalling = true;
    }

    private void GameManager_OnGameStarted()
    {
        _playerRigidbody.bodyType = RigidbodyType2D.Dynamic;
        _playerRigidbody.gravityScale = initialGravityScale;
        _isGameStarted = true;
        
        // Start with a flap, so we are in the Fly state immediately
        _queuedFly = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Scoring"))
        {
            GameManager.Instance.IncreaseScore();
            AudioManager.Instance.Play(SoundType.PointSound);
        }
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if ((other.gameObject.CompareTag("HitObjects") || other.gameObject.CompareTag("Platform")) && !_isDead)
    //     {
    //         AudioManager.Instance.Play(SoundType.HitSound);
    //         BirdDeath();
    //     }




    // }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("HitObjects") && !_isDead)
        {
        AudioManager.Instance.Play(SoundType.HitSound);
        //op_isDead = true;
        BirdDeath();

        }
        if (other.gameObject.CompareTag("Platform") && !_isDead)
        {
        AudioManager.Instance.Play(SoundType.HitSound);
        //_isDead = true;
        BirdDeath();

        }
        if (other.gameObject.CompareTag("Platform") && _isDead)
        {
        _playerRigidbody.simulated = false;

        }

        // if (other.gameObject.CompareTag("Platform") && !_isDead)
        // {
        // AudioManager.Instance.Play(SoundType.HitSound);
        // BirdFall();

        // }
    }

    private void BirdDeath()
    {
        _isDead = true;
        _playerRigidbody.linearVelocity = Vector2.zero;
        _stateController.ChangePlayerState(PlayerState.Death);
        OnPlayerDead?.Invoke();
        AudioManager.Instance.Play(SoundType.DieSound);
        GameManager.Instance.OnGameOver();
    }
}
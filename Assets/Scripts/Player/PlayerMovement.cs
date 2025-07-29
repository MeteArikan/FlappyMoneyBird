using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public event Action OnPlayerDead;
    private StateController _stateController;
    [SerializeField] private float _jumpSpeed = 3f;
    [SerializeField] private float initialGravityScale = 1f; // Store original gravity

    //private bool canFly = false; // Control if the bird can receive input



    private Rigidbody2D _playerRigidbody;
    private bool _isDead;

    public bool IsDead => _isDead;

    private void Awake() {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _stateController = GetComponent<StateController>();
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


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && !_isDead)
        {
            BirdFly();
            // _playerRigidbody.linearVelocity = Vector2.up * _jumpSpeed;
            // AudioManager.Instance.Play(SoundType.FlySound);
        }
    }

    private void BirdFly()
    {
        _playerRigidbody.linearVelocity = Vector2.up * _jumpSpeed;
        AudioManager.Instance.Play(SoundType.FlySound);
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
            BirdFall();

        }
    }

    private void BirdFall()
    {
        _isDead = true;
        //canFly = false; // Allow player input
        _stateController.ChangePlayerState(PlayerState.Death);
        OnPlayerDead?.Invoke();
        AudioManager.Instance.Play(SoundType.DieSound);
        GameManager.Instance.OnGameOver();
    }

}

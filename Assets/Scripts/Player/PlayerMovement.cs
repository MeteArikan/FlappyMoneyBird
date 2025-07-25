using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public event Action OnPlayerDead;
    private StateController _stateController;
    [SerializeField] private float _jumpSpeed = 3f;


    private Rigidbody2D _playerRigidbody;
    private bool _isDead;

    public bool IsDead => _isDead;

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _stateController = GetComponent<StateController>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && !_isDead)
        {
            _playerRigidbody.linearVelocity = Vector2.up * _jumpSpeed;
            AudioManager.Instance.Play(SoundType.FlySound);
        }
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
        _stateController.ChangePlayerState(PlayerState.Death);
        OnPlayerDead?.Invoke();
        AudioManager.Instance.Play(SoundType.DieSound);
        GameManager.Instance.OnGameOver();
    }

}

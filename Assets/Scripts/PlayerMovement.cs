using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public event Action OnPlayerDead;

    //[SerializeField] private GameObject _deathScreen;
    private Animator _playerAnimator;
    private StateController _stateController;
    [SerializeField] private float _jumpSpeed = 3f;


    private Rigidbody2D _playerRigidbody;
    private bool _isDead;

    public bool IsDead => _isDead;

    private void Start()
    {
        //_deathScreen.SetActive(false);
        //Time.timeScale = 1f;
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
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
        if (other.gameObject.CompareTag("Pipes"))
        {
            AudioManager.Instance.Play(SoundType.HitSound);
            //AudioManager.Instance.Play(SoundType.DieSound);



            BirdFall();

        }
    }

    private void BirdFall()
    {
        _isDead = true;
        _stateController.ChangePlayerState(PlayerState.Death);
        //_playerAnimator.SetBool("IsDead", true);
        OnPlayerDead?.Invoke();
        AudioManager.Instance.Play(SoundType.DieSound);
        GameManager.Instance.OnGameOver();
    }
    

    // private void PlayDieSound()
    // {
    //     AudioManager.Instance.Play(SoundType.DieSound);
    // }

}

using System;
using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    [SerializeField] private float _pipeSpeed = 3f;
    [SerializeField] private float _destroyOffset = 10f; // Distance behind player to destroy

    private Transform _playerTransform;
    private PlayerMovement _playerMovement;
    private BoxCollider2D _topPipeCollider;
    private BoxCollider2D _botPipeCollider;
    private BoxCollider2D _scoringCollider;
    private bool _collidersDisabled = false;

    private bool _isGoingUp;
    private bool _isOscillating = true;
    private Vector3 _verticalMoveDirection = Vector3.zero;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        _topPipeCollider = transform.Find("TopPipe").GetComponent<BoxCollider2D>();
        _botPipeCollider = transform.Find("BotPipe").GetComponent<BoxCollider2D>();
        _scoringCollider = transform.Find("Scoring").GetComponent<BoxCollider2D>();
        if (player != null)
        {
            _playerTransform = player.transform;
            _playerMovement = player.GetComponent<PlayerMovement>();

        }
        //_isGoingUp = Random.value > 0.5f;
        if (GameModeController.Instance.GetGameMode() == GameMode.HardMode)
        {
            ChooseVerticalMoveDirection();
        }
    }



    void Update()
    {
        if (_playerMovement.IsDead && !_collidersDisabled)
        {
            if (_topPipeCollider != null) _topPipeCollider.enabled = false;
            if (_botPipeCollider != null) _botPipeCollider.enabled = false;
            if (_scoringCollider != null) _scoringCollider.enabled = false;
            _collidersDisabled = true;
        }
        else if (!_playerMovement.IsDead)
        {
            transform.position += _pipeSpeed * Time.deltaTime * Vector3.left;

            if (GameModeController.Instance.GetGameMode() == GameMode.HardMode)
            {
                VerticalMovement();
            }
        }

        if (_playerTransform != null && transform.position.x < _playerTransform.position.x - _destroyOffset)
        {
            Destroy(gameObject);
        }
    }


    private void ChooseVerticalMoveDirection()
    {
        if (transform.position.y < -1.5f)
        {
            _isGoingUp = true;
        }
        else if (transform.position.y > 1.5f)
        {
            _isGoingUp = false;
            _isOscillating = false;

        }
        else
        {
            _isGoingUp = UnityEngine.Random.value > 0.5f; // Randomly decide if the pipe is going up or down
        }

        _verticalMoveDirection = _isGoingUp ? Vector3.up : Vector3.down;
    }

    private void VerticalMovement()
    {
        // if (_isGoingUp)
        // {
        //     transform.position += _pipeSpeed * Time.deltaTime * Vector3.up * 0.3f; // Add some vertical movement in Hard Mode
        // }
        // else
        // {
        //     transform.position += _pipeSpeed * Time.deltaTime * Vector3.down * 0.3f; // Add some vertical movement in Hard Mode
        // }

        //transform.position += _pipeSpeed * Time.deltaTime * _verticalMoveDirection * 0.4f; // Add some vertical movement in Hard Mode
        if (_isOscillating)
        {
            transform.position += _pipeSpeed * 0.7f * Mathf.Sin(Time.time * 2.5f) * Time.deltaTime * _verticalMoveDirection;
        }
        else
        {
            transform.position += _pipeSpeed * Time.deltaTime * _verticalMoveDirection * 0.5f;
        }
    }
}

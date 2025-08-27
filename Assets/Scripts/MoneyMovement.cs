using System;
using UnityEngine;

public class MoneyMovement : MonoBehaviour
{
    [SerializeField] private float _destroyOffset = 10f; // Distance behind player to destroy
    [SerializeField] private float _passOffset = 6f;
    [SerializeField] private float _moneySpeed = 2.5f;
    private Transform _playerTransform;
    private PlayerMovement _playerMovement;

    private bool _playerPassed = false;
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
            _playerMovement = player.GetComponent<PlayerMovement>();

        }
    }

    void Update()
    {
        if (!_playerMovement.IsDead)
        {
            transform.position += _moneySpeed * Time.deltaTime * Vector3.left;
        }

        CheckIfPlayerpassed();

        if (_playerTransform != null && transform.position.x < _playerTransform.position.x - _destroyOffset)
        {
            Destroy(gameObject);
        }
    }

    private void CheckIfPlayerpassed()
    {
        if (!_playerPassed && _playerTransform != null
        && transform.position.x < _playerTransform.position.x - _passOffset)
        {
            GameManager.Instance.ResetComboCount();
            _playerPassed = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !_playerMovement.IsDead)
        {
            Destroy(gameObject);
        }
    }
}

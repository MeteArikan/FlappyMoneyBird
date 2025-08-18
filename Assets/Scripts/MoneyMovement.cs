using UnityEngine;

public class MoneyMovement : MonoBehaviour
{
    [SerializeField] private float _destroyOffset = 10f; // Distance behind player to destroy
    [SerializeField] private float _moneySpeed = 2.5f;
    private Transform _playerTransform;
    private PlayerMovement _playerMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
            _playerMovement = player.GetComponent<PlayerMovement>();

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_playerMovement.IsDead)
        {
            transform.position += _moneySpeed * Time.deltaTime * Vector3.left;
        }

        if (_playerTransform != null && transform.position.x < _playerTransform.position.x - _destroyOffset)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            // Logic for when the player collects the money
            Debug.Log("Money collected!");
            Destroy(gameObject);
        }
    }
}

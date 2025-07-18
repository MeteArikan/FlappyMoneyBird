using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    [SerializeField] private float _pipeSpeed = 3f;
    [SerializeField] private float _destroyOffset = 10f; // Distance behind player to destroy

    private Transform _playerTransform;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
    }

    void Update()
    {
        transform.position += _pipeSpeed * Time.deltaTime * Vector3.left;

        if (_playerTransform != null && transform.position.x < _playerTransform.position.x - _destroyOffset)
        {
            Destroy(gameObject);
        }
    }
}

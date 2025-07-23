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
        }

        if (_playerTransform != null && transform.position.x < _playerTransform.position.x - _destroyOffset)
        {
            Destroy(gameObject);
        }
    }
}

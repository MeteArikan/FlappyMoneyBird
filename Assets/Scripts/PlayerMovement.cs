using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    [SerializeField] private float _jumpSpeed = 3f;
    private Rigidbody2D _playerRigidbody;
    
    private void Start() {
        _playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerRigidbody.linearVelocity = Vector2.up * _jumpSpeed;
        }
    }
}

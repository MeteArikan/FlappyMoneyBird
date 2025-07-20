using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField] private GameObject _deathScreen;
    //[SerializeField] private Animator _playerAnimator;
    [SerializeField] private float _jumpSpeed = 3f;


    private Rigidbody2D _playerRigidbody;

    private void Start()
    {
        //_deathScreen.SetActive(false);
        //Time.timeScale = 1f;
        _playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
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



            //BirdFall();
            GameManager.Instance.OnGameOver();
        }
    }

    // private void BirdFall()
    // {
    //     _playerAnimator.SetBool("isDeath", true);
    //      AudioManager.Instance.Play(SoundType.DieSound);
    // }

    // private void PlayDieSound()
    // {
    //     AudioManager.Instance.Play(SoundType.DieSound);
    // }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    private Player _player;
    private BoxCollider2D _boxCollider;
    private Animator _animator;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _boxCollider = GetComponent<BoxCollider2D>();

        if(_player == null)
        {
            Debug.LogError("PLAYER IS NULL.");
        }

        _animator = GetComponent<Animator>();
        if(_animator == null)
        {
            Debug.LogError("ANIMATOR IS NULL.");
        }
    }
    void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Laser")
        {
            Destroy(other.gameObject);
            if(_player != null)
            {
                _player.AddScore(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _movementSpeed = 0;
            _boxCollider.enabled = false;
            Destroy(gameObject,2.8f);
        }
        
        if(other.gameObject.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                other.transform.GetComponent<Player>().Damage();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _movementSpeed = 0;
            _boxCollider.enabled = false;
            Destroy(gameObject,2.8f);
        }
    }

    private void Move()
    {
        transform.Translate(Vector3.down * _movementSpeed * Time.deltaTime);

        if (transform.position.y < -5.5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7.5f, transform.position.z);
        }
    }
}

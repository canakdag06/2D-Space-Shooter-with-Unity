using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private GameObject _laserPrefab;
    private Player _player;

    private BoxCollider2D _boxCollider;

    private Animator _animator;

    private AudioSource _audioSource;
    [SerializeField] AudioClip _explosionSound;

    private float _fireRate;
    private float _canFire = -1.0f;

    private bool isDead;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();
        isDead = false;

        if(_player == null)
        {
            Debug.LogError("PLAYER IS NULL.");
        }

        _animator = GetComponent<Animator>();
        if(_animator == null)
        {
            Debug.LogError("ANIMATOR IS NULL.");
        }

        if( _audioSource == null)
        {
            Debug.LogError("AUDIO SOURCE IS NULL.");
        }
        else
        {
            _audioSource.clip = _explosionSound;
        }
    }
    void Update()
    {
        Move();

        if(Time.time > _canFire && !isDead)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        }
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
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            isDead = true;
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
            _audioSource.Play();
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

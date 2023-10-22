using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float _speed, _speedMultiplier;
    private float _minX, _maxX, _minY, _maxY;
    [SerializeField] private GameObject _laserPrefab, _tripleShot, _shield, _rightEngine, _leftEngine;

    private float _nextShot = -1f;  // COOLDOWN
    [SerializeField] private float _cooldown = 0.5f;

    [SerializeField] private int _lives = 3;
    [SerializeField] private int _score;

    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private UIManager _uiManager;

    private bool _isTripleShotActive = false;
    private bool _isSpeedActive = false;
    private bool _isShieldActive = false;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _laserSound;

    private Animator _animator;

    private bool isDead;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        isDead = false;
        if(_spawnManager == null)
        {
            Debug.LogError("SPAWN MANAGER IS NULL.");
        }

        if(_uiManager == null)
        {
            Debug.LogError("UI MANAGER IS NULL.");
        }

        if(_audioSource == null)
        {
            Debug.LogError("AUDIO SOURCE IS NULL.");
        }
        else
        {
            _audioSource.clip = _laserSound;
        }
    }

    void Update()
    {
        Movement();
        Animate();
        if(Input.GetKey(KeyCode.Space) && Time.time > _nextShot)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        _nextShot = Time.time + _cooldown;
        
        if(!_isTripleShotActive)
        {
            Instantiate(_laserPrefab,
                        transform.position + new Vector3(0, 1.1f, 0),   // offset vector
                        Quaternion.identity);
        }
        else
        {
            Instantiate(_tripleShot,
                        transform.position,     // no offset, given by the prefab position
                        Quaternion.identity);
        }
        _audioSource.Play();
    }

    private void Movement()
    {
        if (isDead) { return; }
        float horizontalInput, verticalInput;
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(
            transform.position.x, 
            Mathf.Clamp(transform.position.y, -3.8f, 0), 
            0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    private void Animate()
    {
        _animator = GetComponent<Animator>();

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _animator.SetBool("isTurningLeft", true);
        }
        
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _animator.SetBool("isTurningRight", true);
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _animator.SetBool("isTurningLeft", false);
        }

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            _animator.SetBool("isTurningRight", false);
        }
    }

    public void Damage()
    {
        if(_isShieldActive)
        {
            _isShieldActive = false;
            _shield.SetActive(false);
            return;
        }


        _lives--;

        if(_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if(_lives == 1)
        {
            _leftEngine.SetActive(true);
        }


        _uiManager.UpdateLives(_lives);

        // check if dead
        if(_lives < 1)
        {
            isDead = true;
            _animator.SetTrigger("isDead");
            Destroy(this.gameObject,2f);
            _spawnManager.OnPlayerDeath();
        }
    }

    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotTimer());
    }

    IEnumerator TripleShotTimer()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    public void ActivateSpeed()
    {
        _isSpeedActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedTimer());
    }

    IEnumerator SpeedTimer()
    {
        yield return new WaitForSeconds(5f);
        _speed /= _speedMultiplier;
        _isSpeedActive = false;
    }

    public void ActivateShield()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}

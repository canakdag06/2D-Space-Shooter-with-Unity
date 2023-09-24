using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerups;
    private bool _stopSpawning = false;

    void Start()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerUp());
    }


    void Update()
    {
        
    }

    IEnumerator SpawnEnemy()
    {
        while(!_stopSpawning)
        {
            Vector3 spawnPos = new Vector3 (Random.Range(-8f, 8f), 7.3f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerUp()
    {
        while (!_stopSpawning)
        {
            yield return new WaitForSeconds(Random.Range(10f,20f));
            Vector3 spawnPos = new Vector3(Random.Range(-8f, 8f), 7.3f, 0);
            int randomPowerup = Random.Range(0, 3);
            GameObject newPowerUp = Instantiate(_powerups[randomPowerup], spawnPos, Quaternion.identity);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}

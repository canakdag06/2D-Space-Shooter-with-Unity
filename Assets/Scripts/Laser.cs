using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float speed;

    void Update()
    {
        if(transform.tag == "Laser")
        {
            MoveUp();
        }
        else if(transform.tag == "EnemyLaser")
        {
            MoveDown();
        }
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(this.tag == "EnemyLaser" && other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }
        }
    }
}

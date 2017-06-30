using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{

    public float maxSpeed = 600f;
    public int damageValue = 10;
    public GameObject explosion;
    public GameObject bigEnemy;

    private EnemyHealthScript enemyHealth;

    public void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(ObjectTags.ENEMY)) 
        {
            Instantiate(explosion, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag(ObjectTags.ENEMY_BIG))
        {
            enemyHealth = collision.gameObject.GetComponent<EnemyHealthScript>();
            enemyHealth.TakeDamage(damageValue);
            gameObject.SetActive(false);
        }
    }
}

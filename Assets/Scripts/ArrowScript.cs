using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

    public Vector3 v = new Vector3(0, 0, 0);
    public Vector3 a = new Vector3(0, 0, 0);
    public bool right = false;
    public GameObject explosion;
    public int damageValue;
    public GameObject player;
    private float distance;
    private float angle = 45f;
    private float gravity = 9.81f;
    private float elapsedTime = 0f;

    private PlayerHealthScript playerHealth;

    void Update()
    {
        distance = PlayerPrefs.GetFloat("distance", 0);

        elapsedTime += Time.deltaTime;

        float speed = Mathf.Sqrt((distance * gravity) / Mathf.Sin(2 * DegreeToRadian(angle))) - 1.25f;
        float vx = speed * Mathf.Cos(DegreeToRadian(angle));
        float vy = speed * Mathf.Sin(DegreeToRadian(angle)) - (gravity * elapsedTime);

        v = new Vector3(vx, vy, 0);
        a = new Vector3(0, gravity, 0);

        if (!right)
            v.x = -v.x;
        

        transform.position += v * Time.deltaTime;
        v += a * Time.deltaTime;

        transform.rotation = Quaternion.LookRotation(v, new Vector3(0, 0, 1));
    }

    private float DegreeToRadian(float angle)
    {
        return Mathf.PI * angle / 180.0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(ObjectTags.PLAYER))
        {

            playerHealth = collision.gameObject.GetComponent<PlayerHealthScript>();

            playerHealth.TakeDamage(damageValue);

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag(ObjectTags.TAIL))
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Transform arrowPrefab;
    public Transform hand;
    public Transform target;
    public float targetDistance = 15f;
    public float fireRate = 0.5f;
    public float arrowDelay = 1f;

    public bool lookRight = true;
    public float speed = 5;
    public LayerMask ground;


    private Vector3 targetPosition;
    private float lastShot = 0.0f;

    private Animator animator;

    private Vector3 facingDirection
    {
        get
        {
            return (hand.position - transform.position)
            .normalized;
        }
    }

    void Start()
    {
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    private int arrowCount = 0;

    IEnumerator makeArrow(float delay, bool right)
    {
        yield return new WaitForSeconds(delay);

        var arrowHit = Instantiate(arrowPrefab, hand.position, Quaternion.identity) as Transform;
        arrowHit.GetComponent<ArrowScript>().right = right;

        if (UnityEngine.Random.Range(0f, 1.0f) > 0.5f)
            animator.SetTrigger("attack");
        else
            animator.SetTrigger("special");

        arrowCount++;

    }

    void Update()
    {
        if (IsEnableEnemy())
        {
            Fire();

            if (targetPosition.x > transform.position.x && !lookRight)
                Flip();
            if (targetPosition.x < transform.position.x && lookRight)
                Flip();
        }
    }

    private void Fire()
    {
        if (CanFire())
        {
            StartCoroutine(makeArrow(arrowDelay, lookRight));
        }
    }

    private bool CanFire()
    {
        float distance = Vector3.Distance(target.position, hand.position);
        if (distance <= targetDistance && Time.time >= fireRate + lastShot)
        {
            PlayerPrefs.SetFloat("distance", distance);

            lastShot = Time.time;

            return true;
        }

        return false;
    }

    public void Flip()
    {
        var s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
        lookRight = !lookRight;
    }

    public bool IsEnableEnemy()
    {
        return PlayerPrefs.GetInt("EnemyEnabled", 0) == 0 ? false : true;
    }
}

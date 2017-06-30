using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour {

    void Start()
    {
        Invoke("Die", 0.5f);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}

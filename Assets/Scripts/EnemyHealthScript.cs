using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthScript : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;

    bool isDead;
    bool damaged;


    void Awake()
    {
        currentHealth = startingHealth;
    }


    void Update()
    {
    }


    public void TakeDamage(int amount)
    {
        damaged = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            if (!isDead && damaged)
            {
                Death();
            }
        }
    }


    void Death()
    {
        isDead = true;
        GameScript.gameManger.FinishGame();
    }
}

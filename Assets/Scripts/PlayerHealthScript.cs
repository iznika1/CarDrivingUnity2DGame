using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthScript : MonoBehaviour
{

    public int startingHealth = 100;                            
    public int currentHealth;                                 
    public UnityEngine.UI.Slider healthSlider;                                
    public AudioClip deathClip;                                                   

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
        GameScript.gameManger.GameOver();
    }
}

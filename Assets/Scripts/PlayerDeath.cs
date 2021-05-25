using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    //public GameObject deathEffect;
    public GameObject[] hearts;
    public int life;
    private bool dead;
    public int damage = 1;
    //public GameObject heart;

    private void OnCollisionEnter2D(Collision2D collision) { 
    if (collision.gameObject.CompareTag("Enemy")) {
            //Instantiate(deathEffect, transform.position, Quaternion.identity);
            //     Destroy(gameObject);
            //    LevelManager.instance.Respawn();
            DamagePlayer(damage);
          }
    }
    private void Start()
    {
        life = hearts.Length;
    }
       

    void Update()
    {
        if (dead == true)
        {
            Destroy(gameObject);
            
            LevelManager.instance.Respawn();
        }

        if (hearts == null)
       {
            life = hearts.Length;
            Instantiate(hearts[life]);
       }

                   }
    public void DamagePlayer(int damage)
    {
        life -= damage;
        Destroy(hearts[life].gameObject);
        if (life == 0)
        {
            dead = true;
        }
    }
    }
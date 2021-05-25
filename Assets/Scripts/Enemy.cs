using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    

    [SerializeField]
    float agroRange;

    [SerializeField]
    float moveSpeed;

    Rigidbody2D rb2d;

    public Transform player;

    public int health = 20;
    public GameObject deathEffect;

   // public int damage = 1;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
       // player = GameObject.FindWithTag("Player").transform;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        else
        {
            float distToPlayer = Vector2.Distance(transform.position, player.position);

            if (distToPlayer < agroRange)
            {
                ChasePlayer();
            }
            else
            {
                StopChasingPlayer();
            }
        }

                   
   
      //  void TurnOff()
        //{
          //  this.enabled = false;
        //}
    }

    

    void ChasePlayer()
    {
        if (transform.position.x < player.position.x)
        {
            rb2d.velocity = new Vector2(moveSpeed, 0);
        }
        else
        {
            rb2d.velocity = new Vector2(-moveSpeed, 0);
        }
    }

    void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

  //  void OnTriggerEnter2D(Collider2D hitInfo)
   // {
     //   PlayerDeath playa = hitInfo.GetComponent<PlayerDeath>();
      //  if (playa != null)
      //  {
      //      playa.DamagePlayer(damage);
     //       
     //   }

  //  }
        void StopChasingPlayer()
    {
        rb2d.velocity = new Vector2(0,0);
    }
}

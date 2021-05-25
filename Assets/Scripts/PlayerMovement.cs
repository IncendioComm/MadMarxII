using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;

    public Animator animator;
    public float acceleration = 10.6f;
    public float RunSpeed = 30.0f;
    public float MaxSpeed = 90.0f;
   

    float horizontalMove = 0f;

    bool jump = false;

    // Contador de tiempo para juego responsivo
    public float fJumpPressedRemember=0;
    //Para el salto
    public float fJumpPressedRememberTime = 0.2f;
    //Para not Grounded (saltar en el aire)


    // Update is called once per frame
    void Update()
    {
        // Modificar velocidad para obtener aceleración 
        if (RunSpeed < MaxSpeed && Input.GetAxisRaw("Horizontal") == 1)
        {
            RunSpeed += acceleration * Time.deltaTime;
        }

        if (RunSpeed < MaxSpeed && Input.GetAxisRaw("Horizontal") == -1)
        {
            RunSpeed += acceleration * Time.deltaTime;
        }

        horizontalMove = Input.GetAxisRaw("Horizontal") * RunSpeed;
        //Debug.Log("Al presionar obtengo" + Input.GetAxisRaw("Horizontal"));

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        // Delta de tiempo para control responsivo en el salto
        fJumpPressedRemember -= Time.deltaTime;
                                                                      
        if (Input.GetButtonDown("Jump"))
        {
            fJumpPressedRemember = fJumpPressedRememberTime;
        }

        if (fJumpPressedRemember>0)
        {
            // Manda el booleano jump a Character Controller
            //jump = true;
            animator.SetBool("IsJumping", true);

        }
    }
    
    public void OnLanding (){
        animator.SetBool("IsJumping", false);
    }

    void FixedUpdate ()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, false, fJumpPressedRemember);
            jump = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    public CharacterController controller;
    public PlayerMovement player;
    public Animator anim;
    public bool fall;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        player = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!fall &&!player.charging)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 && controller.IsGrounded())
            {
                anim.Play("Character_Walk");
            }
            else if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow))
            {
                anim.Play("Character_Jump");
            }
            else if (controller.IsGrounded() && player.hidden)
            {
                anim.Play("Character_Crouch");
            }
            else if (controller.IsGrounded())
            {
                anim.Play("Character_Idle");
            }
        }
        else {

            if ( fall & anim.GetCurrentAnimatorStateInfo(0).IsName("Character_Idle"))
            {
                fall = false;
            }
            if (player.charging) {
                anim.Play("Character_Shake_Light");

            }
           
        }
    }

   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "fall")
        {
            fall = true;
            anim.Play("Character_Falling");
            

        }
    }
}

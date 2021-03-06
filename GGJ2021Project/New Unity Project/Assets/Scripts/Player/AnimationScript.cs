﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AnimationScript : MonoBehaviour
{
    public CharacterController2D controller;
    public PlayerMovement player;
    public Animator anim;
    public bool fall;
    private AudioSource AS;
    public AudioClip walk;


    // Start is called before the first frame update
    void Awake()
    {
        AS = GetComponent<AudioSource>();

        controller = GetComponent<CharacterController2D>();
        player = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level2") { fall = true; }
    }
            /*

    // Update is called once per frame
    void Update()
    {
        Debug.Log(anim.GetCurrentAnimatorStateInfo(0).ToString());
        if (!fall &&!player.charging)
        {
           
            if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow))
            {
                anim.Play("Character_Jump");
            }
            else if (Input.GetAxisRaw("Horizontal") != 0 && controller.IsGrounded())
            {
                anim.Play("Character_Walk");
            }
            else if (controller.IsGrounded() && player.isHidden)
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
    */
}

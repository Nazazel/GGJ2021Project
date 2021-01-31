﻿using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VR;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal; //2019 VERSIONS
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// PlayeMovement handles the movement of the player by specifying player speed, reading user Input,
    /// and calling CharacterController2D to move the Player Object  
    /// </summary>

    [SerializeField] private float runSpeed;
    float horizontalMove = 0f;
    bool jump = false;
    public CharacterController controller;
    public GameObject flashLight;
    public Transform respawnPoint;
    public float respawnTime;

    public bool hidden;



    
    private Rigidbody2D rb2d;
    private bool respawning;
    private Vector2 preJumpVelocity;
    private float jumpDistance;
    private float duration;
    private SpriteRenderer SR;
    private float battery=100 ;
    private float currentBattery;
    public float batteryCharge;
    public float LightCost;
    private bool keyAlternate;
    public UIManager ui;

    public int collectableAmount=3;
    private int CurrentCollectables;
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        rb2d = GetComponent<Rigidbody2D>();
        respawning = false;
        jumpDistance = transform.position.y;
        SR = GetComponent<SpriteRenderer>();
        controller.SetLight(flashLight);
        keyAlternate = false;
        currentBattery = battery;
        flashLight.SetActive(false);

    }


    // Update is called once per frame
    void Update()
    {
        if (!respawning)
        {
            if (CurrentCollectables == collectableAmount) { Win(); }

            flashLight.transform.position = transform.position;
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
                jumpDistance = transform.position.y;
                preJumpVelocity = rb2d.velocity;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                LightSwitch();
            }

            if (controller.IsGrounded())
            {
                jumpDistance = transform.position.y;
            }

            if (hidden)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow)) { Hide(); }
                else if (Input.GetKeyUp(KeyCode.DownArrow)) { SR.enabled = true; }
            }

            if (Input.GetKeyDown(KeyCode.Q) && keyAlternate == false && !flashLight.active)
            {
                keyAlternate = true;
                ChargeBattery();

            }
            else if (Input.GetKeyDown(KeyCode.E) && keyAlternate == true && !flashLight.active)
            {
                keyAlternate = false;
                ChargeBattery();

            }

            if (currentBattery <= 0 && flashLight.active) { LightSwitch(); }
            if (currentBattery >= 0)
            {
                if (currentBattery - LightCost >= 0)
                    currentBattery -= LightCost;
                else
                    currentBattery = 0;

            }
            if (flashLight.active)
            {

                if (currentBattery > 10)
                    flashLight.GetComponent<Light2D>().intensity = (currentBattery / 100);
                else
                    flashLight.GetComponent<Light2D>().intensity = .1f;

            }

            ui.setAmount(currentBattery);
        }
    }

    // FixedUpdate is called multiple times per x amount of frames
    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;

        
    }


    private void Win() 
    {
        GameManager.won = true;
    }

    public void ChargeBattery() {
        if (currentBattery + batteryCharge > battery) { currentBattery = battery; }
        else {
            currentBattery += batteryCharge;
        }
    
    }

    #region  triggers

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "hurtbox")
        {
            death();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "hurtbox")
        {
            if (other.gameObject.GetComponent<EyeTrap>() != null) {
                if (!other.gameObject.GetComponent<EyeTrap>().activated) { }
                    other.gameObject.GetComponent<EyeTrap>().Trip();
                    death(); 
            }
            else
            {
                death();
            }

        }
        else if (other.tag == "Hide")
        {
            hidden = true;
        }
        else if (other.tag == "checkpoint")
        {
            respawnPoint = other.transform;   
        }
        else if (other.tag == "pickup")
        {
            CurrentCollectables += 1;
            Destroy(other.gameObject);
        }

    }



    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Hide")
        {
            Unhide();
        }
    }
    #endregion

    //off on light
    public void LightSwitch() {
        if (flashLight.activeSelf)
        {
            flashLight.SetActive(false);
        }
        else {
            if (battery > 0)
            {
                flashLight.SetActive(true);
            }


        }
    }

    //hide 
    public void Hide() {
        SR.enabled = false;
        if (flashLight.active) { LightSwitch(); }
    }
    public void Unhide() {
        SR.enabled = true ;
        hidden = false;
    }



    #region  deaths
    private void death()
    {

        transform.position = respawnPoint.transform.position;
        rb2d.velocity = Vector3.zero;
        StartCoroutine("respawn");

    }

    #endregion

    IEnumerator respawn()
    {
        respawning = true;
        SR.enabled = false;
        yield return new WaitForSeconds(respawnTime);
        respawning = false;
        currentBattery = battery;
        SR.enabled = true;
        rb2d.velocity = Vector3.zero;


    }
}

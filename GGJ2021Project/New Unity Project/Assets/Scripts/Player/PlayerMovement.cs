using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VR;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal; //2019 VERSIONS
using UnityEngine.UI;
using System.Threading;

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
    public bool isHidden;
    public bool charging=false;
    public float waitCharge;
    private float time = 0; 
    private Rigidbody2D rb2d;
    private bool respawning;
    private Vector2 preJumpVelocity;
    private float jumpDistance;
    private float duration;
    private Collider2D col;
    private SpriteRenderer SR;
    private float battery=100 ;
    private float currentBattery;
    public float batteryCharge;
    public float LightCost;
    public float LightMultiplier=2;
    private bool keyAlternate;
    public UIManager ui;
    public Chase maggot;

    private AudioSource AS;
    public AudioClip fonn;
    public AudioClip foff;
    public AudioClip scream;




    public int collectableAmount=3;
    private int CurrentCollectables;
    void Awake()
    {
        AS = GetComponent<AudioSource>();
        CurrentCollectables = GameManager.count;
        controller = GetComponent<CharacterController>();
        rb2d = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        respawning = false;
        jumpDistance = transform.position.y;
        SR = GetComponent<SpriteRenderer>();
        controller.SetLight(flashLight);
        keyAlternate = false;
        currentBattery = battery;
        flashLight.SetActive(false);
        maggot = FindObjectOfType<Chase>();


    }

    private void Start()
    {
        ui.Disable();
        ui.counter(CurrentCollectables);
    }

    // Update is called once per frame
    void Update()
    {
        if (charging) {

            time += .01f;
            if (time >= waitCharge) { charging = false; }
                }
        if (!respawning)
        {
            if (CurrentCollectables == collectableAmount) { Win(); }

            flashLight.transform.position = transform.position;
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            if (Input.GetButtonDown("Jump")||Input.GetKeyDown(KeyCode.UpArrow) )
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
                if (Input.GetKeyDown(KeyCode.DownArrow )|| Input.GetKeyDown(KeyCode.S)) { Hide(); }
                else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S)) { Unhide(); }
            }
            if (Input.GetAxisRaw("Horizontal") == 0 && controller.IsGrounded())
            {
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
            }

            if (currentBattery <= 0 && flashLight.active) { LightSwitch(); }
            if (currentBattery >= 0)
            {
                if (currentBattery - LightCost >= 0 &&!flashLight.active)
                    currentBattery -= LightCost;
                else if (currentBattery - (LightCost*LightMultiplier) >= 0 && flashLight.active)
                    currentBattery -= (LightCost * LightMultiplier);
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
        time = 0;
        charging = true;

        if (currentBattery + batteryCharge > battery) { currentBattery = battery; }
        else {
            currentBattery += batteryCharge;
        }
        Trigger();
    
    }

    #region  triggers

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "hurtbox")
        {
            if (!other.gameObject.GetComponent<Chase>().stunned && !other.gameObject.GetComponent<Chase>().trapped && !isHidden)
            {


                death();
            }
        }
    }
   
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "hurtbox" )
        {
            if (other.gameObject.GetComponent<EyeTrap>() != null) {
                if (!other.gameObject.GetComponent<EyeTrap>().activated)
                {
                    other.gameObject.GetComponent<EyeTrap>().Trip();
                    death();
                }

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
            GameManager.count++;
            other.GetComponent<Collectable>().Collect();
            Destroy(other.gameObject);
            ui.counter(CurrentCollectables);

        }
        else if (other.tag == "area")
        {
            other.GetComponent<teleportActivator>().MessageAI();
        }
        if (other.tag == "transition") 
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        


    }

    public void Trigger()
    {
        if (maggot != null)
        {
            maggot.Alert();
        }
        else if (GameManager.maggot != null) {
            maggot = GameManager.maggot.GetComponent<Chase>();
            maggot.Alert();


        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Hide")
        {
            hidden = false;
            Unhide();
        }
    }
    #endregion

    //off on light
    public void LightSwitch() {
        if (flashLight.activeSelf)
        {
            flashLight.SetActive(false);
            AS.PlayOneShot(foff);
        }
        else {
            if (battery > 0)
            {
                AS.PlayOneShot(fonn);
                flashLight.SetActive(true);
            }


        }
    }

    //hide 
    public void Hide() {
        SR.enabled = false;
        gameObject.layer = 11;
        if (flashLight.active) { LightSwitch(); }
        isHidden = true;
    }
    public void Unhide() {
        gameObject.layer = 0;
        isHidden = false;
        SR.enabled = true ;
    }



    #region  deaths
    private void death()
    {
        ui.Die();
        AS.PlayOneShot(scream);
        if (flashLight.activeSelf) { LightSwitch(); }
        transform.position = respawnPoint.transform.position;
        rb2d.velocity = Vector3.zero;
        if (maggot != null)
        {
            maggot.Calm();
        }
        else if (GameManager.maggot != null)
        {
            maggot = GameManager.maggot.GetComponent<Chase>();
            maggot.Calm();


        }
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

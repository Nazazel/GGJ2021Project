using System.Collections;
using System.Collections.Generic;
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
    public CharacterController2D controller;
    public GameObject flashLight;
    public Transform respawnPoint;
    public float respawnTime;
    public float batteryChargeRate;
    public float startingCharge;
    public bool inHideObject;
    public bool hidden;
    public bool isHidden;
    public bool charging=false;
    public float waitCharge;
    public float chargeMultiplier;
    public float LightCost;
    public float LightMultiplier=2;
    public UIManager ui;
    public Chase maggot;
    public GameObject lightSpot;
    public AudioSource backgroundAudio;
    public AudioClip patrol;
    public AudioClip sus;
    public AudioClip alert;
    public AudioClip fonn;
    public AudioClip foff;
    public AudioClip scream;
    public AudioClip shake;
    public AudioClip toy;
    public AudioClip crouch;
    public AudioClip uncrouch;


    private float time = 0; 
    private Rigidbody2D rb2d;
    private bool respawning;
    private Vector2 preJumpVelocity;
    private float jumpDistance;
    private float duration;
    private Collider2D col;
    private SpriteRenderer SR;
    public float batteryMaxCharge= 100 ;
    public float currentBattery;
    private float ogCharge;
    private bool keyAlternate;
    private AudioSource AS;

    private Light2D finalLight;
    private Light2D globalLight;
    private bool finalTransition = false;
    private float t = 0;
    private float initialLightIntensity;
    public bool fall;

    private Animator anim;


    public int collectableAmount=3;
    private int CurrentCollectables;
    void Awake()
    {
        fall = false;
        ogCharge = batteryChargeRate;
        anim = GetComponent<Animator>();
        inHideObject = false;
        AS = GetComponent<AudioSource>();
        CurrentCollectables = GameManager.count;
        controller = GetComponent<CharacterController2D>();
        rb2d = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        respawning = false;
        jumpDistance = transform.position.y;
        SR = GetComponent<SpriteRenderer>();
        controller.SetLight(flashLight);
        keyAlternate = false;
        currentBattery = 100f;
        flashLight.SetActive(false);
        maggot = FindObjectOfType<Chase>();

        if (SceneManager.GetActiveScene().name == "Level3")
        {
            finalLight = GameObject.FindGameObjectWithTag("finalLight").GetComponent<Light2D>();
            initialLightIntensity = finalLight.intensity;
            globalLight = GameObject.FindGameObjectWithTag("globalLight").GetComponent<Light2D>();
        }

    }

    private void Start()
    {
        ui.Disable();
        ui.counter(CurrentCollectables);
    }

    // Update is called once per frame
    void Update()
    {
        if (!fall)
        {
            if (charging)
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Character_Shake_Light"))
                    anim.Play("Character_Shake_Light");

                batteryChargeRate *= chargeMultiplier;
                time += .01f;
                if (time >= waitCharge || (Input.GetAxisRaw("Horizontal") != 0 || Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) || Input.GetKeyDown(KeyCode.LeftShift))
                {
                    AS.Stop();

                    charging = false;
                }

            }
            else
            {

                batteryChargeRate = ogCharge;
                if (!respawning)
                {

                    flashLight.transform.position = lightSpot.transform.position;
                    horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
                    if (horizontalMove == 0 && (controller.IsGrounded()) && !isHidden)
                    {
                        anim.Play("Character_Idle");
                    }
                    if (isHidden && controller.IsGrounded() && horizontalMove == 0) { anim.Play("Character_Crouch"); }
                    if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        jump = true;
                        jumpDistance = transform.position.y;
                        preJumpVelocity = rb2d.velocity;
                    }
                    if (horizontalMove != 0 && (controller.IsGrounded())) { anim.Play("Character_Walk"); }
                    if (!controller.IsGrounded())
                    {
                        anim.Play("Character_Jump");
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
                        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) { Hide(); }
                        else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S)) { Unhide(); }
                    }
                    if (!hidden && isHidden) { Unhide(); }


                    if (currentBattery <= 0 && flashLight.activeSelf) { LightSwitch(); }
                    if (currentBattery >= 0)
                    {
                        if (currentBattery - LightCost >= 0 && !flashLight.activeSelf)
                            currentBattery -= LightCost;
                        else if (currentBattery - (LightCost * LightMultiplier) >= 0 && flashLight.activeSelf)
                            currentBattery -= (LightCost * LightMultiplier);
                        else
                            currentBattery = 0;

                    }
                    if (flashLight.activeSelf)
                    {

                        if (currentBattery > 10)
                            flashLight.GetComponent<Light2D>().intensity = (currentBattery / 100);
                        else
                            flashLight.GetComponent<Light2D>().intensity = .1f;

                    }

                }
                else
                {
                    rb2d.velocity = Vector2.zero;


                }
            }
            ui.setAmount(currentBattery);

            if (Input.GetAxisRaw("Horizontal") == 0 && controller.IsGrounded())
            {
                if (Input.GetKeyDown(KeyCode.Q) && keyAlternate == false && !flashLight.activeSelf)
                {
                    keyAlternate = true;
                    ChargebatteryMaxCharge();

                }
                else if (Input.GetKeyDown(KeyCode.E) && keyAlternate == true && !flashLight.activeSelf)
                {
                    keyAlternate = false;
                    ChargebatteryMaxCharge();

                }
            }
            if (finalTransition)
            {
                t += Time.deltaTime * 0.2f;
                finalLight.intensity = Mathf.Lerp(initialLightIntensity, 0, t);
                globalLight.intensity = Mathf.Lerp(0.1f, 0, t);
                if (t >= 1)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
        }
        else {

            if (fall & anim.GetCurrentAnimatorStateInfo(0).IsName("Character_Idle"))
            {
                fall = false;
            }
        }
    }

    // FixedUpdate is called multiple times per x amount of frames
    private void FixedUpdate()
    {
        if (!respawning)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
            jump = false;
        }
        
    }


    public void MusicController(Chase.State s) {
        switch (s) {

            case Chase.State.patrol:
                if (backgroundAudio.clip != patrol)
                {
                    backgroundAudio.clip = patrol;
                    backgroundAudio.Play();

                }
                break;
            case Chase.State.suspicious:
                backgroundAudio.clip = sus;
                backgroundAudio.Play();


                break;
            case Chase.State.alerted:
                backgroundAudio.clip = alert;
                backgroundAudio.Play();

                break;
        }
    
    
    }

    private void Win() 
    {
        GameManager.won = true;
    }


    public void ChargebatteryMaxCharge() {
        time = 0;
        charging = true;
        AS.PlayOneShot(shake);
        if (currentBattery + batteryChargeRate > batteryMaxCharge) { currentBattery = batteryMaxCharge; }
        else {
            currentBattery += batteryChargeRate;
        }
        Trigger();
    
    }

    #region  triggers

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "hurtbox")
        {
            if (other.gameObject.GetComponent<Chase>() != null)
            {
                if (!other.gameObject.GetComponent<Chase>().stunned && !other.gameObject.GetComponent<Chase>().trapped && !isHidden)
                {


                    death();
                }
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
            inHideObject = true;
        }
        else if (other.tag == "checkpoint")
        {
            respawnPoint = other.transform;   
        }
        else if (other.tag == "pickup")
        {
            AS.PlayOneShot(toy);
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
            if (finalLight)
            {
                finalTransition = true;
            }
            else
            {
                ui.Transition();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        if (other.tag == "fall")
        {
            fall = true;
            anim.Play("Character_Falling");


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
            inHideObject = false;

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
            if (batteryMaxCharge > 0)
            {
                AS.PlayOneShot(fonn);
                flashLight.SetActive(true);
            }


        }
    }

    //hide 
    public void Hide() {
        AS.PlayOneShot(crouch);
        gameObject.layer = 11;
        SR.sortingOrder = 4;
        if (flashLight.activeSelf) { LightSwitch(); }
        isHidden = true;
    }
    public void Unhide() {
        if (isHidden) { AS.PlayOneShot(uncrouch); }
        SR.sortingOrder = 10;
        gameObject.layer = 0;
        isHidden = false;
        SR.enabled = true ;
    }



    #region  deaths
    private void death()
    {

        ui.Die();
        AS.PlayOneShot(scream);
        if (GameManager.maggot != null) { Destroy(GameManager.maggot); }
        if (flashLight.activeSelf) { LightSwitch(); }
        transform.position = respawnPoint.transform.position;
        rb2d.velocity = Vector2.zero;
        if (maggot != null)
        {
          //  maggot.Calm();
        }
        else if (GameManager.maggot != null)
        {
            maggot = GameManager.maggot.GetComponent<Chase>();
            //maggot.Calm();
        }
        StartCoroutine("respawn");

    }

    #endregion

    IEnumerator respawn()
    {
        respawning = true;
        SR.enabled = false;
        rb2d.velocity = Vector3.zero;
        inHideObject = false;
        hidden = false;
        yield return new WaitForSeconds(respawnTime);
        respawning = false;
        currentBattery = batteryMaxCharge;
        SR.enabled = true;
        rb2d.velocity = Vector3.zero;
        backgroundAudio.clip = patrol;
        backgroundAudio.Play();


    }
}

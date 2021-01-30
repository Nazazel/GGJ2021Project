using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
   

    private Rigidbody2D rb2d;
    private bool respawning;
    private Vector2 preJumpVelocity;
    private float jumpDistance;
    private float duration;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        rb2d = GetComponent<Rigidbody2D>();
        respawning = false;
        jumpDistance = transform.position.y;
        StartCoroutine("respawn");

    }

    IEnumerator WaitForSoundReset()
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            jumpDistance = transform.position.y;
            preJumpVelocity = rb2d.velocity;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            LightSwitch();
        }
   
        if (controller.IsGrounded())
        {
            jumpDistance = transform.position.y;
        }
    }

    // FixedUpdate is called multiple times per x amount of frames
    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }


    #region  triggers
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Death")
        {
            death();
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
     
    }
    #endregion

    public void LightSwitch() {
        flashLight.SetActive(!flashLight.active);
    
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
        yield return new WaitForSeconds(respawnTime);
        respawning = false;
    }
}

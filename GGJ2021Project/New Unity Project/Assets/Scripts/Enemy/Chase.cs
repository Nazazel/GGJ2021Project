using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : MonoBehaviour
{
    public GameObject target;
    public GameObject player;
    private PlayerMovement play;
    private NavMeshAgent agent;
    public GameObject[] wayPoints;
    public GameObject[] SpawnPoints;
    public bool stunned;
    public bool trapped;
    public bool m_FacingRight = true;
    private Animator animator;
    public float stunTime;
    public float TrapTime;
    public float toFar = 10;
    private bool stop;
    public enum State { patrol, suspicious, alerted }
    public State state;
    public float PatrolSpeed;
    public float suspiciousSpeed;
    public float SuspiciousDistance;
    public float AlertedSpeed;
    public float AlertedDistance;
    public float LoseInterestRange;
    private Rigidbody2D rb;
    private Collider2D c2d;
    private Coroutine co;
    private AudioSource AS;
    public AudioClip move;
    public AudioClip[] sus;
  
    public AudioClip[] alert;
    public AudioClip stun;

    //charge
    public float totalbatteryMaxCharge;
    private float currentBattery;
    public float IncrementAmount;
    public bool open = false;
    public bool active = false;



    // Start is called before the first frame update
    private void Awake()
    {
        AS = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        if (player == null) { player = GameObject.FindGameObjectWithTag("Player"); }
        c2d = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        state = State.patrol;
        play = player.GetComponent<PlayerMovement>();
        stunned = false;
        trapped = false;
        GameManager.maggot = gameObject;

    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = PatrolSpeed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        SetWayPoint();
    }




    // Update is called once per frame
    void Update()
    {

        if (active && !open)
        {
            currentBattery += IncrementAmount;
            if (currentBattery >= totalbatteryMaxCharge)
            {
                open = true;
            }
        }
        if (open) { Trip(); }

        Move(agent.velocity.x);

        //set states
        if (!trapped && !stunned)
        {

            if (state == State.alerted && (Vector2.Distance(transform.position, player.transform.position) >toFar)) { SetWayPointDistanceBased(); }
            else if ((Vector2.Distance(transform.position, player.transform.position)) < AlertedDistance && !stop && !play.isHidden && state!=State.alerted)
            {
                if (state != State.alerted)
                {
                    state = State.alerted;
                    AS.PlayOneShot(alert[Random.Range(0, sus.Length)]);
                    play.MusicController(state);

                }
            }
            else if ((Vector2.Distance(transform.position, player.transform.position)) < SuspiciousDistance && !stop && !play.isHidden && state != State.alerted)
            {
                if (state != State.suspicious)
                {
                    state = State.suspicious;
                    AS.PlayOneShot(sus[Random.Range(0,sus.Length)]);
                    play.MusicController(state);

                }

            }

            else if ((Vector2.Distance(transform.position, target.transform.position)) < 1f && !stop && (target != player) && state != State.alerted)
            {
              
                    state = State.patrol;

                SetWayPoint();
               

            }
            else if ((Vector2.Distance(transform.position, player.transform.position)) < 1f && !stop && (target == player) && state != State.alerted)
            {
                
                animator.SetBool("attacking", true);
                AS.PlayOneShot(alert[Random.Range(0, sus.Length)]);

                SetWayPoint();



            }
        }


        switch (state) {

            case State.alerted:
                animator.SetBool("alerted",true);
                agent.speed = AlertedSpeed;
                target = player;
                gameObject.layer = 0;
                play.isHidden = false;
             //   if (Vector2.Distance(transform.position, target.transform.position) > LoseInterestRange) { state = State.patrol; }
                break;
            case State.suspicious:
                animator.SetBool("alerted", false);
                agent.speed = suspiciousSpeed;
                target = player;
                gameObject.layer = 12;
                //if (Vector2.Distance(transform.position, target.transform.position) > LoseInterestRange) { state = State.patrol; }
                break;

            case State.patrol:
                animator.SetBool("alerted", false);
                agent.speed = PatrolSpeed;
                if (target == player) { SetWayPoint(); }
                gameObject.layer = 12;
                break;


        }
        if (GameManager.won)
        {
            agent.isStopped = true;
            
        }
        else
        {
            agent.SetDestination(target.transform.position);
        }
    }

    private void Move(float move) {
        if (move > 0 && !m_FacingRight)
            Flip();

        else if (move < 0 && m_FacingRight)
            Flip();
    }

    

IEnumerator Stun()
    {
        rb.velocity = Vector2.zero;
        agent.isStopped = true;
        rb.isKinematic = true;
        c2d.isTrigger = true;
        stunned = true;
        yield return new WaitForSeconds(stunTime);
        open = false;
        currentBattery = 0;
        stunned = false;
        c2d.isTrigger = false;
        rb.isKinematic = false;
        agent.isStopped = false;
        animator.SetBool("stunned", false);
        SetWayPoint();

    }

    IEnumerator TrapStun()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        agent.isStopped = true;
        trapped = true;
        yield return new WaitForSeconds(TrapTime);
        open = false;
        currentBattery = 0;
        trapped = false;
        rb.isKinematic = false;
        agent.isStopped=false;
        animator.SetBool("stunned", false);
        SetWayPoint();
    }

    public void SetWayPoint() {
        if (wayPoints.Length >= 1)
        {
            while (true)
            {
                GameObject x = wayPoints[Random.Range(0, wayPoints.Length)];
                if (x != target) { target = x; break; }
            }
            state = State.patrol;
            play.MusicController(state);
            AS.PlayOneShot(move);

            agent.SetDestination(target.transform.position);
        }

    }
    public void SetWayPointDistanceBased()
    {
        if (wayPoints.Length >= 1)
        {

            GameObject x = wayPoints[0];
            float distance= Vector3.Distance(player.transform.position, x.transform.position);
            foreach (GameObject waypoint in wayPoints) {
                if (Vector3.Distance(player.transform.position, waypoint.transform.position) < distance) { x = waypoint; }
            
            }
                
            
            state = State.patrol;
            play.MusicController(state);
            AS.PlayOneShot(move);

            agent.SetDestination(target.transform.position);
        }

    }

    public void Redo()
    {
        if (!open)
        {
            active = false;
            currentBattery = 0;
        }
    }

    public void Lit()
    {
        if (!open)
        {
            active = true;


        }

    }

    

    public void Trip() {
        if (co == null)
        {
            animator.SetBool("stunned", true);

            co = StartCoroutine(Stun());
        }
        else if (!trapped) {
            animator.SetBool("stunned", true);

            StopCoroutine(co);
            co = StartCoroutine(Stun());

        }
    }



    public void Trapped() {

        if (co == null)
        {
            animator.SetBool("stunned", true);

            co = StartCoroutine(TrapStun());
        }
        else if(!trapped){
            animator.SetBool("stunned", true);
            StopCoroutine(co);
            co = StartCoroutine(TrapStun());
        }
    }

    void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void Alert() {
        if (state != State.suspicious)
        {
            AS.PlayOneShot(sus[Random.Range(0, sus.Length)]);
            state = State.suspicious;
            play.MusicController(state);


        }
        target = player;
    }

    public void Calm() {
        animator.SetBool("attacking", true);
        state = State.patrol;

        SetWayPoint();

    }
    public  void Respawn(Vector3 position, GameObject[] waypoints)  
    {
       
        GameObject x = Instantiate(gameObject, position, transform.rotation);
        x.GetComponent<Chase>().wayPoints = waypoints;
        Destroy(gameObject);

    }

}

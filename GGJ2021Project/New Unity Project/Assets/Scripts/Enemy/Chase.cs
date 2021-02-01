using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
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
    public AudioClip sus;
    public AudioClip alert;
    public AudioClip stun;




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

        Move(agent.velocity.x);

        //set states
        if (!trapped && !stunned)
        {


            if (state == State.alerted && (Vector2.Distance(transform.position, player.transform.position)) < 1f) { state = State.patrol; }
            else if ((Vector2.Distance(transform.position, player.transform.position)) < AlertedDistance && !stop && !play.isHidden && state!=State.alerted)
            {
                if (state != State.alerted)
                {
                    state = State.alerted;
                    AS.PlayOneShot(alert);

                }
            }
            else if ((Vector2.Distance(transform.position, player.transform.position)) < SuspiciousDistance && !stop && !play.isHidden && state != State.alerted)
            {
                if (state != State.suspicious)
                {
                    state = State.suspicious;
                    AS.PlayOneShot(sus);
                }
               



            }

            else if ((Vector2.Distance(transform.position, target.transform.position)) < 1f && !stop && (target != player) && state != State.alerted)
            {
              
                    state = State.patrol;
                    AS.PlayOneShot(move);

                    SetWayPoint();
               

            }
            else if ((Vector2.Distance(transform.position, player.transform.position)) < 1f && !stop && (target == player) && state != State.alerted)
            {
                
                animator.SetBool("attacking", true);
                state = State.patrol;
                AS.PlayOneShot(alert);

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
        if (GameManager.won) { agent.Stop(); }
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
        agent.Stop();
        rb.isKinematic = true;
        c2d.isTrigger = true;
        stunned = true;
        yield return new WaitForSeconds(stunTime);
        stunned = false;
        c2d.isTrigger = false;
        rb.isKinematic = false;
        agent.Resume();
        animator.SetBool("stunned", false);
        SetWayPoint();

    }

    IEnumerator TrapStun()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        agent.Stop();
        trapped = true;
        yield return new WaitForSeconds(TrapTime);
        trapped = false;
        rb.isKinematic = false;
        agent.Resume();
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
            agent.SetDestination(target.transform.position);
        }

    }

    public void Lit() {
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
        AS.PlayOneShot(sus);

        state = State.suspicious;
        target = player;
    }

    public void Calm() {
        animator.SetBool("attacking", true);
        state = State.patrol;
        AS.PlayOneShot(move);

        SetWayPoint();

    }
    public  void Respawn(Vector3 position, GameObject[] waypoints)  
    {
       
        GameObject x = Instantiate(gameObject, position, transform.rotation);
        x.GetComponent<Chase>().wayPoints = waypoints;
        Destroy(gameObject);

    }

}

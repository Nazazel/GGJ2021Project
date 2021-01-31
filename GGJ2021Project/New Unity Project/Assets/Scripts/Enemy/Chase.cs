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

    // Start is called before the first frame update
    private void Awake()
    {
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
                state = State.alerted;


            }
            else if ((Vector2.Distance(transform.position, player.transform.position)) < SuspiciousDistance && !stop && !play.isHidden && state != State.alerted)
            {
                state = State.suspicious;


            }

            else if ((Vector2.Distance(transform.position, target.transform.position)) < 1f && !stop && (target != player) && state != State.alerted)
            {

                SetWayPoint();


            }
            else if ((Vector2.Distance(transform.position, player.transform.position)) < 1f && !stop && (target == player) && state != State.alerted)
            {

                SetWayPoint();


            }
        }


        switch (state) {

            case State.alerted:
                agent.speed = AlertedSpeed;
                target = player;
                gameObject.layer = 0;
                play.isHidden = false;
             //   if (Vector2.Distance(transform.position, target.transform.position) > LoseInterestRange) { state = State.patrol; }
                break;
            case State.suspicious:
                agent.speed = suspiciousSpeed;
                target = player;
                gameObject.layer = 12;
                //if (Vector2.Distance(transform.position, target.transform.position) > LoseInterestRange) { state = State.patrol; }
                break;

            case State.patrol:
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
        stunned = true;
        yield return new WaitForSeconds(stunTime);
        stunned = false;
        rb.isKinematic = false;
        agent.Resume();
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
            co = StartCoroutine(Stun());
        }
        else if (!trapped) {
            StopCoroutine(co);
            co = StartCoroutine(Stun());

        }
    }

    public void Trapped() {
        if (co == null)
        {
            co = StartCoroutine(TrapStun());
        }
        else if(!trapped){
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
        state = State.alerted;
        target = player;
    }

    public  void Respawn(Vector3 position, GameObject[] waypoints)  
    {
       
        GameObject x = Instantiate(gameObject, position, transform.rotation);
        x.GetComponent<Chase>().wayPoints = waypoints;
        Destroy(gameObject);

    }

}

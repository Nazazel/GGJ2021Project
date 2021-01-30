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
    private Coroutine co;

    // Start is called before the first frame update
    private void Awake()
    {
        state = State.patrol;
        play = player.GetComponent<PlayerMovement>();
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = PatrolSpeed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        //set states
        if (!trapped && !stunned)
        {
            if ((Vector2.Distance(transform.position, player.transform.position)) < .1f && !stop)
            {
                SetWayPoint();


            }
            else if (state == State.alerted)
            {


            }

            else if ((Vector2.Distance(transform.position, player.transform.position)) < SuspiciousDistance && !stop && !play.hidden)
            {
                state = State.suspicious;


            }
            else if ((Vector2.Distance(transform.position, player.transform.position)) < AlertedDistance && !stop && !play.hidden)
            {
                state = State.alerted;


            }
            else
            {
                state = State.patrol;

            }
        }


        switch (state) {

            case State.alerted:
                agent.speed = AlertedSpeed;
                target = player;
                break;
            case State.suspicious:
                agent.speed = suspiciousSpeed;
                target = player;
                break;
            case State.patrol:
                agent.speed = PatrolSpeed;
                if (target == player) { SetWayPoint(); }
                break;


        }

        agent.SetDestination(target.transform.position);
    }

    IEnumerator Stun()
    {
        agent.Stop();
        stunned = true;
        yield return new WaitForSeconds(stunTime);
        stunned = false;
        SetWayPoint();

    }

    IEnumerator TrapStun()
    {
        agent.Stop();
        trapped = true;
        yield return new WaitForSeconds(TrapTime);
        trapped = false;
        SetWayPoint();
    }

    public void SetWayPoint() {
            
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

    public  void Respawn(Vector3 position, GameObject[] waypoints)  
    {
        transform.position = position;
        wayPoints = waypoints;
        trapped = false;
        stunned = false;
        SetWayPoint();
    }
        
        }

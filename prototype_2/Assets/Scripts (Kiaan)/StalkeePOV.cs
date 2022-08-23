using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class StalkeePOV : MonoBehaviour
{   
    //Movement
    NavMeshAgent agent;
    public Transform[] waypoints;
    int i;
    Vector3 target;
    //POV
    public float sightD = 15f;
    public GameObject player;
    Vector3 direction;
    float angle;
    RaycastHit hit;
    public float FOVAngle = 160f;
    public Animator anim;
    bool isIdle;

    bool isWalking;
    bool isTurning;

    float minDist = 5f;
    float maxDist = 10f;
    float Dist;

    [SerializeField]
    private CurrentState _CurrentState;
    enum CurrentState
    {
        Idle,
        Walking,
        Looking

    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        UpdateDestination();
        _CurrentState = CurrentState.Idle;

    }
    private void AnimationChecker()
    {
        if (_CurrentState == CurrentState.Idle)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isLooking", false);
        }
        else if (_CurrentState == CurrentState.Walking)
        {
            anim.SetBool("isWalking", true);
            anim.SetBool("isLooking", false);
        }

        else if (_CurrentState == CurrentState.Looking)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isLooking", true);
        }


    }

    private void Update()
    {
        DistBetPlayers();
        AnimationChecker();
       // if (isWalking)
       // {
       //     anim.Play("Walk");
       // }
       // else if (agent.transform.position == waypoints[i].position)
       // {
       //     anim.Play("Idle");
       //     isTurning = true;
       // }

       //else if (isTurning)
       // {
       //     anim.Play("Turn");
       // }

        //AI Movement
        if (Vector3.Distance(transform.position,target)<1)
        {
            IterateWayPoint();
            UpdateDestination();
            Check();
            _CurrentState = CurrentState.Idle;
            _CurrentState = CurrentState.Looking;



        }

        //AI Sight and Detection
        direction = player.transform.position - this.transform.position;
        angle = Vector3.Angle(direction, this.transform.forward);
        if (angle < FOVAngle * 0.5f)
        {
            if (Physics.Raycast(new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z), direction.normalized, out hit, sightD))
            {
                if (hit.collider.tag == "Player")
                {
                    Debug.Log("SUS");
                }
            }
        }

       
    }

    //Set Destination
    void UpdateDestination()
    {
        target = waypoints[i].position;
        _CurrentState = CurrentState.Walking;


        StartCoroutine(Delay());

        IEnumerator Delay()
        { 
            yield return new WaitForSeconds(5);

            agent.SetDestination(target);
            _CurrentState = CurrentState.Walking;
        }
    }

    void Check()
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            if(agent.transform.position == waypoints[i].position)
            {
                Debug.Log("just landed");

                isIdle = true;
                anim.Play("Turn");
                Debug.Log("just turned");
            }
        }
    }

    //Updated Destination
    void IterateWayPoint()
    {
        i++;
        if(i == waypoints.Length)
        {
            isWalking = false;
            isIdle = true;
            Debug.Log("End Point");
            i = 0;  //restart AI Loop
        }

        

    }

    public void DistBetPlayers()
    {
        Dist = Vector3.Distance(agent.transform.position, player.transform.position);
        if(Dist< minDist)
        {
            //timer starts
            Debug.Log("You are too close!");
        }

         if(Dist> maxDist)
        {
            //timer starts
            Debug.Log("You are too far!");

        }
    }


}

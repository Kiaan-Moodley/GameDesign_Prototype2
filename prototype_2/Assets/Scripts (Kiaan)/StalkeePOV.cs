using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;


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

    public bool timerIsRunning = false;
    public float timeRemaining = 10;
    public float timeFarRemaining = 10;
    float seconds;

    bool tooClose = false;
    bool tooFar = false;
    enum CurrentState
    {
        Idle,
        Walking,
        Looking

    }

    public GameObject TooFarAlert;
    public GameObject TooCloseAlert;
    public TextMeshProUGUI timerFar;
    public TextMeshProUGUI timerClose;

    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        UpdateDestination();
        //_CurrentState = CurrentState.Idle;
        tooClose = false;
        tooFar = false;
    

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
        CheckTimer();
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
            _CurrentState = CurrentState.Walking;

            agent.SetDestination(target);
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
            tooClose = true;
            TooCloseAlert.SetActive(true);
            timerIsRunning = true;
        }
        else
        {
            tooClose = false;
            TooCloseAlert.SetActive(false);
           timerIsRunning = false;
            timeRemaining = 10;

        }

        if (Dist> maxDist)
        {
            //timer starts
            Debug.Log("You are too far!");
            tooFar = true;
            TooFarAlert.SetActive(true);
            timerIsRunning = true;

        }
        else
        {
            tooFar = false;
            TooFarAlert.SetActive(false);
            timerIsRunning = false;
            timeFarRemaining = 10;


        }
    }

    void CheckTimer()
    {
        seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerClose.text = seconds.ToString();
        timerFar.text = seconds.ToString();
        


        if (timerIsRunning && tooClose == true)
        {
            timerClose.gameObject.SetActive(true);
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }

            else if (timeRemaining < 0)
            {
                timerClose.gameObject.SetActive(false);
                //Game over
                Debug.Log("Game over");
                timeRemaining = 0;
                timerIsRunning = false;
            }


            if (tooClose == false)
            {
                timeRemaining = 10;
                timerIsRunning = false;

            }
        }

           else if (timerIsRunning && tooFar == true)
        {
            timerFar.text = seconds.ToString();
            timerFar.gameObject.SetActive(true);
                if (timeFarRemaining > 0)
                {
                    timeFarRemaining -= Time.deltaTime;
                }
                else
                {
                    timerFar.gameObject.SetActive(false);
                    //Game over
                    Debug.Log("Game over");
                    timeFarRemaining = 0;
                    timerIsRunning = false;
                }
                //if (tooFar == false)
                //{
                //    timeFarRemaining = 10;
                //    timerIsRunning = false;

                //}
            }

        }
    }






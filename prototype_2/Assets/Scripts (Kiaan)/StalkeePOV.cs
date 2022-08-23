using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;


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

    float minDist = 3f;
    float maxDist = 20f;

    float Dist;

    [SerializeField]
    private CurrentState _CurrentState;

    public bool timerIsRunning = false;
    public bool timerIsRunningClose = false;
    public float timeRemaining = 10;
    public float timeFarRemaining = 10;
    public Slider slSus;
    float seconds;
    float secondsFar;

    bool tooClose = false;
    bool tooFar = false;
    enum CurrentState
    {
        Idle,
        Walking,
        Looking,
        Eating

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

        else if(_CurrentState == CurrentState.Eating)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isLooking", false);
            anim.SetBool("isEating", true);
        }
    }

    private void Update()
    {
        timerClose.text = seconds.ToString();
        timerFar.text = secondsFar.ToString();
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
        if (Vector3.Distance(transform.position, target) < 1)
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
                    slSus.value += 2f;
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
        for (int i = 0; i < 11; i++)
        {
            if (agent.transform.position == waypoints[i].position)
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
        if (i == waypoints.Length)
        {
            i = 11;

            _CurrentState = CurrentState.Looking;
            _CurrentState = CurrentState.Eating;
            Debug.Log("End Point");
            Debug.Log("Game over. You win!");
            //restart AI Loop
        }



    }

    public void DistBetPlayers()
    {
        Dist = Vector3.Distance(agent.transform.position, player.transform.position);
        if (Dist < minDist)
        {
            //timer starts
            Debug.Log("You are too close!");
            tooClose = true;
            Debug.Log("Tooclose is true");

            timerIsRunningClose = true;
            Debug.Log("Tooclose is true and time is running");

            TooCloseAlert.SetActive(true);
        }
        else
        {
            tooClose = false;
            TooCloseAlert.SetActive(false);
            timerIsRunningClose = false;
            timeRemaining = 10;

        }

        if (Dist > maxDist)
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
        secondsFar = Mathf.FloorToInt(timeFarRemaining % 60);




        if (timerIsRunningClose && tooClose == true)
        {
            Debug.Log("Too close timr is running");
            timerClose.text = seconds.ToString();
            timerClose.gameObject.SetActive(true);
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                timerClose.text = seconds.ToString();

            }

            else
            {
                timerClose.gameObject.SetActive(false);
                //Game over
                Debug.Log("Game over");
                timeRemaining = 0;
                timerIsRunningClose = false;
            }


            //if (tooClose == false)
            //{
            //    timeRemaining = 10;
            //    timerIsRunning = false;

            //}
        }

        else if (timerIsRunning && tooFar == true)
        {
            timerFar.text = seconds.ToString();
            timerFar.gameObject.SetActive(true);
            if (timeFarRemaining > 0)
            {
                timeFarRemaining -= Time.deltaTime;
                timerFar.text = secondsFar.ToString();
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






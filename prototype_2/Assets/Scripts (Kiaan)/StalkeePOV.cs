using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



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
    bool isLooking ;

    float minDist = 3f;
    float maxDist = 20f;
    float susValue = 0;

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
     public instantiateCrowd susBar;

    public AudioSource audioSound;

    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        UpdateDestination();
        _CurrentState = CurrentState.Idle;
        tooClose = false;
        tooFar = false;
        audioSound.Play();
        

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
            isLooking = true;
            Debug.Log("isLooking is true");
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
        Suspicion();
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
            Debug.Log(CurrentState.Looking+ "I am looking");
            //isLooking = true;



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
        isLooking = false;


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
                SceneManager.LoadScene("TooCloseScene");
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
                SceneManager.LoadScene("TooFarScene");

            }
            //if (tooFar == false)
            //{
            //    timeFarRemaining = 10;
            //    timerIsRunning = false;

            //}
        }

       

    }
    public void Suspicion()
    {
        if(susBar.isBlending && isLooking == true)
        {
            float appdist1 = 10;
            Dist = Vector3.Distance(agent.transform.position, player.transform.position);
            if (Dist<=appdist1)
            {
                Debug.Log("isBledning and && isLooking is true");
                susValue += 2;

                slSus.value = susValue;
                Debug.Log(slSus.value);

                susBar.isBlending = false;
                isLooking = false;

            }

        }
        //code not working, slider is filled up completely. Most likely has to do with the update function
        // if(!susBar.isBlending && isLooking == true)
        //{
        //    susBar.isBlending = true;

        //    susValue += 5;

        //    slSus.value = susValue;

        //    isLooking = false;
        //    Debug.Log(slSus.value);
          
        //}

        //******* Also does the same constantly repeating until slider is full*******//
        if (susBar.isCrouching && isLooking == true)
        {
            Debug.Log("isCrouching is true and isLooking is true");
            float appdist = 7;
            Dist = Vector3.Distance(agent.transform.position, player.transform.position);
            // if player is at a certain distance from stalkee
            //if player is further then, it shouldnt matter.
            if (Dist<=appdist)

            {
                Debug.Log("isCrouching and && isLooking is true");
                susValue += 2;

                slSus.value = susValue;
                Debug.Log(slSus.value);

                susBar.isBlending = false;
                isLooking = false;
            }
           

        }

        if(isLooking && !susBar.isBlending && !susBar.isCrouching)
        {
            Debug.Log("isLooking but player is not bledning or crouching");
            Dist = Vector3.Distance(agent.transform.position, player.transform.position);
            float appdist2 = 13;
            if (Dist<appdist2)
            {
                susValue += 5;
                slSus.value = susValue;

            }
                
        }
    }
}






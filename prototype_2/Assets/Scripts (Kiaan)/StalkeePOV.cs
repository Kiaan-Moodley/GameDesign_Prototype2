using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        UpdateDestination();
    }

    private void Update()
    {
        //AI Movement
        if(Vector3.Distance(transform.position,target)<1)
        {
            IterateWayPoint();
            UpdateDestination();
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
        StartCoroutine(Text());
        IEnumerator Text()
        {
            yield return new WaitForSeconds(5);
            target = waypoints[i].position;
            agent.SetDestination(target);
        }
    }

    //Updated Destination
    void IterateWayPoint()
    {
        i++;
        if(i == waypoints.Length)
        {
            Debug.Log("End Point");
            i = 0;  //restart AI Loop
        }
    }
}

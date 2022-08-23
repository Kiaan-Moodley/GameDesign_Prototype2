using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantiateCrowd : MonoBehaviour
{
    public GameObject goPrefab, charC;
    public float startTime = 3f;
    public float iCount = -5;
    public float timer = 0f, timer2 = 0f;
    public bool isTimer = false, isTimer2 = false;
    public CharacterController ccPerson;
    // Start is called before the first frame update
    void Start()
    {
        timer = startTime;
        timer2 = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            for (int i = 0; i < 4; i++)
            {
                Instantiate(goPrefab, new Vector3(iCount, 5, -1.1f), Quaternion.identity);
                Instantiate(goPrefab, new Vector3(iCount + 0.4f, 5, 0f), Quaternion.identity);
                iCount += 0.8f;
                timer2 = startTime;
                isTimer2 = true;
            }

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
             ccPerson.height = 0;
           //ccPerson.transform.position = new Vector3(ccPerson.transform.position.x, 0.3f, ccPerson.transform.position.z);
            timer = startTime;
            isTimer = true;
        }

        if (isTimer == true)
        {
            timer -= 1 * Time.deltaTime;

            if (timer <= 0)
            {
                timer = 0;
            }
            
            if (timer == 0)
            {
                 ccPerson.height = 2f;
               // ccPerson.transform.position = new Vector3(ccPerson.transform.position.x, 0.6444f, ccPerson.transform.position.z);
                isTimer = false;
            }
        }

        if (isTimer2 == true)
        {
            timer2 -= 1 * Time.deltaTime;

            if (timer2 <= 0)
            {
                timer2 = 0;
            }

            if (timer2 == 0)
            {
                GameObject[] taggedgo = GameObject.FindGameObjectsWithTag("crowd");
                foreach(GameObject @object in taggedgo)
                {
                    Destroy(@object);
                }
                isTimer2 = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class instantiateCrowd : MonoBehaviour
{
    public GameObject goPrefab, charC, goStalkee;
    public Slider slSus;
    public float startTime = 3f;
    public float iCount = 0;
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
        if (Input.GetKeyDown(KeyCode.B))
        {
            iCount = 0;
            slSus.value += 2f;
            for (int i = 0; i < 4; i++)
            {
               // Instantiate(goPrefab, new Vector3(iCount, 5, -1.1f), Quaternion.identity);
               // Instantiate(goPrefab, new Vector3(iCount + 0.4f, 5, 0f), Quaternion.identity);
                Instantiate(goPrefab, new Vector3(goStalkee.transform.position.x - 5f, 3, goStalkee.transform.position.z +iCount), Quaternion.identity);
                iCount += 0.9f;
                timer2 = startTime;
                isTimer2 = true;
            }

        }

        if (Input.GetKeyDown(KeyCode.C))
        {
              ccPerson.height = 0;
            //ccPerson.transform.position = new Vector3(ccPerson.transform.position.x, 0.3f, ccPerson.transform.position.z);
            //ccam.transform.position = new Vector3(ccam.transform.position.x, -2.1f, ccam.transform.position.z);
            slSus.value += 2f;
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
                 ccPerson.height = 1.8f;
                //ccam.transform.position = new Vector3(ccam.transform.position.x, -2, ccam.transform.position.z);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantiateCrowd : MonoBehaviour
{
    public GameObject goPrefab;
    public float iCount = -5;
    // Start is called before the first frame update
    void Start()
    {
        
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
            }

        }
    }
}

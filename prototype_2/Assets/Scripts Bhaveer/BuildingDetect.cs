using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDetect : MonoBehaviour
{
    public GameObject goPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            Debug.Log("player hit");
            goPlayer.transform.position = new Vector3(-176.8f, 2f, -35.4f);
        }
        
    }

   
}

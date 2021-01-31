using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportActivator : MonoBehaviour
{
    public Chase maggot;
    public GameObject SpawnPoint;
    public GameObject[] wayPoints;
    void Start()
    {
        maggot = FindObjectOfType<Chase>();

    }

    // Update is called once per frame
    public void MessageAI() {

        if (maggot != null) {
            maggot.Respawn(SpawnPoint.transform.position,wayPoints);
        
        }
        else if (GameManager.maggot != null)
        {
            maggot = GameManager.maggot.GetComponent<Chase>();
            maggot.Respawn(SpawnPoint.transform.position, wayPoints);


        }

    }
}

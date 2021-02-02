using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportActivator : MonoBehaviour
{
    public Chase maggot;
    public GameObject SpawnPoint;
    public GameObject[] wayPoints;
    public GameObject prefab;
    void Start()
    {
        maggot = FindObjectOfType<Chase>();

    }

    // Update is called once per frame
    public void MessageAI() {

        if (maggot != null)
        {
            maggot.Respawn(SpawnPoint.transform.position, wayPoints);

        }
        else if (GameManager.maggot != null)
        {
            maggot = GameManager.maggot.GetComponent<Chase>();
            maggot.Respawn(SpawnPoint.transform.position, wayPoints);


        }
        else {
           GameObject m= Instantiate(prefab, new Vector3(-300,-300,-300), transform.rotation);
            m.GetComponent<Chase>().Respawn(SpawnPoint.transform.position, wayPoints);

        }

    }
}

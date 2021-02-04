using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffGOsTrigger : MonoBehaviour
{
    public List<GameObject> TurnOffGOs = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            for (int i = 0; i < TurnOffGOs.Count; i++)
            {
                TurnOffGOs[i].SetActive(false);
            }
        }
    }
}

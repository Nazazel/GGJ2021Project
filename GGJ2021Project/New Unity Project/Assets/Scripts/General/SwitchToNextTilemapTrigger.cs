using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToNextTilemapTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject DeactivateTilemap, DeactivateEnvironment, ActivateTilemap, ActivateEnvironment;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ActivateTilemap.SetActive(true);
            ActivateEnvironment.SetActive(true);
            DeactivateTilemap.SetActive(false);
            DeactivateEnvironment.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class FlashLight : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.GetComponent<EyeTrap>()) {

            other.gameObject.GetComponent<EyeTrap>().Lit();
        }
      
    }
}

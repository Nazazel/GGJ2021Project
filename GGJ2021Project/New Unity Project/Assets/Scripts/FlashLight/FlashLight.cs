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

            if(!other.gameObject.GetComponent<EyeTrap>().activated)
                other.gameObject.GetComponent<EyeTrap>().Lit();
        }

        if (other.gameObject.GetComponent<Chase>())
        {

            other.gameObject.GetComponent<Chase>().Lit();
        }
        if (other.gameObject.GetComponent<Door>())
        {

            other.gameObject.GetComponent<Door>().Lit();
        }

    }
   
    void OnTriggerExit2D(Collider2D other)
    {

      
        if (other.gameObject.GetComponent<Door>())
        {

            other.gameObject.GetComponent<Door>().Redo();
        }
        if (other.gameObject.GetComponent<EyeTrap>())
        {

            other.gameObject.GetComponent<EyeTrap>().Redo();
        }
        if (other.gameObject.GetComponent<Chase>())
        {

            other.gameObject.GetComponent<Chase>().Redo();
        }

    }
}

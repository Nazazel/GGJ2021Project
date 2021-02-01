using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlightTutorialSign : MonoBehaviour
{
    public GameObject signOne, signTwo;
    float t = 0;
    private void LateUpdate()
    {
        t += Time.deltaTime;
        if (t > 0.9f)
        {
            signOne.SetActive(!signOne.activeSelf);
            signTwo.SetActive(!signTwo.activeSelf);
            t = 0;
        }
    }
}

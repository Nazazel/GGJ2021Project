using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPanner : MonoBehaviour
{
    [SerializeField]
    private float m_Frequency, m_Amplitude;

    Vector3 InitialPos;
    private void Awake()
    {
        InitialPos = transform.position;
    }

    private void LateUpdate()
    {
        transform.position = InitialPos + transform.right * m_Amplitude * Mathf.Sin(2 * Mathf.PI * m_Frequency * Time.time);


    }
}

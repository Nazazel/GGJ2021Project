using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPanner : MonoBehaviour
{
    [SerializeField]
    private float m_Frequency, m_Amplitude, m_MaxTimeSinceSwitch;
    private float m_TimeSinceSwitch = 0;
    private float m_XPosLastUpdate = 0;

    private void LateUpdate()
    {
        transform.position += transform.right * m_Amplitude * Mathf.Sin(2 * Mathf.PI * m_Frequency * Time.time);


    }
}

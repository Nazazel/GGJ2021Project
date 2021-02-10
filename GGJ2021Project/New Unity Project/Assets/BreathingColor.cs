using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BreathingColor : MonoBehaviour
{
    private SpriteRenderer SR;
    private Vector3 InitialScale;
    private Color InitialColor;

    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        InitialScale = transform.localScale;
        InitialColor = SR.color;
    }

    [SerializeField]
    bool ChangeAlpha = false;
    [SerializeField]
    private Vector4 ColorScalarOffset;
    [SerializeField]
    private float ColorSpeed = 1f, Amplitude = 0.1f, ScaleSpeed = 0.1f , ScaleAmp = 0.1f;

    private void Update()
    {
        Color newColor = ColorSineValue();
        SR.color = newColor;
    }

    private void FixedUpdate()
    {
        transform.localScale = ScaleSineValue();
    }

    /// <summary>
    /// pass speed variation of "1" for no change to speed
    /// </summary>
    /// <param name="speedVariation"></param>
    /// <returns></returns>
    private Color ColorSineValue()
    {
        if (ChangeAlpha)
        {
             return new Color(
                 InitialColor.r * ColorScalarOffset.x + Amplitude * Mathf.Sin(2 * Mathf.PI * ColorSpeed * Time.time),
                 InitialColor.g * ColorScalarOffset.y + Amplitude * Mathf.Sin(2 * Mathf.PI * ColorSpeed * Time.time),
                 InitialColor.b * ColorScalarOffset.z + Amplitude * Mathf.Sin(2 * Mathf.PI * ColorSpeed * Time.time),
                 InitialColor.a * ColorScalarOffset.w + Amplitude * Mathf.Sin(2 * Mathf.PI * ColorSpeed * Time.time));      
        }
        else
        {
            return new Color(
                 InitialColor.r * ColorScalarOffset.x + Amplitude * Mathf.Sin(2 * Mathf.PI * ColorSpeed * Time.time),
                 InitialColor.g * ColorScalarOffset.y + Amplitude * Mathf.Sin(2 * Mathf.PI * ColorSpeed * Time.time),
                 InitialColor.b * ColorScalarOffset.z + Amplitude * Mathf.Sin(2 * Mathf.PI * ColorSpeed * Time.time),
                 InitialColor.a);
        }
    }

    private Vector3 ScaleSineValue()
    {
        return new Vector3(
            InitialScale.x + ScaleAmp * Mathf.Sin(2 * Mathf.PI * ScaleSpeed * Time.time),
            InitialScale.y + ScaleAmp * Mathf.Sin(2 * Mathf.PI * ScaleSpeed * Time.time),
            InitialScale.z + ScaleAmp * Mathf.Sin(2 * Mathf.PI * ScaleSpeed * Time.time));
    }
}

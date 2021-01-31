using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    public float totalBattery;
    [SerializeField]
    private float currentBattery;
    public float IncrementAmount;
    public bool open = false;
    public bool active = false;

 
    private Vector3 originPosition;
    public float shake_decay = 0.002f;
    public float shake_intensity = .3f;

    private float temp_shake_intensity = 0;
    public void Redo()
    {
        active = false;
        currentBattery = 0;
        transform.position = originPosition;
    }

    public void Lit() {
        active = true;
        originPosition = transform.position;
        temp_shake_intensity = shake_intensity;


    }

    void Shake()
    {


    }

    private void Update()
    {
        if (active)
        {
            currentBattery += IncrementAmount;
            if (currentBattery >= totalBattery)
            {
                open = true;
                GetComponent<Collider2D>().isTrigger = true;

            }
            transform.position = originPosition + Random.insideUnitSphere * temp_shake_intensity;
        }
       
    }
}

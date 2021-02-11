using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTrap : MonoBehaviour
{
    public bool activated;
    public float cooldown;
    public bool enemy = false;
    public Chase maggot;
    private Animator anim;

    //charge
    public float totalbatteryMaxCharge;
    private float currentBattery;
    public float IncrementAmount;
    public bool open = false;
    public bool active = false;
    private void Awake()
    {
        activated = false;
        anim = GetComponent<Animator>();
    }


    public void Redo()
    {
        if (!open)
        {
            active = false;
            currentBattery = 0;
        }
    }

    public void Lit()
    {
        if (!open)
        {
            active = true;
            
           
        }

    }


    //public void Lit() { Trip(); }
    public void Trip() {
        anim.SetBool("on",true);
        activated = true;
        if (enemy &&maggot!=null) { maggot.Trapped(); }
        
        StartCoroutine(cool());
    }

    private void Update()
    {
        if (active && !open)
        {
            currentBattery += IncrementAmount;
            if (currentBattery >= totalbatteryMaxCharge)
            {
                open = true;
            }
        }
        if (open) { Trip(); }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Chase>()!=null) { enemy = true; maggot = other.gameObject.GetComponent<Chase>(); }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Chase>() != null) { enemy = false; ; maggot = null; }

    }

    IEnumerator cool()
    {
       
        yield return new WaitForSeconds(cooldown);
        activated = false;
        enemy = false;
        open = false;
        anim.SetBool("on", false);



    }
}

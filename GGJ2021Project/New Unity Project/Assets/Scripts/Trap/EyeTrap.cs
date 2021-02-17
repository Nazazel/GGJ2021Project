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
    public float blinkTimer;
    private bool blink;
    private void Awake()
    {
        activated = false;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        blink = true;
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
        open = false;
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
        if (blink) {
            blink = false;
            StartCoroutine(Blink());


        }

    }

    private IEnumerator Blink() {
        yield return new WaitForSeconds(blinkTimer);
        if (!active && !activated)
        { anim.SetBool("blink", true);
            yield return new WaitForSeconds(.5f);
            anim.SetBool("blink", false);
        }
        blink = true;
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
        active = false;
        yield return new WaitForSeconds(cooldown);
        activated = false;
        enemy = false;
        open = false;
        anim.SetBool("on", false);



    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTrap : Lightable
{
    public bool activated;
    public float cooldown;
    public bool enemy = true;
    public Chase maggot;
    private void Start()
    {
        activated = false;
    }



    public void Lit() { Trip(); }
    public void Trip() { 
        activated = true;
        if (enemy) { maggot.Trapped(); }
        StartCoroutine(cool());
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
        

    }
}

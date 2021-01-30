using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTrap : Lightable
{
    public bool activated;

    private void Start()
    {
        activated = false;
    }



    public void Lit() { Trip(); }
    public void Trip() { activated = true; }
}

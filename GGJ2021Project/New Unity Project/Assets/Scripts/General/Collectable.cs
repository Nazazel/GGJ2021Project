﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Chase maggot;

    void Awake()
    {
        maggot = FindObjectOfType<Chase>();

    }

    public void Collect() {
        if (maggot != null)
        {
            maggot.Alert();
        }
        else if (GameManager.maggot != null) {
            maggot = GameManager.maggot.GetComponent<Chase>();
            maggot.Alert();


        }

    }
}

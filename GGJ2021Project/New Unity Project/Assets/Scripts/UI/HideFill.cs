using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideFill : MonoBehaviour
{
    // Start is called before the first frame update
    private Slider timeSlider;
    public Image fill;
    public bool off=false;

    private void Start()
    {
        timeSlider = GetComponent<Slider>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!off) {
            if (timeSlider.value <= 0) { fill.gameObject.SetActive(false); off = true; }
        }
        if (off) {
            if (timeSlider.value > 0) { fill.gameObject.SetActive(true); off = false; }
        }

    }
}

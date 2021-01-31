using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Slider slider;
    public Image[] images;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void setAmount(float score) { slider.value = score; }

    public void counter(int score) {
        for (int i = 0; i < score;i++) {
            images[i].enabled = true;
        }
    
    }

    public void Disable()
    {
        images[0].enabled = false;
        images[1].enabled = false;
        images[2].enabled = false;
    }

}



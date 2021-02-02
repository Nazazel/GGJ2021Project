using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Slider slider;
    public Image[] images;
    public Image death;
    public float respawnTime;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        death.enabled = false;
    }

    public void setAmount(float score) { slider.value = score; }

    public void counter(int score) {
        for (int i = 0; i < score;i++) {
            images[i].enabled = true;
        }
    
    }

    public void Transition() {

        death.enabled = true;

    }
    public void Disable()
    {
        images[0].enabled = false;
        images[1].enabled = false;
        images[2].enabled = false;
    }

    IEnumerator fade() {


        yield return new WaitForSeconds(respawnTime);
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            death.color = new Color(1, 1, 1, i);
            yield return null;
        }
        death.enabled = false;
        death.color= new Color(1, 1, 1, 1);


    }

    public void Die() {
        death.enabled = true;
        StartCoroutine(fade());
    }

}



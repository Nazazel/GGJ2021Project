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
    public Animator anim;
    public Sprite lightedSprite;
    public Sprite openSprite;
    public SpriteRenderer sr;
    private Vector3 originPosition;
    public float shake_decay = 0.002f;
    public float shake_intensity = .3f;

    private float temp_shake_intensity = 0;
    public void Redo()
    {
        if (!open)
        {
            active = false;
            currentBattery = 0;
            transform.position = originPosition;
            anim.enabled = true;
        }
    }

    public void Lit() {
        if (!open)
        {
            active = true;
            sr.sprite = lightedSprite;
            anim.enabled = false;
            originPosition = transform.position;
            temp_shake_intensity = shake_intensity;
        }

    }
    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        if (active &&!open)
        {
            currentBattery += IncrementAmount;
            if (currentBattery >= totalBattery)
            {
                open = true;
                GetComponent<Collider2D>().isTrigger = true;
                sr.sprite = openSprite;
            }
            transform.position = originPosition + Random.insideUnitSphere * temp_shake_intensity;
        }
       
    }
}

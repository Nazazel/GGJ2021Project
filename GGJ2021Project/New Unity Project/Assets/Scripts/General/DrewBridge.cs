using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrewBridge : MonoBehaviour
{
    public HingeJoint2D joint;
    private void Awake()
    {
        joint = GetComponent<HingeJoint2D>();
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Player") { joint.useLimits = true; }
    }
}

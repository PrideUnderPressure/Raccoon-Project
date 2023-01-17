using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giant : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
    }
    void Update()
    {
    }

    public void BeIdle()
    {
        anim.SetBool("isIdle", true);
        anim.SetBool("isTurned", false);
    }
    public void BeTurning()
    {
        anim.SetBool("isTurned", true);
        anim.SetBool("isIdle", false);
    }
}

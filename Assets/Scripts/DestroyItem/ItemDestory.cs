using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestory : MonoBehaviour
{
    private Animator anim;
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (anim.enabled == false)
        {
            anim.enabled = true;
        }
    }
}

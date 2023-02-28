using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadeEq : MonoBehaviour
{
    private bool canOpen;


    private void Update()
    {
        if (canOpen && Input.GetKeyDown(KeyCode.Space))
        {
            canOpen = false;
            gameObject.GetComponent<NPCFunction>().OpenShop();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canOpen = false;
        }
    }
}

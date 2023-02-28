using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    private Vector3 mousePos;
    private Vector3 playerPos;

    private void Awake()
    {
        playerPos = FindObjectOfType<PlayerMovement>().transform.position;
        transform.position = playerPos;
    }

    private void LateUpdate()
    {
        if (PlayerMovement.Instance.DisableInput)
        {
            return;
        }
        PlayerAndMousePos();
        // MaxDistance();
    }

    private void PlayerAndMousePos()
    {
        playerPos = FindObjectOfType<PlayerMovement>().transform.position;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = Vector3.Lerp(transform.position, (mousePos + playerPos) / 2, 0.2f);
    }

    // private void MaxDistance()
    // {
    //     // Debug.Log(Vector3.Distance(playerPos, mousePos));
    //     // Debug.Log(mousePos.normalized);
    //     if (Vector3.Distance(mousePos, play) < Distance)
    //     {
    //     }
    //     else
    //     {
    //         transform.position = (mousePos + playerPos) / 4;
    //
    //     }
    // }
}
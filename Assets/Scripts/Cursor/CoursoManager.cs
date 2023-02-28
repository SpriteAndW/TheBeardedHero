using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoursoManager : MonoBehaviour
{
    public Sprite normal;

    private Sprite currentSprite;

    private Image cursorImage;

    private Canvas cursorCanvas;

    private void Start()
    {
        cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<Canvas>();
        cursorImage = cursorCanvas.GetComponent<RectTransform>().GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        if (cursorCanvas == null)
        {
            return;
        }

        cursorImage.transform.position = Input.mousePosition;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;

public class ItemBounce : MonoBehaviour
{
    private Transform spriteTrans;
    private BoxCollider2D coll;
    public float gravity = -3.5f;
    private bool isGround;
    private float distance;
    private Vector2 direction;
    private Vector3 targetPos;

    private void Awake()
    {
        spriteTrans = transform.GetChild(0);
        coll = GetComponent<BoxCollider2D>();
        coll.enabled = false;
    }

    private void Update()
    {
        Bounce();
    }

    public void InitBounceItem(Vector3 target, Vector2 dir)
    {
        coll.enabled = false;
        direction = dir;
        targetPos = target;

        distance = Vector3.Distance(target, transform.position);
        spriteTrans.position +=Vector3.up*1.5f;
    }
    
    private void Bounce()
    {
        //图片位置Y轴小于等于当前坐标
        isGround = spriteTrans.position.y <= transform.position.y;

        if (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position += (Vector3)direction * distance * -gravity * Time.deltaTime;
        }

        if (!isGround)
        {
            //如果没有图片Y轴没有到达目标位置，那就一直下降，直到到达目标位置
            spriteTrans.position += Vector3.up * gravity * Time.deltaTime;
        }
        else
        {
            spriteTrans.position = transform.position;
            coll.enabled = true;
        }
    }
}

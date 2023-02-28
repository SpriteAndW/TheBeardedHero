using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer sp;
    private SpriteRenderer playerSp;

    private Color color;

    [Header("时间控制参数")] public float activeTime; //显示时间
    public float activeStart; //开始显示的时间点

    [Header("不透明度控制")] private float alpha;
    public float alphaSet;
    public float alphaMultiplier;

    private void OnEnable()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerSp = player.gameObject.GetComponent<SpriteRenderer>();
        sp = GetComponent<SpriteRenderer>();

        alpha = alphaSet;

        sp.sprite = playerSp.sprite;
        
        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;

        activeStart = Time.time;
    }

    private void Update()
    {
        alpha *= alphaMultiplier;

        color = new Color(0.5f, 0.5f, 1, alpha);

        sp.color = color;

        if (Time.time >=activeStart + activeTime)
        {
            //返回对象池
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
}

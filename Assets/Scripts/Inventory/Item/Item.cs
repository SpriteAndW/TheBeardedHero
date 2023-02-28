using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Item : MonoBehaviour
{
    private SpriteRenderer sp;
    private BoxCollider2D coll;

    // public int itemID;
    public InventoryItem item;
    public ItemDetail itemDetail;

    private void Awake()
    {
        sp = GetComponentInChildren<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        if (item.itemID != 0)
        {
            Init(item.itemID,item.itemAmount);
        }
    }

    public void Init(int ID,int amount)
    {
        item.itemID = ID;
        item.itemAmount = amount;
        itemDetail = InventoryManager.Instance.itemDetailData.GetItemDetail(item.itemID);
        
        if (itemDetail != null)
        {
            sp.sprite = itemDetail.itemOnWorldSprite != null ? itemDetail.itemOnWorldSprite : itemDetail.itemIcon;

            //修改碰撞体尺寸
            Vector2 newSize = new Vector2(sp.sprite.bounds.size.x + 0.1f, sp.sprite.bounds.size.y + 0.1f);
            coll.size = newSize;
            coll.offset = new Vector2(sp.sprite.bounds.center.x, sp.sprite.bounds.center.y);
        }
    }
}
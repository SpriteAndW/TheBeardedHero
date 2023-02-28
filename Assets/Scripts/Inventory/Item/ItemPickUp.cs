using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        Item item = col.GetComponent<Item>();

        if (item != null)
        {
            if (item.itemDetail.canPickUp)
            {
                //拾取物品到背包
                InventoryManager.Instance.AddItem(item, true);
            }
        }
    }
}
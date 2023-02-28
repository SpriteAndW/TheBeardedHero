using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerWeapon : MonoBehaviour
{
    public ItemDetail weapon;
    public ItemDetail bullet;

    public int slotIndex = -1;
    


    private void Update()
    {
        if (PlayerMovement.Instance.DisableInput)
        {
            return;
        }
        SwitchWeapon();
    }

    /// <summary>
    /// 根据输入切换武器和弹药
    /// </summary>
    void SwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            bullet = GetEquipmentDetile(ItemType.Bullet, false);
            EventHandler.CallChangeEquipment(bullet);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            bullet = GetEquipmentDetile(ItemType.Bullet, true);
            EventHandler.CallChangeEquipment(bullet);
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            weapon = GetEquipmentDetile(ItemType.Weapon, true);
            EventHandler.CallChangeEquipment(weapon);
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            weapon = GetEquipmentDetile(ItemType.Weapon, false);
            EventHandler.CallChangeEquipment(weapon);
        }
    }


    /// <summary>
    /// 鼠标滚轮控制武器切换，Q、E控制子弹切换（在ActionBar中）
    /// </summary>
    /// <param name="itemType">物品类型</param>
    /// <param name="isAdd">是否往下翻</param>
    /// <returns></returns>
    private ItemDetail GetEquipmentDetile(ItemType itemType, bool isAdd)
    {
        //背包栏没有对应物品的时候，返回空，不然会死循环
        int i = 0;

        if (isAdd)
        {
            if (slotIndex != 9)
            {
                slotIndex++;
            }
            else
            {
                slotIndex = 0;
            }

            while (InventoryManager.Instance.playerBag.itemList[slotIndex].itemAmount == 0 || InventoryManager.Instance
                       .itemDetailData
                       .GetItemDetail(InventoryManager.Instance.playerBag.itemList[slotIndex].itemID).itemType !=
                   itemType)
            {
                slotIndex++;
                if (slotIndex > 9)
                {
                    slotIndex = 0;
                    i++;
                    if (i > 2)
                    {
                        return new ItemDetail();
                    }
                }
            }
        }
        else
        {
            if (slotIndex <= 0)
            {
                slotIndex = 9;
            }
            else
            {
                slotIndex--;
            }

            while (InventoryManager.Instance.playerBag.itemList[slotIndex].itemAmount == 0 || InventoryManager.Instance
                       .itemDetailData
                       .GetItemDetail(InventoryManager.Instance.playerBag.itemList[slotIndex].itemID).itemType !=
                   itemType)
            {
                slotIndex--;
                if (slotIndex <= -1)
                {
                    slotIndex = 9;
                    i++;
                    if (i > 2)
                    {
                        return new ItemDetail();
                    }
                }
            }
        }

        return InventoryManager.Instance.itemDetailData.GetItemDetail(InventoryManager.Instance.playerBag
            .itemList[slotIndex].itemID);
    }
}
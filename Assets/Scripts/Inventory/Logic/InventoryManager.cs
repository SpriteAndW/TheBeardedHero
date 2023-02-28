using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : Singleton<InventoryManager>
{
    [Header("物品数据")] public ItemDetailData_SO itemDetailData;
    [Header("武器数据")] public WeaponData_SO weaponDetailData;
    [Header("子弹数据")] public BulletData_SO BulletDetailData;
    [Header("盔甲数据")] public ArmorData_SO ArmorDetailData;
    [Header("戒指数据")] public RingData_SO RingDetailData;

    [Header("打造装备数据")] public MadeEqData_SO MadeEqData;
    [Header("背包数据")] public InventoryBagData_SO playerBag;

    public InventoryBagData_SO equipmentBag;

    [Header("玩家金币")] public int playerMoney = 300;

    private InventoryBagData_SO currentBoxBag;

    public bool bagIsOpen;

    private void Start()
    {
        EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        EventHandler.CallUpdateInventoryUI(InventoryLocation.Equipment, equipmentBag.itemList);
    }

    private void OnEnable()
    {
        EventHandler.ChangeEquipment += OnChangeEquipment;
        EventHandler.TradeItemEvent += OnTradeItemEvent;
        EventHandler.MadeEquipmentEvent += OnMadeEquipmentEvent;
    }

    private void OnDisable()
    {
        EventHandler.ChangeEquipment -= OnChangeEquipment;
        EventHandler.TradeItemEvent -= OnTradeItemEvent;
        EventHandler.MadeEquipmentEvent -= OnMadeEquipmentEvent;
    }

    private void OnMadeEquipmentEvent(int madeItemID, List<InventoryItem> equipItemList)
    {
        foreach (var item in equipItemList)
        {
            if (!ChackEquipItemInBag(item.itemID, item.itemAmount))
            {
                StartCoroutine(InventoryUI.Instance.tipPanle.DiaPlayTip(TipType.MadeEqFail));
                return;
            }
        }

        //减去背包中打造的材料
        foreach (var item in equipItemList)
        {
            playerBag.itemList[GetItemIndexInBage(item.itemID)].itemAmount -= item.itemAmount;
        }

        var index = GetItemIndexInBage(madeItemID);
        AddItemAtIndex(madeItemID, index, 1);
        StartCoroutine(InventoryUI.Instance.tipPanle.DiaPlayTip(TipType.MadeEqSuccessful));

        EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
    }


    private void OnTradeItemEvent(ItemDetail itemDetail, bool isBuy)
    {
        if (isBuy)
        {
            if (playerMoney >= itemDetail.itemPrice)
            {
                playerMoney -= itemDetail.itemPrice;

                var index = GetItemIndexInBage(itemDetail.itemID);
                AddItemAtIndex(itemDetail.itemID, index, 1);
                StartCoroutine(InventoryUI.Instance.tipPanle.DiaPlayTip(TipType.BuySuccessful));
            }
            else
            {
                StartCoroutine(InventoryUI.Instance.tipPanle.DiaPlayTip(TipType.BuyFail));
            }
        }
        else
        {
            if (itemDetail.itemType!=ItemType.Mission)
            {
                playerMoney += (int)(itemDetail.itemPrice * itemDetail.sellPercentage);

                var index = GetItemIndexInBage(itemDetail.itemID);
                AddItemAtIndex(itemDetail.itemID, index, -1);
                StartCoroutine(InventoryUI.Instance.tipPanle.DiaPlayTip(TipType.SellSuccessful));
            }
            else
            {
                StartCoroutine(InventoryUI.Instance.tipPanle.DiaPlayTip(TipType.SellFail));
            }
        }

        EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
    }

    /// <summary>
    /// 根据装备类型切换对应的数据
    /// </summary>
    /// <param name="equipmentDetail"></param>
    private void OnChangeEquipment(ItemDetail equipmentDetail)
    {
        switch (equipmentDetail.itemType)
        {
            case ItemType.Weapon:
                equipmentBag.itemList[0].itemID = equipmentDetail.itemID;
                EventHandler.CallChangeAttackEquipment();
                break;
            case ItemType.Bullet:
                equipmentBag.itemList[1].itemID = equipmentDetail.itemID;
                EventHandler.CallChangeAttackEquipment();
                break;
            case ItemType.Armor:
                equipmentBag.itemList[2].itemID = equipmentDetail.itemID;
                break;
            case ItemType.Ring:
                equipmentBag.itemList[3].itemID = equipmentDetail.itemID;
                break;
        }
    }


    public void AddItem(Item item, bool isDestory)
    {
        //判断是否有相同物品,可否堆叠
        var index = GetItemIndexInBage(item.item.itemID);
        AddItemAtIndex(item.item.itemID, index, 1);

        Debug.Log((item.itemDetail.itemName));
        if (isDestory)
        {
            Destroy(item.gameObject);
        }

        //更新UI 
        EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
    }


    /// <summary>
    /// 检查背包是否有空位
    /// </summary>
    /// <returns></returns>
    private bool CheckBagCapacity()
    {
        for (int i = 0; i < playerBag.itemList.Count; i++)
        {
            if (playerBag.itemList[i].itemID == 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 通过物品ID找到背包已有物品位置
    /// </summary>
    /// <param name="ID">物品ID </param>
    /// <returns></returns>
    private int GetItemIndexInBage(int ID)
    {
        for (int i = 0; i < playerBag.itemList.Count; i++)
        {
            if (playerBag.itemList[i].itemID == ID)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// 检查背包是否有,且数量达到要求
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="itemNum"></param>
    /// <returns></returns>
    private bool ChackEquipItemInBag(int itemID, int itemNum)
    {
        int index = GetItemIndexInBage(itemID);

        if (index >= 0)
        {
            int num = playerBag.itemList[index].itemAmount;
            if (num >= itemNum)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// 再指定序号位置添加物品
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="index"></param>
    /// <param name="amount"></param>
    private void AddItemAtIndex(int ID, int index, int amount)
    {
        if (index == -1 && CheckBagCapacity())
        {
            var item = new InventoryItem { itemID = ID, itemAmount = amount }; //快速初始化并赋值

            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == 0)
                {
                    playerBag.itemList[i] = item;
                    break;
                }
            }
        }
        else
        {
            int currentAmount = playerBag.itemList[index].itemAmount + amount;
            var item = new InventoryItem { itemID = ID, itemAmount = currentAmount }; //快速初始化并赋值
            playerBag.itemList[index] = item;
        }
    }

    /// <summary>
    /// 玩家背包内物品交换
    /// </summary>
    /// <param name="fromIndex">当前格子序号</param>
    /// <param name="targetIndex">目标格子序号</param>
    public void SwapItem(int fromIndex, int targetIndex)
    {
        InventoryItem currentItem = playerBag.itemList[fromIndex];
        InventoryItem targetItem = playerBag.itemList[targetIndex];

        if (targetItem.itemID != 0 && targetItem.itemID != currentItem.itemID) //如果目标格子有物品,交换双方的物品信息
        {
            playerBag.itemList[fromIndex] = targetItem;
            playerBag.itemList[targetIndex] = currentItem;
        }
        else if (targetItem.itemID == currentItem.itemID)
        {
            playerBag.itemList[targetIndex] = targetItem;
            targetItem.itemAmount += currentItem.itemAmount;
            playerBag.itemList[fromIndex] = new InventoryItem();
        }
        else //如果格子没有物品,当前格子信息到目标格子,初始化当前格子信息
        {
            playerBag.itemList[targetIndex] = currentItem;
            playerBag.itemList[fromIndex] = new InventoryItem();
        }

        //刷新UI
        EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
    }

    /// <summary>
    /// 不同背包交换物品
    /// </summary>
    /// <param name="locationFrom">背包类型起始</param>
    /// <param name="fromIndex">起始序号</param>
    /// <param name="locationTarget">背包类型目标</param>
    /// <param name="targetIndex">目标序号</param>
    public void SwapItem(InventoryLocation locationFrom, int fromIndex, InventoryLocation locationTarget,
        int targetIndex)
    {
        var currentList = GetItemList(locationFrom);
        var targetList = GetItemList(locationTarget);


        InventoryItem currentItem = currentList[fromIndex];

        if (targetIndex < targetList.Count)
        {
            InventoryItem targetItem = targetList[targetIndex];

            if (targetItem.itemID != 0 && targetItem.itemID != currentItem.itemID) //有不相同的两个物品
            {
                if (locationTarget != InventoryLocation.Equipment)
                {
                    currentList[fromIndex] = targetItem;
                    targetList[targetIndex] = currentItem;
                }
                else
                {
                    targetList[targetIndex].itemID = currentItem.itemID;
                }
            }
            else if (targetItem.itemID == currentItem.itemID) //相同两个物品
            {
                targetItem.itemAmount += currentItem.itemAmount;
                targetList[targetIndex] = targetItem;
                if (locationTarget != InventoryLocation.Equipment)
                {
                    currentList[fromIndex] = new InventoryItem(); //清空当前物品数据,初始化
                }
            }
            else //目标空格子
            {
                targetList[targetIndex] = currentItem;
                if (locationTarget != InventoryLocation.Equipment)
                {
                    currentList[fromIndex] = new InventoryItem();
                }
            }

            //更新UI
            EventHandler.CallUpdateInventoryUI(locationFrom, currentList);
            EventHandler.CallUpdateInventoryUI(locationTarget, targetList);
        }
    }

    /// <summary>
    /// 根据背包位置返回背包数据
    /// </summary>
    /// <param name="location">背包位置:Player,Box,Shop</param>
    /// <returns></returns>
    private List<InventoryItem> GetItemList(InventoryLocation location)
    {
        return location switch
        {
            InventoryLocation.Player => playerBag.itemList,
            InventoryLocation.Box => currentBoxBag.itemList,
            InventoryLocation.Equipment => equipmentBag.itemList,
            _ => null,
        };
    }
}
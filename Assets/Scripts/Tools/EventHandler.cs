using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHandler
{
    public static event Action<string, Vector3> TransitionEvent;

    public static void CallTransitionEvent(string sceneName, Vector3 pos)
    {
        TransitionEvent?.Invoke(sceneName, pos);
    }

    public static event Action BeforeSceneUnloadEvent;

    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }

    public static event Action AfterSceneLoadedEvent;

    public static void CallAfterSceneLoadedEvent()
    {
        AfterSceneLoadedEvent?.Invoke();
    }

    public static event Action<Vector3> MoveToPosition;

    public static void CallMoveToPosition(Vector3 targetPosition)
    {
        MoveToPosition?.Invoke(targetPosition);
    }

    public static event Action<DialoguePiece> ShowDialogueEvent;

    public static void CallDialogueEvent(DialoguePiece dialoguePiece)
    {
        ShowDialogueEvent?.Invoke(dialoguePiece);
    }

    public static event Action<GameObject, bool> CameraLookEvent;

    public static void CallCameraLookEvent(GameObject target, bool isTalking)
    {
        CameraLookEvent?.Invoke(target, isTalking);
    }

    public static event Action<InventoryLocation, List<InventoryItem>> UpdateInventoryUI;

    public static void CallUpdateInventoryUI(InventoryLocation inventoryLocation, List<InventoryItem> inventoryItemList)
    {
        UpdateInventoryUI?.Invoke(inventoryLocation, inventoryItemList);
    }

    public static event Action<int, int,Vector3> InstantiateItemInScene;

    public static void CallInstantiateItemInScene(int ID, int num,Vector3 pos)
    {
        InstantiateItemInScene?.Invoke(ID,num, pos);
    }

    /// <summary>
    /// 在显示界面改变武器和子弹属性
    /// </summary>
    public static event Action ChangeAttackEquipment;

    public static void CallChangeAttackEquipment()
    {
        ChangeAttackEquipment?.Invoke();
    }


    /// <summary>
    /// 在数据中修改装备属性
    /// </summary>
    public static event Action<ItemDetail> ChangeEquipment;

    public static void CallChangeEquipment(ItemDetail equipment)
    {
        ChangeEquipment?.Invoke(equipment);
    }
    
    //卸下装备修改属性
    public static event Action<ItemDetail> UnloadEquipment;

    public static void CallUnloadEquipment(ItemDetail equipment)
    {
        UnloadEquipment?.Invoke(equipment);
    }


    /// <summary>
    /// 生成对象池
    /// </summary>
    public static event Action<int> InitBulletPool;

    public static void CallInitBulletPool(int bulletID)
    {
        InitBulletPool?.Invoke(bulletID);
    }


    /// <summary>
    /// 更新血量显示
    /// </summary>
    public static event Action UpdateHealthUI;

    public static void CallUpdateHealthUI()
    {
        UpdateHealthUI?.Invoke();
    }

    /// <summary>
    /// 通用背包打开事件
    /// </summary>
    public static event Action<SlotType, InventoryBagData_SO> BaseBagOpenEvent;

    public static void CallBaseBagOpenEvent(SlotType slotType, InventoryBagData_SO dataSo)
    {
        BaseBagOpenEvent?.Invoke(slotType, dataSo);
    }

    /// <summary>
    /// 通用背包关闭事件
    /// </summary>
    public static event Action<SlotType, InventoryBagData_SO> BaseBagCloseEvent;

    public static void CallBaseBagCloseEvent(SlotType slotType, InventoryBagData_SO bag_SO)
    {
        BaseBagCloseEvent?.Invoke(slotType, bag_SO);
    }

    /// <summary>
    /// 商品买卖事件
    /// </summary>
    public static event Action<ItemDetail, bool> TradeItemEvent;

    public static void CallTradeItemEvent(ItemDetail itemDetail, bool isBuy)
    {
        TradeItemEvent?.Invoke(itemDetail, isBuy);
    }

    /// <summary>
    /// 物品打造事件
    /// </summary>
    public static event Action<int, List<InventoryItem>> MadeEquipmentEvent;

    public static void CallMadeEquipmentEvent(int madeID, List<InventoryItem> materialEquip)
    {
        MadeEquipmentEvent?.Invoke(madeID,materialEquip);
    }


}
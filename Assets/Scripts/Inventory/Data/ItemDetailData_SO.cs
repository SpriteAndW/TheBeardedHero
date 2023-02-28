using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDetailData_SO", menuName = "Inventory/ItemDetailData_SO")]
public class ItemDetailData_SO : ScriptableObject
{
    public List<ItemDetail> ItemDetails;

    public ItemDetail GetItemDetail(int ID)
    {
        return ItemDetails.Find(i => i.itemID == ID);
    }
}

[System.Serializable]
public class ItemDetail
{
    public int itemID;
    public string itemName;
    public ItemType itemType; //物品类型
    public Sprite itemIcon; //物品图标
    public Sprite itemOnWorldSprite; //物品在地图显示图标

    [TextArea] public string itemDescription; //物品描述

    public bool canPickUp; //是否能够捡起

    public int WeaponID;
    public int BulletID;
    public int ArmorID;
    public int RingID;  
    
    public int itemPrice;
    [Range(0f, 1f)] public float sellPercentage;
}


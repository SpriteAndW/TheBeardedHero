using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName ="InventoryBagData_SO",menuName = "Inventory/InventoryBagData_SO")]
public class InventoryBagData_SO : ScriptableObject
{
    public List<InventoryItem> itemList;
}

[System.Serializable]
public class InventoryItem
{
    public int itemID;
    public int itemAmount;
}

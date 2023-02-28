using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItem : MonoBehaviour
{
    [System.Serializable]
    public class LootInventoryItem
    {
        public InventoryItem item;
        [Range(0, 1)] public float lootWeight;
    }

    public List<LootInventoryItem> lootInventoryItems;


    public void SpwnLootItem(Vector3 pos)
    {
        float currentValue = Random.value; //随从产生0-1的随机数

        for (int i = 0; i < lootInventoryItems.Count; i++)
        {
            if (currentValue <= lootInventoryItems[i].lootWeight)
            {
                var spawnPos = new Vector3(transform.position.x + Random.Range(-1,1), transform.position.y + Random.Range(-1,1), 0);
                
                Debug.Log(spawnPos);
                EventHandler.CallInstantiateItemInScene(lootInventoryItems[i].item.itemID,lootInventoryItems[i].item.itemAmount,spawnPos);
            }
        }
    }
}
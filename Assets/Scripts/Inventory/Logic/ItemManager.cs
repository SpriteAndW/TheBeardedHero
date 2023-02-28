using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private Transform itemParent;

    public GameObject itemPrefab;
    public GameObject bounceItemPrefab;

    private void OnEnable()
    {
        EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;

    }

    private void OnAfterSceneLoadedEvent()
    {
        itemParent = GameObject.FindWithTag("ItemParent").transform;
    }

    // private void Awake()
    // {
    //     itemParent = GameObject.FindWithTag("ItemParent").transform;
    // }

    private void OnInstantiateItemInScene(int ID, int num,Vector3 pos)
    {
        var item = Instantiate(bounceItemPrefab, pos, Quaternion.identity,itemParent);
        item.GetComponent<Item>().item.itemID = ID;
        item.GetComponent<Item>().item.itemAmount  = num;
        item.GetComponent<ItemBounce>().InitBounceItem(pos,Vector3.up);
    }
}

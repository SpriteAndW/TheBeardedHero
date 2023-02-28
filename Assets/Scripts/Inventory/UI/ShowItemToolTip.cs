using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SlotUI))]
public class ShowItemToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private SlotUI slotUI => GetComponent<SlotUI>();

    private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slotUI.itemAmount != 0)
        {
            inventoryUI.itemToolTip.gameObject.SetActive(true);
            inventoryUI.itemToolTip.SetupToolTip(slotUI.itemDetail, slotUI.slotType);

            inventoryUI.itemToolTip.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
            inventoryUI.itemToolTip.transform.position = transform.position - new Vector3(120f, -20, 0);
        }
        else
        {
            inventoryUI.itemToolTip.gameObject.SetActive(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryUI.itemToolTip.gameObject.SetActive(false);
    }
}
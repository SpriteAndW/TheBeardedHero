using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFunction : MonoBehaviour
{
    public InventoryBagData_SO ItemData;
    public SlotType slotType;
    private bool isOpen;


    private void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
        }
    }

    public void OpenShop()
    {
        isOpen = true;
        EventHandler.CallBaseBagOpenEvent(slotType, ItemData);
        PlayerMovement.Instance.DisableInput = true;
    }

    public void CloseShop()
    {
        isOpen = false;
        EventHandler.CallBaseBagCloseEvent(slotType, ItemData);
        PlayerMovement.Instance.DisableInput = false;
    }
}
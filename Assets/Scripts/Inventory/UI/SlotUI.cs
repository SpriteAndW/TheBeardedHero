using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class SlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("组件获取")] [SerializeField] private Image slotImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] public Image slotHightlight;
    [SerializeField] private Button button;
    [Header("格子类型")] public SlotType slotType;
    public bool isSelected;
    public int slotIndex;

    private InventoryLocation slotLocation;
    [Header("物品信息")] public ItemDetail itemDetail;
    public int itemAmount;

    private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

    public InventoryLocation Location
    {
        get
        {
            return slotType switch
            {
                SlotType.Bag => InventoryLocation.Player,
                SlotType.Box => InventoryLocation.Box,
                SlotType.Weapon => InventoryLocation.Equipment,
                SlotType.Bullet => InventoryLocation.Equipment,
                SlotType.Armor => InventoryLocation.Equipment,
                SlotType.Ring => InventoryLocation.Equipment,

                _ => InventoryLocation.Player
            };
        }
    }

    private void Start()
    {
        slotLocation = Location;

        isSelected = false;
        if (itemDetail.itemID == 0)
        {
            UpdateEmptySlot();
        }
    }

    /// <summary>
    /// 更新格子中对应的物品信息
    /// </summary>
    /// <param name="item">物品详情</param>
    /// <param name="amount">物品数量</param>
    public void UpdateSlot(ItemDetail item, int amount)
    {
        itemDetail = item;
        slotImage.sprite = item.itemIcon;
        itemAmount = amount;
        slotImage.enabled = true;
        amountText.text = amount.ToString();
        button.interactable = true;

        if (slotLocation == InventoryLocation.Equipment || slotType == SlotType.Shop || slotType == SlotType.MadeEq)
        {
            amountText.enabled = false;
        }

        if (slotType == SlotType.Shop)
        {
            transform.GetChild(3).GetChild(0).GetComponent<Text>().text = item.itemPrice.ToString();
        }


        if (slotType == SlotType.MadeEq)
        {
            var eqDetail = InventoryManager.Instance.MadeEqData.GetMadeEqDetail(item.itemID);
            int num = eqDetail.needItem.Count;

            for (int i = 0; i < num; i++)
            {
                var materialPrefab = Instantiate(InventoryUI.Instance.madeMaterialPrefab, transform.GetChild(3));
                materialPrefab.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = InventoryManager.Instance
                    .itemDetailData.GetItemDetail(eqDetail.needItem[i].itemID).itemIcon;
                materialPrefab.transform.GetChild(2).gameObject.GetComponent<Text>().text =
                    eqDetail.needItem[i].itemAmount.ToString();
            }
        }
    }

    /// <summary>
    /// 将空格子设置为空
    /// </summary>
    public void UpdateEmptySlot()
    {
        if (isSelected)
        {
            isSelected = false;
        }
        
        slotImage.enabled = false;
        amountText.text = String.Empty;
        button.interactable = false;
        itemAmount = 0;
    }

    /// <summary>
    /// 鼠标点击事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemAmount == 0)
        {
            return;
        }

        isSelected = !isSelected;
        inventoryUI.UpdateSlotHightlight(slotIndex);

        if (slotType == SlotType.Shop || slotType == SlotType.MadeEq)
        {
            //双击商品事件
            if (eventData.clickCount % 2 == 0)
            {
                if (slotType == SlotType.Shop)
                {
                    EventHandler.CallTradeItemEvent(itemDetail, true);
                }
                else
                {
                    EventHandler.CallMadeEquipmentEvent(itemDetail.itemID,
                        InventoryManager.Instance.MadeEqData.GetMadeEqDetail(itemDetail.itemID).needItem);
                }
            }
        }
    }

    /// <summary>
    /// 鼠标开始拖拽事件
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemAmount != 0)
        {
            inventoryUI.dragItemImage.enabled = true;
            inventoryUI.dragItemImage.sprite = slotImage.sprite;
            // inventoryUI.dragItemImage.SetNativeSize();   // 图片原始大小

            isSelected = true;
            inventoryUI.UpdateSlotHightlight(slotIndex);
        }
    }

    /// <summary>
    /// 鼠标拖拽过程中事件
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnDrag(PointerEventData eventData)
    {
        inventoryUI.dragItemImage.transform.position = Input.mousePosition;
    }

    /// <summary>
    /// 鼠标结束拖拽事件
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnEndDrag(PointerEventData eventData)
    {
        inventoryUI.dragItemImage.enabled = false;

        if (eventData.pointerCurrentRaycast.gameObject != null) //如果目标不为空
        {
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null) //如果目标不具备SlotUI组件,不执行以下
            {
                return;
            }

            var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
            int targetIndex = targetSlot.slotIndex;

            // Debug.Log(slotIndex+"+"+targetIndex);
            if (slotIndex == targetIndex && slotType == targetSlot.slotType)
            {
                return;
            }

            //背包内普通物品交换
            if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)
            {
                InventoryManager.Instance.SwapItem(slotIndex, targetIndex);
            }
            else if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Shop && itemDetail.itemPrice > 0)
            {
                //贩卖道具
                EventHandler.CallTradeItemEvent(itemDetail, false);
            }
            else if (slotType == SlotType.Shop && targetSlot.slotType == SlotType.Bag)
            {
                //购买道具
                EventHandler.CallTradeItemEvent(itemDetail, true);
            }
            else if (slotType != SlotType.Shop && targetSlot.slotType != SlotType.Shop &&
                     slotType != targetSlot.slotType && slotType != SlotType.Box)
            {
                //跨背包数据交换物品
                //装备切换,只能交换,不能单独卸下
                if (itemDetail.itemType.ToString() == targetSlot.slotType.ToString())
                {
                    if (itemDetail.itemType == ItemType.Weapon || itemDetail.itemType == ItemType.Bullet)
                    {
                        EventHandler.CallChangeAttackEquipment();
                    }
                    else
                    {
                        if (targetSlot.itemDetail.itemID != 0)
                        {
                            EventHandler.CallUnloadEquipment(targetSlot.itemDetail);
                        }

                        EventHandler.CallChangeEquipment(itemDetail);
                    }

                    InventoryManager.Instance.SwapItem(Location, slotIndex, targetSlot.Location, targetIndex);
                }
            }
            // else if (slotType == SlotType.Box && targetSlot.slotType == SlotType.Box)
            // {
            //
            //     InventoryManager.Instance.SwapItem(Location, slotIndex, targetSlot.Location, targetIndex);
            // }

            //高亮显示关闭
            inventoryUI.UpdateSlotHightlight(-1);
        }
        //-----------------丢在地图上----------------------
        // else
        // {
        //     // if (itemDetail.canDropped)
        //     // {
        //     //将鼠标屏幕坐标变为世界坐标
        //     var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
        //         -Camera.main.transform.position.z));
        //     itemAmount -= 1;
        //     EventHandler.CallInstantiateItemInScene(itemDetail.itemID, 1, pos);
        //     // }
        // }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class InventoryUI : Singleton<InventoryUI>
{
    public ItemToolTip itemToolTip;
    public TipPanle tipPanle;
    [Header("拖拽图片")] public Image dragItemImage;

    [Header("玩家背包UI")] [SerializeField] private GameObject bagUI;
    [Header("快捷栏")] [SerializeField] private GameObject actionBarUI;


    [Header("快捷栏和背包格子")] [SerializeField] private SlotUI[] playerSlots;
    [Header("装备栏格子")] [SerializeField] private SlotUI[] EquipmentSlots;

    [Header("通用背包")] [SerializeField] private GameObject baseBag;
    public GameObject shopSlotPrefab;
    public GameObject boxSlotPrefab;
    public GameObject madeEqSlotPrefab;
    public GameObject madeMaterialPrefab;
    public Button closeButton;
    public InventoryBagData_SO shopData;

    [Header("交易UI")] [SerializeField] private List<SlotUI> baseBagSlots;
    public TextMeshProUGUI playerMoneyText;


    [Header("渐变参数")] public float fadeTime = 1f;
    public CanvasGroup BaseBagCanvasGroup;
    public RectTransform BaseBagRectTransform;

    public CanvasGroup BagCanvasGroup;
    public RectTransform BagRectTransform;
    private bool isFirst = true;


    // protected override void Awake()
    // {
    //     base.Awake();
    //     BagCanvasGroup = transform.GetChild(0).GetComponent<CanvasGroup>();
    //     BagRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
    //     
    //     BagCanvasGroup = transform.GetChild(1).GetComponent<CanvasGroup>();
    //     BagRectTransform = transform.GetChild(1).GetComponent<RectTransform>();
    //
    // }

    private void OnEnable()
    {
        EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;
        EventHandler.ChangeEquipment += OnChangeEquipment;
        EventHandler.BaseBagOpenEvent += OnBaseBagOpenEvent;
        EventHandler.BaseBagCloseEvent += OnBaseBagCloseEvent;
    }

    private void OnDisable()
    {
        EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;
        EventHandler.ChangeEquipment -= OnChangeEquipment;
        EventHandler.BaseBagOpenEvent -= OnBaseBagOpenEvent;
        EventHandler.BaseBagCloseEvent -= OnBaseBagCloseEvent;
    }


    private void OnBaseBagCloseEvent(SlotType slotType, InventoryBagData_SO data)
    {
        // baseBag.SetActive(false);    
        OpenBaseBagUI(false);
        itemToolTip.gameObject.SetActive(false);
        UpdateSlotHightlight(-1);

        foreach (var slot in baseBagSlots)
        {
            Destroy(slot.gameObject);
        }

        baseBagSlots.Clear();

        if (slotType == SlotType.Shop || slotType == SlotType.MadeEq)
        {
            OpenBagUI();

            // bagOpened = false;
        }

        PlayerMovement.Instance.DisableInput = false;
    }

    private void OnBaseBagOpenEvent(SlotType slotType, InventoryBagData_SO data)
    {
        // baseBag.SetActive(true);
        OpenBaseBagUI(true);
        GameObject prefab = slotType switch
        {
            SlotType.Shop => shopSlotPrefab,
            SlotType.Box => boxSlotPrefab,
            SlotType.MadeEq => madeEqSlotPrefab,
            _ => null,
        };

        baseBagSlots = new List<SlotUI>();
        for (int i = 0; i < data.itemList.Count; i++)
        {
            var slot = Instantiate(prefab, baseBag.transform.GetChild(0)).GetComponent<SlotUI>();
            slot.slotIndex = i;
            baseBagSlots.Add(slot);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(baseBag.GetComponent<RectTransform>());

        if (slotType == SlotType.Shop || slotType == SlotType.MadeEq)
        {
            OpenBagUI();
            // bagOpened = true;
        }

        PlayerMovement.Instance.DisableInput = true;
        OnUpdateInventoryUI(InventoryLocation.Box, data.itemList);
    }

    /// <summary>
    /// 根据不同装备类型,切换SlotUI显示
    /// </summary>
    /// <param name="equipmentDetail"></param>
    /// <param name="itemType"></param>
    private void OnChangeEquipment(ItemDetail equipmentDetail)
    {
        if (equipmentDetail.itemID != 0)
        {
            switch (equipmentDetail.itemType)
            {
                case ItemType.Weapon:
                    EquipmentSlots[0].itemDetail = equipmentDetail;
                    EquipmentSlots[0].UpdateSlot(equipmentDetail, 1);
                    break;
                case ItemType.Bullet:
                    EquipmentSlots[1].itemDetail = equipmentDetail;
                    EquipmentSlots[1].UpdateSlot(equipmentDetail, 1);
                    break;
                case ItemType.Armor:
                    EquipmentSlots[2].itemDetail = equipmentDetail;
                    EquipmentSlots[2].UpdateSlot(equipmentDetail, 1);
                    break;
                case ItemType.Ring:
                    EquipmentSlots[3].itemDetail = equipmentDetail;
                    EquipmentSlots[3].UpdateSlot(equipmentDetail, 1);
                    break;
            }
        }
    }


    private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
    {
        switch (location)
        {
            case InventoryLocation.Player:
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].itemAmount > 0)
                    {
                        var item = global::InventoryManager.Instance.itemDetailData.GetItemDetail(list[i].itemID);
                        playerSlots[i].UpdateSlot(item, list[i].itemAmount);
                    }
                    else
                    {
                        playerSlots[i].UpdateEmptySlot();
                    }
                }

                break;
            case InventoryLocation.Equipment:
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].itemAmount > 0)
                    {
                        var item = global::InventoryManager.Instance.itemDetailData.GetItemDetail(list[i].itemID);

                        EquipmentSlots[i].UpdateSlot(item, list[i].itemAmount);


                        if (i > 2)
                        {
                            if (EquipmentSlots[i].itemDetail.itemID != 0 && isFirst)
                            {
                                EventHandler.CallChangeEquipment(EquipmentSlots[i].itemDetail);
                            }
                        }
                    }
                    else
                    {
                        EquipmentSlots[i].UpdateEmptySlot();
                    }
                }

                isFirst = false;

                break;
            case InventoryLocation.Box:
                for (int i = 0; i < baseBagSlots.Count; i++)
                {
                    if (list[i].itemAmount > 0)
                    {
                        var item = InventoryManager.Instance.itemDetailData.GetItemDetail(list[i].itemID);
                        baseBagSlots[i].UpdateSlot(item, list[i].itemAmount);
                    }
                    else
                    {
                        baseBagSlots[i].UpdateEmptySlot();
                    }
                }

                break;
        }

        playerMoneyText.text = InventoryManager.Instance.playerMoney.ToString();
    }


    private void Start()
    {
        for (int i = 0; i < playerSlots.Length; i++)
        {
            playerSlots[i].slotIndex = i;
        }

        for (int i = 0; i < EquipmentSlots.Length; i++)
        {
            EquipmentSlots[i].slotIndex = i;
        }

        playerMoneyText.text = InventoryManager.Instance.playerMoney.ToString();

        InventoryManager.Instance.bagIsOpen = bagUI.activeInHierarchy;
    }

    private void Update()
    {
        if (PlayerMovement.Instance.DisableInput)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenBagUI();
        }
    }


    /// <summary>
    /// 控制背包淡入淡出
    /// </summary>
    private void OpenBagUI()
    {
        if (!InventoryManager.Instance.bagIsOpen)
        {
            InventoryManager.Instance.bagIsOpen = !InventoryManager.Instance.bagIsOpen;
            PanelFadeIn();

            StartCoroutine(ItemSAnimation());
            //更新UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,
                global::InventoryManager.Instance.playerBag.itemList);
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Equipment,
                global::InventoryManager.Instance.equipmentBag.itemList);
        }
        else
        {
            InventoryManager.Instance.bagIsOpen = !InventoryManager.Instance.bagIsOpen;
            PanelFadeOut();
        }
    }

    private void OpenBaseBagUI(bool isOpen)
    {
        if (isOpen)
        {
            baseBag.SetActive(true);
            BaseBagCanvasGroup.alpha = 0f;
            BaseBagRectTransform.transform.localPosition = new Vector3(1000f, 20f, 0f);
            BaseBagRectTransform.DOAnchorPos(new Vector2(200f, 20f), fadeTime, false).SetEase(Ease.OutElastic);
            BaseBagCanvasGroup.DOFade(1, fadeTime);
        }
        else
        {
            // baseBag.SetActive(false);
            BaseBagCanvasGroup.alpha = 1f;
            BaseBagRectTransform.transform.localPosition = new Vector3(200f, 20f, 0f);
            BaseBagRectTransform.DOAnchorPos(new Vector2(1000f, 20f), fadeTime, false).SetEase(Ease.InOutQuint);
            BaseBagCanvasGroup.DOFade(0, fadeTime);
        }
    }

    /// <summary>
    /// 更新物品格子高亮显示
    /// </summary>
    /// <param name="index"></param>
    public void UpdateSlotHightlight(int index)
    {
        foreach (var slot in playerSlots)
        {
            if (slot.isSelected && slot.slotIndex == index)
            {
                slot.slotHightlight.gameObject.SetActive(true);
            }
            else
            {
                slot.isSelected = false;
                slot.slotHightlight.gameObject.SetActive(false);
            }
        }
    }

    public void CloseBaseBag()
    {
        EventHandler.CallBaseBagCloseEvent(SlotType.Shop, shopData);
        // EventHandler.CallCloseTradeUI();
    }

    /// <summary>
    /// 背包淡入
    /// </summary>
    private void PanelFadeIn()
    {
        bagUI.SetActive(true);

        BagCanvasGroup.alpha = 0f;
        BagRectTransform.transform.localPosition = new Vector3(0f, -1000f, 0f);
        BagRectTransform.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false).SetEase(Ease.OutElastic);
        actionBarUI.transform.DOMoveY(700f, 0.8f).SetEase(Ease.OutElastic);
        BagCanvasGroup.DOFade(1, fadeTime);
    }

    /// <summary>
    /// 背包淡出
    /// </summary>
    private void PanelFadeOut()
    {
        BagCanvasGroup.alpha = 1f;
        BagRectTransform.transform.localPosition = new Vector3(0f, -0f, 0f);
        BagRectTransform.DOAnchorPos(new Vector2(0f, -1000f), fadeTime, false).SetEase(Ease.InOutQuint);
        actionBarUI.transform.DOMoveY(70f, 0.5f).SetEase(Ease.InOutQuint);

        BagCanvasGroup.DOFade(0, fadeTime);
    }

    /// <summary>
    /// 物品格子动画
    /// </summary>
    /// <returns></returns>
    private IEnumerator ItemSAnimation()
    {
        for (int i = 10; i < playerSlots.Length; i++)
        {
            playerSlots[i].transform.localScale = Vector3.zero;
        }

        for (int i = 10; i < playerSlots.Length; i++)
        {
            playerSlots[i].transform.DOScale(1f, fadeTime).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
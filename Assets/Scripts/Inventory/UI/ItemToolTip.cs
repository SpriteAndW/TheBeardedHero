using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemToolTip : MonoBehaviour
{
    [Header("基本组件")] [SerializeField] private TextMeshProUGUI topName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI description;

    [Header("附加组件")] [SerializeField] private GameObject otherItemDetail;
    [SerializeField] private TextMeshProUGUI top;
    [SerializeField] private TextMeshProUGUI mid;
    [SerializeField] private TextMeshProUGUI btm;

    [Header("贩卖组件")] [SerializeField] private GameObject itemCoin;
    [SerializeField] private Text value;

    public void SetupToolTip(ItemDetail itemDetail, SlotType slotType)
    {
        topName.text = itemDetail.itemName;
        itemType.text = GetItemType(itemDetail.itemType);
        description.text = itemDetail.itemDescription;

        if (itemDetail.itemType == ItemType.Armor || itemDetail.itemType == ItemType.Bullet ||
            itemDetail.itemType == ItemType.Weapon || itemDetail.itemType == ItemType.Ring)
        {
            SwitchOtherTip(itemDetail);
        }
        else
        {
            otherItemDetail.SetActive(false);
        }

        if (itemDetail.itemType != ItemType.Mission)
        {
            itemCoin.SetActive(true);
            var price = itemDetail.itemPrice;

            if (slotType != SlotType.Shop)
            {
                price = (int)(price * itemDetail.sellPercentage);
            }

            value.text = price.ToString();
        }
        else
        {
            itemCoin.SetActive(false);
        }

        //让UI重新布局
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    /// <summary>
    /// 根据不同物品类型显示不同信息
    /// </summary>
    /// <param name="itemDetail"></param>
    private void SwitchOtherTip(ItemDetail itemDetail)
    {
        WeaponDetail weaponDetail = InventoryManager.Instance.weaponDetailData.GetWeaponDetail(itemDetail.WeaponID);
        BulletDetail bulletDetail = InventoryManager.Instance.BulletDetailData.GetBulletDetil(itemDetail.BulletID);
        ArmorDetail armorDetail = InventoryManager.Instance.ArmorDetailData.GetArmorDetail(itemDetail.ArmorID);
        RingDetail ringDetail = InventoryManager.Instance.RingDetailData.GetRingDetail(itemDetail.RingID);

        //TODO:其他物品类型
        switch (itemDetail.itemType)
        {
            case ItemType.Weapon:
                otherItemDetail.SetActive(true);
                top.text = "攻击速度:";
                mid.text = "偏移量:";
                btm.text = "发射数量:";
                top.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                    weaponDetail.timeCool.ToString();
                mid.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                    weaponDetail.angle.ToString();
                btm.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                    weaponDetail.bulletNum.ToString();
                break;
            case ItemType.Bullet:
                otherItemDetail.SetActive(true);
                top.text = "子弹速度:";
                mid.text = "存在时间:";
                btm.text = "伤害值:";
                top.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                    bulletDetail.bulletSpeed.ToString();
                mid.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                    bulletDetail.bulletTime.ToString();
                btm.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                    bulletDetail.minDamage.ToString() + " ~ " + bulletDetail.maxDamage.ToString();
                break;
            case ItemType.Armor:
                otherItemDetail.SetActive(true);
                top.text = "防御值:";
                mid.text = "招募数:";
                btm.text = "魅力值:";
                top.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                    armorDetail.defense.ToString();
                mid.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                    armorDetail.recruitCount.ToString();
                btm.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                    armorDetail.charm.ToString();
                break;
            case  ItemType.Ring:
                switch (ringDetail.ringtype)
                {
                    case RingType.simple:
                        otherItemDetail.SetActive(true);
                        top.text = "攻击变化:";
                        mid.text = "防御变化:";
                        btm.text = "速度变化:";
                        top.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                            ringDetail.attack.ToString();
                        mid.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                            ringDetail.defance.ToString();
                        btm.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                            ringDetail.speed.ToString();
                        break;
                    case RingType.other:
                        otherItemDetail.SetActive(true);
                        top.text = "伤害倍数:";
                        mid.text = "受伤倍数:";
                        btm.text = "生命变化:";
                        top.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                            ringDetail.damage.ToString();
                        mid.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                            ringDetail.hurtCount.ToString();
                        btm.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                            ringDetail.HealthChange.ToString();
                        break;
                }
                break;;
                
        }
    }

    private string GetItemType(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Armor => "盔甲",
            ItemType.Bullet => "发射物",
            ItemType.Mission => "任务道具",
            ItemType.Weapon => "武器",
            ItemType.Material => "材料",
            ItemType.Ring => "戒指",
            _ => "无",
        };
    }
}
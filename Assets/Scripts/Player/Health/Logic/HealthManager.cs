using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : Singleton<HealthManager>
{
    [Header("玩家初始属性")] public float maxHealth;
    public float currentHealth;
    private float moveSpeed = 5;
    private float defense = 5;
    private int recruitCount = 1; //招募人数最大值
    private int charm = 10; //人物魅力，招募人时需要
    private int attack = 0;
    private float damageCount = 1;
    private float hurtCount = 1;

    [Header("玩家变化属性")] public float currentMoveSpeed;
    public float currentDefense;
    //TODO:完成招募系统,NPC跟随玩家战斗
    public int currentRecruitCount; //招募人数最大值
    public int currentCharm; //人物魅力，招募人时需要
    public int currentAttack;
    public float currentDamageCount;
    public float currentHurtCount;
    public float hurtLength = 1; //受伤无敌时间
    public int dashCount; //冲刺个数
    public int bombCount; //炸弹个数

    protected override void Awake()
    {
        base.Awake();
        currentHealth = maxHealth;
        currentMoveSpeed = moveSpeed;
        currentDefense = defense;
        currentRecruitCount = recruitCount;
        currentCharm = charm;
        currentAttack = attack;
        currentDamageCount = damageCount;
        currentHurtCount = hurtCount;
    }


    private void OnEnable()
    {
        EventHandler.ChangeEquipment += OnChangeEquipment;
        EventHandler.UnloadEquipment += OnUnloadEquipment;
    }

    private void OnDisable()
    {
        EventHandler.ChangeEquipment -= OnChangeEquipment;
        EventHandler.UnloadEquipment -= OnUnloadEquipment;
    }

    private void OnUnloadEquipment(ItemDetail itemDetail)
    {
        switch (itemDetail.itemType)
        {
            case ItemType.Armor:
                ArmorDetail armorDetail = InventoryManager.Instance.ArmorDetailData.GetArmorDetail(itemDetail.ArmorID);

                currentDefense -= armorDetail.defense;
                currentRecruitCount -= armorDetail.recruitCount;
                currentCharm -= armorDetail.charm;
                break;
            case ItemType.Ring:

                RingDetail ringDetail = InventoryManager.Instance.RingDetailData.GetRingDetail(itemDetail.RingID);

                switch (ringDetail.ringtype)
                {
                    case RingType.simple:
                        currentAttack -= ringDetail.attack;
                        currentDefense -= ringDetail.defance;
                        currentMoveSpeed -= ringDetail.speed;
                        break;
                    case RingType.other:
                        currentDamageCount -= ringDetail.damage;
                        currentHurtCount -= ringDetail.hurtCount;
                        maxHealth -= ringDetail.HealthChange;
                        break;
                }

                break;
        }
    }

    private void OnChangeEquipment(ItemDetail itemDetail)
    {
        switch (itemDetail.itemType)
        {
            case ItemType.Armor:

                ArmorDetail armorDetail = InventoryManager.Instance.ArmorDetailData.GetArmorDetail(itemDetail.ArmorID);

                currentDefense += armorDetail.defense;
                currentRecruitCount += armorDetail.recruitCount;
                currentCharm += armorDetail.charm;
                break;
            case ItemType.Ring:

                RingDetail ringDetail = InventoryManager.Instance.RingDetailData.GetRingDetail(itemDetail.RingID);

                switch (ringDetail.ringtype)
                {
                    case RingType.simple:
                        currentAttack += ringDetail.attack;
                        currentDefense += ringDetail.defance;
                        currentMoveSpeed += ringDetail.speed;
                        break;
                    case RingType.other:
                        currentDamageCount += ringDetail.damage;
                        currentHurtCount += ringDetail.hurtCount;
                        maxHealth += ringDetail.HealthChange;
                        break;
                }

                break;
        }

        EventHandler.CallUpdateHealthUI();
    }
}
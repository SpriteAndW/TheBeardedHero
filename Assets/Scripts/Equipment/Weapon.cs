using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Bullet bulletPrefab;
    // [Header("武器参数")] public int itemIDWeapon;
    public ItemDetail itemDetailWeapon;
    public WeaponDetail weaponDetail;

    // [Header("子弹参数")] public int itemIDBullet;
    public ItemDetail itemDetailBullet;
    public BulletDetail bulletDetail;

    private float timer;
    private GameObject player;
    private Vector2 mousePos;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = transform.parent.gameObject;
    }

    private void OnEnable()
    {
        EventHandler.ChangeAttackEquipment += OnChangeAttackEquipment;
    }

    private void OnDisable()
    {
        EventHandler.ChangeAttackEquipment -= OnChangeAttackEquipment;
    }

    private void Start()
    {
        OnChangeAttackEquipment();
    }

    /// <summary>
    /// 武器弹药属性显示切换
    /// </summary>
    private void OnChangeAttackEquipment()
    {
        if (InventoryManager.Instance.equipmentBag.itemList[0].itemID == 0 ||
            InventoryManager.Instance.equipmentBag.itemList[1].itemID == 0)
        {
            return;
        }
        // Debug.Log("Weapon   +"+InventoryManager.Instance.equipmentBag.itemList[0].itemID);

        itemDetailWeapon =
            InventoryManager.Instance.itemDetailData.GetItemDetail(InventoryManager.Instance.equipmentBag.itemList[0]
                .itemID);
        weaponDetail = InventoryManager.Instance.weaponDetailData.GetWeaponDetail(itemDetailWeapon.WeaponID);

        itemDetailBullet =
            InventoryManager.Instance.itemDetailData.GetItemDetail(InventoryManager.Instance.equipmentBag.itemList[1]
                .itemID);
        bulletDetail = InventoryManager.Instance.BulletDetailData.GetBulletDetil(itemDetailBullet.BulletID);

        spriteRenderer.sprite = itemDetailWeapon.itemOnWorldSprite;
        bulletPrefab.Init(bulletDetail.bulletID);
        
    }

    private void Update()
    {
        if (player.GetComponent<PlayerMovement>().DisableInput)
        {
            return;
        }
        WeaponLookCurse();
    }

    private void WeaponLookCurse()
    {
        mousePos = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized; //获取鼠标与人物坐标
        float rotZ = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg; //将数字转换为角度
        //由于图片是pivot是在右下角，所以要加上角度偏移
        float add;
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
        {
            add = -45;
            spriteRenderer.flipX = true;
        }
        else
        {
            add = -135;
            spriteRenderer.flipX = false;
        }

        transform.rotation = Quaternion.Euler(0, 0, rotZ + add);

        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
            }
        }

        if ((Input.GetButton("Fire1") || Input.GetButtonDown("Fire1")) && !InventoryManager.Instance.bagIsOpen &&
            InventoryManager.Instance.equipmentBag.itemList[0].itemID != 0 &&
            InventoryManager.Instance.equipmentBag.itemList[1].itemID != 0)
        {
            if (timer == 0)
            {
                Fire();

                timer = weaponDetail.timeCool;
            }
        }
    }

    private void Fire()
    {
        ScreenImpuse();
        
        int median = weaponDetail.bulletNum / 2;
        for (int i = 0; i < weaponDetail.bulletNum; i++)
        {
            bulletPrefab.name = bulletDetail.bulletID.ToString();
            GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab.gameObject, transform.position);
            bullet.name = bulletDetail.bulletID.ToString();

            bullet.GetComponent<Bullet>().bulletID = bulletDetail.bulletID;
            bullet.transform.position = transform.position;

            float axis = Random.Range(-weaponDetail.angle, weaponDetail.angle);
            if (weaponDetail.bulletNum % 2 == 1)
            {
                bullet.GetComponent<Bullet>()
                    .SetSpeed(Quaternion.AngleAxis(axis * (i - median), Vector3.forward) * mousePos,bulletDetail);
            }
            else
            {
                bullet.GetComponent<Bullet>()
                    .SetSpeed(Quaternion.AngleAxis(axis * (i - median) + axis / 2, Vector3.forward) * mousePos,bulletDetail);
            }
        }
    }

    //TODO:可优化写到Player
    private void ScreenImpuse()
    {
        player.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
    }
}
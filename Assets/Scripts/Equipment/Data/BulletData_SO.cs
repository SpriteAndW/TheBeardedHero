using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BulletData_SO", menuName = "Equipment/BulletData")]
public class BulletData_SO : ScriptableObject
{
    public List<BulletDetail> BulletDetils;

    public BulletDetail GetBulletDetil(int ID)
    {
        return BulletDetils.Find(i => i.bulletID == ID);
    }
}

[System.Serializable]
public class BulletDetail
{
    public int bulletID;
    public BulletType bulletType = BulletType.Player;
    public Sprite bulletSprite; //子弹图片
    public float bulletSpeed;
    public float bulletTime;
    public float minDamage;
    public float maxDamage;
}
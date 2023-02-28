using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WeaponData_SO", menuName = "Equipment/WeaponData")]
public class WeaponData_SO : ScriptableObject
{
    public List<WeaponDetail> WeaponDetails;

    public WeaponDetail GetWeaponDetail(int ID)
    {
        return WeaponDetails.Find(i => i.weaponID == ID);
    }
}

[System.Serializable]
public class WeaponDetail
{
    public int weaponID;
    public float timeCool; //攻击速度，越低越快
    public float angle; //攻击偏移，越低越好
    public int bulletNum; //子弹数量
}
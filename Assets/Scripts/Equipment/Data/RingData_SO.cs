using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RingData_SO", menuName = "Equipment/RingData_SO")]
public class RingData_SO : ScriptableObject
{
    public List<RingDetail> RingDetails;

    public RingDetail GetRingDetail(int ID)
    {
        return RingDetails.Find(a => a.RingID == ID);
    }
}


[System.Serializable]
public class RingDetail
{
    public int RingID;
    public RingType ringtype;

    [Header("普通类型戒指")] public int attack;
    public int defance;
    public int speed;

    [Header("其他类型戒指")] public float damage;

    public float hurtCount;
    public float HealthChange;
}
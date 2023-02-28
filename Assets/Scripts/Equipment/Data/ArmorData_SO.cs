using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArmorData_SO", menuName = "Equipment/ArmorData")]
public class ArmorData_SO : ScriptableObject
{
    public List<ArmorDetail> ArmorDetails;

    public ArmorDetail GetArmorDetail(int ArmorID)
    {
        return ArmorDetails.Find(a => a.ArmorID == ArmorID);
    }
}

[System.Serializable]
public class ArmorDetail
{
    public int ArmorID;
    public int defense; //防御
    public int recruitCount; //招募人数限制（影响招募人的个数）
    public int charm; //人物魅力（影响可否招募人）
}
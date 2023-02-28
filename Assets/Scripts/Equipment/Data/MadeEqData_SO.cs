using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MadeEqData_SO", menuName = "Equipment/MadeEqData_SO")]
public class MadeEqData_SO : ScriptableObject
{
    public List<MadeEqDetail> MadeEqDetailsList;


    public MadeEqDetail GetMadeEqDetail(int ID)
    {
        return MadeEqDetailsList.Find(i => i.eqID == ID);
    }
}

[System.Serializable]
public class MadeEqDetail
{
    public int eqID;
    public List<InventoryItem> needItem;
}
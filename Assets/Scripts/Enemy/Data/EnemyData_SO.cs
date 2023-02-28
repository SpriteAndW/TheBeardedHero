using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EnemyData_SO", menuName = "Enemy/EnemyData_SO")]
public class EnemyData_SO : ScriptableObject
{
    public List<EnemyDetails> enemyDetails;

    public EnemyDetails GetEnemyDetails(int EnemyID)
    {
        return enemyDetails.Find(i => i.enemyID == EnemyID);
    }
}

[System.Serializable]
public class EnemyDetails
{
    public int enemyID;
    public string enemyName;
    public float maxHp;
    public float currentHp;
    public float moveSpeed;
    public float distance; //警戒范围
    public float attackDistance;  //攻击范围
    public float attackCool; //发射子弹冷却

    public float minDamage;
    public float maxDamage;
}
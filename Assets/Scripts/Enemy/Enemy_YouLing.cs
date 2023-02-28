using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


/// <summary>
/// 幽灵怪物
/// 攻击方式：在玩家进入警戒范围时，隐身（sprite.alph渐变为0,collider关闭）,
///         在以玩家为中心点的距离内,随机现身(sprite.alph渐变为1,collider打开),
///         发射子弹,等待几秒后,再次隐身,攻击冷却完成后,再次出现攻击玩家
/// </summary>
public class Enemy_YouLing : Enemy
{
    private bool isFoundPlayer;


    private Vector3 startingPosition;
    private Vector3 roamPosition;
    [SerializeField] private float timer;
    public float coolTime = 3;
    public float disTime = 2f;
    [SerializeField] private int bulletNum = 3;

    public int bulletID;
    public BulletDetail bulletDetail;

    protected override void Start()
    {
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
        target = GameObject.FindWithTag("Player").transform;
        bulletDetail = InventoryManager.Instance.BulletDetailData.GetBulletDetil(bulletID);
    }

    protected override void Update()
    {
        base.Update();
        YouLingActive();
        CollEnable();
    }

    private void YouLingActive()
    {
        if (enemyState == EnemyState.NoFind)
        {
            if (Vector2.Distance(transform.position, target.position) < enemyDetail.distance)
            {
                enemyState = EnemyState.floowPlayer;
            }

            MoveRandomPosition();
        }
        else if (enemyState == EnemyState.floowPlayer)
        {
            //变透明
            sp.DOColor(new Color(1, 1, 1, 0), disTime);
            if (sp.color.a <= 0.01f)
            {
                enemyState = EnemyState.canAttack;
            }
        }
        else
        {
            MoveToTarget(target.transform.position);
        }
    }


    private void CollEnable()
    {
        if (sp.color.a > 0.8f)
        {
            coll.enabled = true;
            // UIBar.SetActive(true);
            ShadowSp.SetActive(true);
        }
        else
        {
            coll.enabled = false;
            UIBar.SetActive(false);
            ShadowSp.SetActive(false);
        }
    }

    private void MoveToTarget(Vector3 target)
    {
        if (timer <= 0)
        {
            if (sp.color.a == 0)
            {
                Vector3 pos =
                    new Vector3(
                        Random.Range(target.x - enemyDetail.attackDistance, target.x + enemyDetail.attackDistance),
                        Random.Range(target.y - enemyDetail.attackDistance, target.y + enemyDetail.attackDistance));
                transform.position = pos;
                rb.velocity = Vector3.zero;
            }

            if (transform.position.x < target.x)
            {
                sp.flipX = true;
            }
            else
            {
                sp.flipX = false;
            }

            sp.DOColor(new Color(1, 1, 1, 1), disTime);
            if (sp.color.a >= 0.9f)
            {
                anim.SetTrigger("Attack");
                enemyState = EnemyState.floowPlayer;
            }

            timer = coolTime;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }


    /// <summary>
    /// 随机再地图上走来走去
    /// </summary>
    private void MoveRandomPosition()
    {
        transform.position =
            Vector3.MoveTowards(transform.position, roamPosition, Time.deltaTime * enemyDetail.moveSpeed);

        if (Vector3.Distance(transform.position, roamPosition) < 0.1f || timer <= 0)
        {
            if (timer <= 0)
            {
                roamPosition = GetRoamingPosition();
                if (transform.position.x < roamPosition.x)
                {
                    sp.flipX = true;
                }
                else
                {
                    sp.flipX = false;
                }

                timer = coolTime;
                anim.SetBool("IsMoving", true);
            }
            else
            {
                anim.SetBool("IsMoving", false);

                timer -= Time.deltaTime;
            }
        }
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPosition + GetRandomDir() * Random.Range(10f, 10f);
    }


    private static Vector3 GetRandomDir()
    {
        return new Vector3(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
    }

    private void AttackPlayer()
    {
        Vector2 dir = (target.position - transform.position).normalized;

        int median = bulletNum / 2;

        for (int i = 0; i < bulletNum; i++)
        {
            EnemyManager.Instance.bulletPrefab.name = bulletDetail.bulletID.ToString();
            GameObject bullet =
                ObjectPool.Instance.GetObject(EnemyManager.Instance.bulletPrefab.gameObject, transform.position);
            bullet.name = bulletDetail.bulletID.ToString();

            bullet.GetComponent<Bullet>().bulletID = bulletDetail.bulletID;
            bullet.transform.position = transform.position;

            float axis = bulletNum * 5;

            if (bulletNum % 2 == 1)
            {
                bullet.GetComponent<Bullet>()
                    .SetSpeed(Quaternion.AngleAxis(axis * (i - median), Vector3.forward) * dir,
                        bulletDetail);
            }
            else
            {
                bullet.GetComponent<Bullet>()
                    .SetSpeed(Quaternion.AngleAxis(axis * (i - median) + axis / 2, Vector3.forward) * dir,
                        bulletDetail);
            }
        }
    }
}
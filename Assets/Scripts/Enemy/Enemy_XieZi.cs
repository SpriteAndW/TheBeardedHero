using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_XieZi : Enemy
{
    private bool isFoundPlayer;
    private Vector3 startingPosition;
    private Vector3 roamPosition;
    private float timer;
    private float coolTime = 3;

    private float attackTimer;
    [SerializeField] private int bulletNum = 3;

    public int bulletID;
    public BulletDetail bulletDetail;


    protected override void Start()
    {
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition(startingPosition);
        target = GameObject.FindWithTag("Player").transform;
        bulletDetail = InventoryManager.Instance.BulletDetailData.GetBulletDetil(bulletID);
    }

    protected override void Update()
    {
        base.Update();
        XieZiActive();
    }

    protected override  void FixedUpdate()
    {
        rb.velocity = new Vector2(enemyDetail.moveSpeed * moveX, enemyDetail.moveSpeed * moveY);
    }

    private void XieZiActive()
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
            if (Vector2.Distance(transform.position, target.position) > 2 * enemyDetail.distance)
            {
                enemyState = EnemyState.NoFind;
                roamPosition = transform.position;
            }

            if (Vector2.Distance(transform.position, target.position) < enemyDetail.attackDistance)
            {
                enemyState = EnemyState.canAttack;
            }

            MoveToTarget(transform.position, target.position);

            

            anim.SetBool("IsMoving", true);

        }
        else
        {
            if (Vector2.Distance(transform.position, target.position) > 2 * enemyDetail.distance)
            {
                enemyState = EnemyState.NoFind;
                roamPosition = transform.position;
            }
            else if (Vector2.Distance(transform.position, target.position) < 2 * enemyDetail.distance &&
                     Vector2.Distance(transform.position, target.position) > enemyDetail.attackDistance)
            {
                enemyState = EnemyState.floowPlayer;
            }
            else
            {   
                anim.SetBool("IsMoving", false);

                if (attackTimer <= 0)
                {
                    moveX = moveY = 0;
                    attackTimer = enemyDetail.attackCool;
                    anim.SetTrigger("Attack");
                }
            }
        }

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 随机再地图上走来走去
    /// </summary>
    private void MoveRandomPosition()
    {
        MoveToTarget(transform.position, roamPosition);


        if (Vector3.Distance(transform.position, roamPosition) < 0.1f || timer <= 0)
        {
            if (timer <= 0)
            {
                roamPosition = GetRoamingPosition(startingPosition);

                timer = coolTime;
                anim.SetBool("IsMoving", true);
            }
            else
            {
                anim.SetBool("IsMoving", false);
                moveX = moveY = 0;

                timer -= Time.deltaTime;
            }
        }
        
    }

    private Vector3 GetRoamingPosition(Vector3 pos)
    {
        return pos + GetRandomDir() * Random.Range(-5f, 5f);
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
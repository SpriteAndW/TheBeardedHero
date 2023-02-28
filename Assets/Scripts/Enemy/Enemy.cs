using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    //组件
    protected GameObject UIBar;
    protected GameObject ShadowSp;
    protected Animator anim;
    protected SpriteRenderer sp;
    protected BoxCollider2D coll;
    protected Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    protected Transform target;
    protected EnemyData_SO enemyData;
    public float currentEnemyHp;
    public int enemyID;
    public EnemyDetails enemyDetail;


    [Header("Hurt")]
    private float hurtLength = 0.1f;
    private float hurtCounter;
    public EnemyState enemyState = EnemyState.NoFind;

    protected float moveX;
    protected float moveY;
    private float moveLerp = 0.1f;
    


    protected virtual void Awake()
    {
        //组件获取
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        UIBar = transform.GetChild(0).gameObject;
        ShadowSp = transform.GetChild(1).gameObject;

        //属性获取
        enemyData = EnemyManager.Instance.EnemyDataSO;
        enemyDetail = enemyData.GetEnemyDetails(enemyID);
        currentEnemyHp = enemyDetail.maxHp;
    }

    protected virtual void Start()
    {
        UIBar.SetActive(false);
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        if (enemyDetail.currentHp <= 0)
        {
            return;
        }

        if (hurtCounter <= 0)
        {
            spriteRenderer.material.SetFloat("_FlashAmount", 0);
        }
        else
        {
            hurtCounter -= Time.deltaTime;
        }

        anim.SetFloat("MoveX", rb.velocity.x);
        anim.SetFloat("MoveY", rb.velocity.y);
    }

    protected virtual void FixedUpdate()
    {
        
    }


    public void TakenDamage(float _amount)
    {
        currentEnemyHp -= _amount;
        HurtShader();
        UIBar.SetActive(true);
        UIBar.GetComponent<HealthBar>().UpdateHp();
        if (currentEnemyHp <= 0)
        {
            //TODO:可以先销毁敌人，再播放销毁动画
            coll.enabled = false;
            anim.enabled = false;
            anim.SetTrigger("Die");
        }
    }

    protected void HurtShader()
    {
        spriteRenderer.material.SetFloat("_FlashAmount", 1);
        hurtCounter = hurtLength;
    }

    public void Death()
    {
        //敌人掉落物品
        var lootitem = gameObject.GetComponent<LootItem>();
        if (lootitem != null)
        {
            lootitem.SpwnLootItem(transform.position);
        }

        //TODO:生成销毁可以放到对象池中
        Destroy(this.gameObject);
    }

    public void MoveToTarget(Vector3 current, Vector3 target)
    {
        if (current.y >= target.y + moveLerp)
        {
            moveY = -1;
        }
        else if (current.y <= target.y-moveLerp)
        {
            moveY = 1;
        }
        else
        {
            moveY = 0;
        }
        
        if (current.x >= target.x + moveLerp)
        {
            moveX = -1;
        }
        else if (current.x <= target.x -moveLerp)
        {
            moveX = 1;
        }
        else
        {
            moveX = 0;
        }
        
    }
}
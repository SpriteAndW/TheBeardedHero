using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    public GameObject explosionPrefab;

    public GameObject damageCanvas;

    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private SpriteRenderer sp => GetComponent<SpriteRenderer>();
    private BoxCollider2D coll => GetComponent<BoxCollider2D>();

    public int bulletID;
    public BulletDetail bulletDetail;

    private float attackDamage = 10;
    private Vector2 startPos;
    private float timer;

    private void OnEnable()
    {
        timer = 0;
    }

    private void Start()
    {
        if (bulletID != 0)
        {
            Init(bulletID);
        }
        startPos = transform.position;
    }

    void Update()
    {
        DistanceWithPlayer(bulletDetail.bulletTime);
    }

    public void Init(int ID)
    {
        bulletID = ID;
        bulletDetail = InventoryManager.Instance.BulletDetailData.GetBulletDetil(bulletID);

        if (bulletDetail != null)
        {
            sp.sprite = bulletDetail.bulletSprite;

            //修改碰撞体尺寸
            Vector2 newSize = new Vector2(sp.sprite.bounds.size.x + 0.1f, sp.sprite.bounds.size.y + 0.1f);
            coll.size = newSize;
            coll.offset = new Vector2(sp.sprite.bounds.center.x, sp.sprite.bounds.center.y);
        }
    }


    public void SetSpeed(Vector2 direction, BulletDetail bulletDetail)
    {
        if (bulletDetail.bulletType == BulletType.Player)
        {
            Vector2 mousePos =
                (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized; //获取鼠标与人物坐标
            float rotZ = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg; //将数字转换为角度

            transform.rotation = Quaternion.Euler(0, 0, rotZ);
        }
        else
        {
            float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        rb.velocity = direction.normalized * bulletDetail.bulletSpeed;
    }


    
    private void DistanceWithPlayer(float bulletTime)
    {
        timer += Time.deltaTime;
        if (timer > bulletTime)
        {
            ObjectPool.Instance.PushObject(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (bulletDetail.bulletType == BulletType.Player)
        {
            if (other.CompareTag("Wall") || other.CompareTag("Enemy"))
            {
                if (other.CompareTag("Enemy"))
                {
                    attackDamage =
                        (Random.Range(bulletDetail.minDamage, bulletDetail.maxDamage) +
                         HealthManager.Instance.currentAttack) * HealthManager.Instance.currentDamageCount;
                    other.GetComponent<Enemy>().TakenDamage(attackDamage);
                    GameObject damegeText = ObjectPool.Instance.GetObject(damageCanvas, other.transform.position);
                    damegeText.GetComponent<DamageNumber>().ShowUIDamage(Mathf.RoundToInt(attackDamage));
                    damegeText.transform.position = other.transform.position;

                    other.GetComponentInChildren<HealthBar>().UpdateHp();
                    other.GetComponent<Enemy>().enemyState = EnemyState.floowPlayer;
                    // 击退效果  根据敌人和子弹坐标
                    // Vector2 difference = HealthChange.transform.position - transform.position;
                    // HealthChange.transform.position = new Vector2(HealthChange.transform.position.x + difference.x, HealthChange.transform.position.y + difference.y);
                }

                ObjectPool.Instance.GetObject(explosionPrefab, other.transform.position);
                ObjectPool.Instance.PushObject(this.gameObject);
            }
        }
        else
        {
            if (other.CompareTag("Wall") || other.CompareTag("Player"))
            {
                if (other.CompareTag("Player") && other.GetComponent<PlayerHealth>().canHurt &&
                    !other.GetComponent<PlayerMovement>().isDashing)
                {
                    other.GetComponent<PlayerHealth>().Hurt(bulletDetail.minDamage, bulletDetail.maxDamage);

                    float damage = other.GetComponent<PlayerHealth>().damage * HealthManager.Instance.currentHurtCount;
                    GameObject damegeText = ObjectPool.Instance.GetObject(damageCanvas, other.transform.position);
                    damegeText.GetComponent<DamageNumber>().ShowUIDamage(Mathf.RoundToInt(damage));
                    damegeText.transform.position = other.transform.position;
                }

                ObjectPool.Instance.GetObject(explosionPrefab, other.transform.position);
                ObjectPool.Instance.PushObject(this.gameObject);
            }
        }
    }
}
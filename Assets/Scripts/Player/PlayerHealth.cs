using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerHealth : MonoBehaviour
{
    private SpriteRenderer sp;

    // private float hurtLength = 0.5f;
    private float hurtCounter;
    public bool canHurt;
    public float damage;


    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (hurtCounter <= 0)
        {
            canHurt = true;
            sp.material.SetFloat("_FlashAmount", 0);
        }
        else
        {
            hurtCounter -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && canHurt && !GetComponent<PlayerMovement>().isDashing)
        {
            Hurt(other.GetComponent<Enemy>().enemyDetail.minDamage, other.GetComponent<Enemy>().enemyDetail.maxDamage);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && canHurt && !GetComponent<PlayerMovement>().isDashing)
        {
            Hurt(other.GetComponent<Enemy>().enemyDetail.minDamage, other.GetComponent<Enemy>().enemyDetail.maxDamage);
        }
    }
    

    /// <summary>
    /// 受伤产生Shader变化，且相机抖动
    /// </summary>
    /// <param name="minDamage">最小伤害</param>
    /// <param name="maxDamage">最大伤害</param>
    public void Hurt(float minDamage, float maxDamage)
    {
        //TODO:受伤音效
        damage = Mathf.Max(1, Random.Range(minDamage, maxDamage) - HealthManager.Instance.currentDefense);

        HealthManager.Instance.currentHealth -= damage * HealthManager.Instance.currentHurtCount;
        EventHandler.CallUpdateHealthUI();
        HurtShader();
        transform.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
    }

    private void HurtShader()
    {
        canHurt = false;
        sp.material.SetFloat("_FlashAmount", 0.8f);
        hurtCounter = HealthManager.Instance.hurtLength;
    }
}
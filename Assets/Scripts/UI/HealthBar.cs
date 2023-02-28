using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image hpImage;
    private Image hpEffectImage;

    private float hp;
    private float maxHp;

    private float hurtSpeed = 0.0002f;
    private float timeLeft = 5f;

    private void Awake()
    {
        hpImage = transform.GetChild(2).GetComponent<Image>();

        hpEffectImage = transform.GetChild(1).GetComponent<Image>();
    }

    private void Start()
    {
        maxHp = transform.GetComponentInParent<Enemy>().enemyDetail.maxHp;
        hp = transform.GetComponentInParent<Enemy>().enemyDetail.currentHp;
    }

    private void OnEnable()
    {
        UpdateHp();
    }

    private void LateUpdate()
    {
        if (gameObject.activeInHierarchy)
        {
            if (timeLeft <= 0)
            {
                gameObject.SetActive(false);
                timeLeft = 5f;
            }
            else
            {
                timeLeft -= Time.deltaTime;
            }
        }
    }

    public void UpdateHp()
    {
        timeLeft = 5f;

        maxHp = transform.GetComponentInParent<Enemy>().enemyDetail.maxHp;
        hp = transform.GetComponentInParent<Enemy>().currentEnemyHp;
        StartCoroutine(UpdateHpCo());
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }

    }

    private IEnumerator UpdateHpCo()
    {
        hpImage.fillAmount = hp / maxHp;
        while (hpEffectImage.fillAmount >= hpImage.fillAmount)
        {
            hpEffectImage.fillAmount -= hurtSpeed;
            yield return new WaitForSeconds(hurtSpeed);
        }

        if (hpEffectImage.fillAmount < hpImage.fillAmount)
        {
            hpEffectImage.fillAmount = hpImage.fillAmount;
        }
    }
}
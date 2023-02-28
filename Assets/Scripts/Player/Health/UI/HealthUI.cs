using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public GameObject[] HealthHears;
    public float hurtSpeed = 0.0002f;

    private void OnEnable()
    {
        EventHandler.UpdateHealthUI += OnUpdateHealthUI;
    }

    private void OnDisable()
    {
        EventHandler.UpdateHealthUI -= OnUpdateHealthUI;

    }

    private void Start()
    {
        foreach (var healthHear in HealthHears)
        {
            healthHear.SetActive(false);
        }
        EventHandler.CallUpdateHealthUI();
    }

    /// <summary>
    /// 更新血量最大值和血量UI
    /// </summary>
    private void OnUpdateHealthUI()
    {
        for (int i = 0; i < 8; i++)
        {
            if (HealthManager.Instance.maxHealth >= (i + 1) * 50)
            {
                HealthHears[i].SetActive(true);
            }
            else
            {
                HealthHears[i].SetActive(false);
            }
        }

        for (int i = 0; i < 8; i++)
        {
            if (HealthManager.Instance.currentHealth <= (i + 1) * 50)
            {
                HealthHears[i].transform.GetChild(2).GetComponent<Image>().fillAmount =
                    (HealthManager.Instance.currentHealth - (i * 50)) / 50;
                StartCoroutine(UpdateHpCo(i, HealthManager.Instance.currentHealth));
            }
            else
            {
                HealthHears[i].transform.GetChild(2).GetComponent<Image>().fillAmount = 1;
            }
        }
    }

    private IEnumerator UpdateHpCo(int i, float currentHealth)
    {
        HealthHears[i].transform.GetChild(2).GetComponent<Image>().fillAmount = (currentHealth - (i * 50)) / 50;
        while (HealthHears[i].transform.GetChild(1).GetComponent<Image>().fillAmount >=
               HealthHears[i].transform.GetChild(2).GetComponent<Image>().fillAmount)
        {
            HealthHears[i].transform.GetChild(1).GetComponent<Image>().fillAmount -= hurtSpeed;
            yield return new WaitForSeconds(hurtSpeed);
        }

        if (HealthHears[i].transform.GetChild(1).GetComponent<Image>().fillAmount <
            HealthHears[i].transform.GetChild(2).GetComponent<Image>().fillAmount)
        {
            HealthHears[i].transform.GetChild(1).GetComponent<Image>().fillAmount =
                HealthHears[i].transform.GetChild(2).GetComponent<Image>().fillAmount;
        }
    }
}
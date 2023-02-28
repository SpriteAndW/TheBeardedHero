using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class DamageNumber : MonoBehaviour
{
    public Text damageText;
    public float liftTime;
    private float uptime;
    public float upSpeed;
    private float isleft;

    private void OnEnable()
    {
        Invoke(nameof(PushPool), liftTime);
        isleft = Random.Range(-2f, 2f);
        uptime = liftTime;
    }

    private void Update()
    {
        if (uptime >= 0.5f)
        {
            transform.position += new Vector3(isleft * Time.deltaTime, upSpeed * Time.deltaTime, 0);
        }
        else if (uptime >= 0.25)
        {
            transform.position += new Vector3(isleft * Time.deltaTime, -upSpeed * Time.deltaTime * 2, 0);
        }
        uptime -= Time.deltaTime;
    }


    public void ShowUIDamage(float _amount)
    {
        damageText.text = _amount.ToString();
    }

    private void PushPool()
    {
        ObjectPool.Instance.PushObject(this.gameObject);
    }
}
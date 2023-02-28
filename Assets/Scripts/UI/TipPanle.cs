using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TipPanle : MonoBehaviour
{
    private TextMeshProUGUI text;
    private CanvasGroup canvasGroup;
    private Vector3 pos;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }


    /// <summary>
    /// 提示内容
    /// </summary>
    /// <param name="tipType">提示类型</param>
    public IEnumerator DiaPlayTip(TipType tipType)
    {
        
        text.text = GetTipType(tipType);
        transform.position = new Vector3(960,800,0);
        canvasGroup.DOFade(1, 0.5f);
        yield return new WaitForSeconds(1f);

        transform.DOMoveY(1000, 0.5f);
        canvasGroup.DOFade(0, 0.5f);
    }

    private string GetTipType(TipType tipType)
    {
        return tipType switch
        {
            TipType.Save => "<color=yellow>游戏保存成功</color>",
            TipType.BuySuccessful => "<color=yellow>商品购买成功</color>",
            TipType.BuyFail => "<color=red>金币不足,购买商品失败</color>",
            TipType.SellSuccessful => "<color=yellow>物品贩卖成功</color>",
            TipType.SellFail => "<color=red>物品无法贩卖</color>",
            TipType.MadeEqSuccessful => "<color=yellow>物品打造成功!!!</color>",
            TipType.MadeEqFail => "<color=red>材料不足,无法打造</color>",
            _ => "无",
        };
    }
}
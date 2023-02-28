using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator anim;
    private AnimatorStateInfo info;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1)
        {
            // Destroy(this.gameObject);
            ObjectPool.Instance.PushObject(this.gameObject);
        }
    }
}

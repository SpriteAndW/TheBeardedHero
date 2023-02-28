using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : Singleton<PlayerMovement>
{
    private Rigidbody2D rb;
    private Animator anim;
    public bool isFighting = true;
    public bool DisableInput;
    public GameObject dashShadowPrefab;


    private float moveX, moveY;
    private bool isMovIng;

    private Vector2 mousePos;

    //[Header("Dash参数")] 
    public bool isDashing;
    private float dashTime = 0.2f; //冲刺时间
    private float dashTimeLeft; //冲刺剩余时间
    private float lastDash = -10f;
    private float dashCoolDown = 0.5f;
    private float dashSpeed = 15;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += BeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += AfterSceneLoadedEvent;
        EventHandler.MoveToPosition += MoveToPositionEvent;
    }


    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= BeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= AfterSceneLoadedEvent;
        EventHandler.MoveToPosition -= MoveToPositionEvent;
    }

    private void MoveToPositionEvent(Vector3 pos)
    {
        transform.position = pos;
    }

    private void AfterSceneLoadedEvent()
    {
        DisableInput = false;
    }

    private void BeforeSceneUnloadEvent()
    {
        DisableInput = true;
    }

    private void Update()
    {
        if (DisableInput == false)
        {
            PlayerInput();
            CursePosition();
        }

        SwitchAnimation();
    }

    private void FixedUpdate()
    {
        if (DisableInput==false)
        {
            rb.velocity = new Vector2(moveX * HealthManager.Instance.currentMoveSpeed, moveY * HealthManager.Instance.currentMoveSpeed);
            Dash();
        }
        else
        {
            rb.velocity = Vector2.zero;
            isMovIng = false;
        }
    }

    /// <summary>
    /// 玩家输入控制
    /// </summary>
    private void PlayerInput()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        if (moveX != 0 && moveY != 0)
        {
            moveX *= 0.6f;
            moveY *= 0.6f;
        }

        Vector2 moveMentVector = new Vector2(moveX, moveY);
        isMovIng = moveMentVector != Vector2.zero;

        if (Input.GetKeyDown(KeyCode.LeftShift) && isMovIng)
        {
            if (Time.time >= (lastDash + dashCoolDown))
            {
                ReadyToDash();
                
                // HealthManager.Instance.dashCount -= 1;

            }
        }
    }

    /// <summary>
    /// 切换动画
    /// </summary>
    private void SwitchAnimation()
    {
        anim.SetBool("IsMoving", isMovIng);
        if (isFighting) //判断周围是否有敌人或者进入战斗状态
        {
            anim.SetFloat("InputX", mousePos.x);
            anim.SetFloat("InputY", mousePos.y);
        }
        else
        {
            if (isMovIng)
            {
                anim.SetFloat("InputX", moveX);
                anim.SetFloat("InputY", moveY);
            }
        }
    }

    private void CursePosition()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }


    private void ReadyToDash()
    {
        isDashing = true;
        
        dashTimeLeft = dashTime;

        lastDash = Time.time;
    }

    private void Dash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                rb.velocity = new Vector2(dashSpeed * moveX, moveY * dashSpeed);

                dashTimeLeft -= Time.fixedDeltaTime;

                ObjectPool.Instance.GetObject(dashShadowPrefab, transform.position);
            }

            if (dashTimeLeft <= 0)
            {
                isDashing = false;
            }
        }
    }
}
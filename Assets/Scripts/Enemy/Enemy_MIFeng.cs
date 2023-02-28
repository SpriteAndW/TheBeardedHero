using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_MIFeng : Enemy
{
    private Vector3 startPos;
    private float waitTime = 2;
    private float timeStart;
    
    private Stack<Vector2Int> MoveStepStack;
    public List<Vector2Int> MoveStepList;


    protected override void Start()
    {
        base.Start();
        startPos = transform.position;
        timeStart = waitTime;
        
        MoveStepStack = new Stack<Vector2Int>();
        MoveStepList = new List<Vector2Int>();
    }

    protected override void Update()
    {
        base.Update();
        MiFengActive();
    }

    protected override  void FixedUpdate()
    {
        if (enemyState != EnemyState.NoFind)
        {
            rb.velocity = new Vector2(enemyDetail.moveSpeed * moveX, enemyDetail.moveSpeed * moveY);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void Movement(Vector3 target)
    {
        MoveStepStack.Clear();
        MoveStepList.Clear();

        Vector2Int startPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2Int endPos = new Vector2Int((int)target.x, (int)target.y);
        AStar.Instance.BuildPath(SceneManager.GetActiveScene().name, startPos, endPos, MoveStepStack);

        foreach (var step in MoveStepStack)
        {
            MoveStepList.Add(new Vector2Int(step.x, step.y));
        }
    }
    
    private void MoveRandom()
    {
        if (MoveStepList.Count < 1)
        {
            Movement(target.position);
        }
        if (MoveStepList.Count==0)
        {
            return;
        }
        if (Vector2.Distance(transform.position, MoveStepList[0]) < 0.25f)
        {
            MoveStepList.RemoveAt(0);
        }
        else
        {
            MoveToTarget(transform.position, new Vector3(MoveStepList[0].x, MoveStepList[0].y, 0));
        }
        
    }

    /// <summary>
    /// 如果玩家靠近蜜蜂，它就会跟随玩家
    /// </summary>
    private void MiFengActive()
    {
        if (enemyState == EnemyState.NoFind)
        {
            if (Vector2.Distance(transform.position, target.position) < enemyDetail.distance)
            {
                enemyState = EnemyState.floowPlayer;
            }
        }


        if (enemyState == EnemyState.floowPlayer)
        {
            if (Vector2.Distance(transform.position, target.position) > 2 * enemyDetail.distance)
            {
                if (timeStart <= 0)
                {
                    enemyState = EnemyState.NoFind;
                    timeStart = waitTime;
                }
                else
                {
                    timeStart -= Time.deltaTime;
                }
            }
            else
            {
                // MoveToTarget(transform.position,target.position);
                MoveRandom();
            }
        }
    }
}
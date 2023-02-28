using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class Enemy_Slim : Enemy
{
    private Vector3 startPosition;
    private Vector3 roamPosition;
    public int coolTime = 3;

    private Stack<Vector2Int> MoveStepStack;
    private List<Vector2Int> MoveStepList;

    protected override void Start()
    {
        startPosition = transform.position;
        roamPosition = GetRoamingPosition(10);
        MoveStepStack = new Stack<Vector2Int>();
        MoveStepList = new List<Vector2Int>();
    }

    protected override void Update()
    {
        base.Update();

    }

    protected override void FixedUpdate()
    {
        MoveRandom();

        rb.velocity = new Vector2(enemyDetail.moveSpeed * moveX, enemyDetail.moveSpeed * moveY);
    }

    private void Movement()
    {
        MoveStepStack.Clear();
        MoveStepList.Clear();

        Vector2Int startPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2Int endPos = new Vector2Int((int)roamPosition.x, (int)roamPosition.y);
        AStar.Instance.BuildPath(SceneManager.GetActiveScene().name, startPos, endPos, MoveStepStack);

        foreach (var step in MoveStepStack)
        {
            MoveStepList.Add(new Vector2Int(step.x,step.y));
        }
    }


    private void MoveRandom()
    {
        if (MoveStepList.Count < 1)
        {
            roamPosition = GetRoamingPosition(10);

            StopCoroutine(WaitSecond(coolTime));
            StartCoroutine(WaitSecond(coolTime));
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

    private IEnumerator WaitSecond(int second)
    {
        moveX = moveY = 0;
        anim.SetBool("IsMoving",false);
        yield return new WaitForSeconds(second);
        Movement();
        anim.SetBool("IsMoving",true);
    }

    /// <summary>
    /// 随机数值
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRoamingPosition(int round)
    {
        return startPosition + GetRandomDir() * Random.Range(-round, round);
    }

    /// <summary>
    /// 随机方向
    /// </summary>
    /// <returns></returns>
    private static Vector3 GetRandomDir()
    {
        return new Vector3(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
    }
}
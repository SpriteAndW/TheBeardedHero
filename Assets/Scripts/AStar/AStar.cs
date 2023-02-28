using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : Singleton<AStar>
{
    private GridNodes gridNodes;
    private Node startNode;
    private Node targetNode;
    private int gridWidth;
    private int gridHeight;
    private int originX;
    private int originY;

    private List<Node> openNodeList; //当前Node周围的8个点
    private HashSet<Node> closeNodeList; //所有选中的点  查找快,添加慢 TODO:敌人经常添加,可能更换List

    private bool pathFound;


    /// <summary>
    /// 构建路径更新Stack的每一步
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="movementStack"></param>
    public void BuildPath(string sceneName, Vector2Int startPos, Vector2Int endPos, Stack<Vector2Int> movementStep)
    {
        pathFound = false;

        if (GenerateGridNodes(sceneName, startPos, endPos))
        {
            //查找最短路径
            if (FindShortesPath())
            {
                //构建NPC移动路径
                UpdatePathOnMovementStepStack(movementStep);
            }
        }
    }

    /// <summary>
    /// 构建网格节点信息,初始化两个列表
    /// </summary>
    /// <param name="sceneName">场景名字</param>
    /// <param name="startPos">起点</param>
    /// <param name="endPos">终点</param>
    /// <returns></returns>
    private bool GenerateGridNodes(string sceneName, Vector2Int startPos, Vector2Int endPos)
    {
        if (GridMapManager.Instance.GetGridDimensionS(sceneName, out Vector2Int gridDimensions,
                out Vector2Int gridOrigin))
        {
            //根据瓦片地图范围构建网格移动节点范围数组
            gridNodes = new GridNodes(gridDimensions.x, gridDimensions.y);
            gridWidth = gridDimensions.x;
            gridHeight = gridDimensions.y;
            originX = gridOrigin.x;
            originY = gridOrigin.y;

            openNodeList = new List<Node>();
            closeNodeList = new HashSet<Node>();
        }
        else
            return false;

        //gridNodes的范围是从0,0开始,需要减去原点坐标得到实际的网格位置
        startNode = gridNodes.GetGridNode(startPos.x - originX, startPos.y - originY);
        targetNode = gridNodes.GetGridNode(endPos.x - originX, endPos.y - originY);

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                var key = (x + originX) + "x" + (y + originY) + "y" + sceneName;
                TileDetails tileDetails = GridMapManager.Instance.GetTileDetails(key);

                if (tileDetails != null)
                {
                    Node node = gridNodes.GetGridNode(x, y);

                    if (tileDetails.isObstacle)
                    {
                        node.isObstacle = true;
                    }
                }
            }
        }

        return true;
    }

    /// <summary>
    /// 找到最短的路径所有的Node添加到closeNodeList里
    /// </summary>
    /// <returns></returns>
    private bool FindShortesPath()
    {
        //添加起点
        openNodeList.Add(startNode);

        while (openNodeList.Count > 0)
        {
            //排序列表,Node内涵比较函数
            openNodeList.Sort();

            //最近的点,一开始就是自己,也就是0
            Node closeNode = openNodeList[0];

            //一直输出最近的点,直到最近的点为目标点
            openNodeList.RemoveAt(0); //删除当前最近的点,循环下一个最近的点
            closeNodeList.Add(closeNode); //记录每一个最近的点
            if (closeNode == targetNode)
            {
                pathFound = true;
                break;
            }

            //起算周围8哥Node补充到OpenList
            EvaluateNeighbourNodes(closeNode);
        }

        return pathFound;
    }


    /// <summary>
    /// 评估周围8个点,并生成对应的消耗值
    /// </summary>
    /// <param name="currentNode"></param>
    private void EvaluateNeighbourNodes(Node currentNode)
    {
        Vector2Int currentNodePos = currentNode.gridPosition;
        Node validNeighbourNode;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                validNeighbourNode = GetValidNeighbourNode(currentNodePos.x + x, currentNodePos.y + y);

                if (validNeighbourNode != null)
                {
                    if (!openNodeList.Contains(validNeighbourNode))
                    {
                        validNeighbourNode.gCost = currentNode.gCost + GetDistance(currentNode, validNeighbourNode);
                        validNeighbourNode.hCost = GetDistance(validNeighbourNode, currentNode);
                        //链接父节点
                        validNeighbourNode.parentNode = currentNode;
                        openNodeList.Add(validNeighbourNode);
                    }
                }
            }
        }
    }


    /// <summary>
    /// 找到有效的Node,非障碍,非已选择
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private Node GetValidNeighbourNode(int x, int y)
    {
        //格子超标,返回空
        if (x >= gridWidth || y >= gridHeight || x < 0 || y < 0)
        {
            return null;
        }

        Node neighbour = gridNodes.GetGridNode(x, y);

        //周围的点是障碍或已经在最近路线表里,返回空
        if (neighbour.isObstacle || closeNodeList.Contains(neighbour))
        {
            return null;
        }
        else
        {
            return neighbour;
        }
    }

    /// <summary>
    /// 返回两点的距离值
    /// </summary>
    /// <param name="nodeA"></param>
    /// <param name="nodeB"></param>
    /// <returns>斜的*14+直的*10</returns>
    private int GetDistance(Node nodeA, Node nodeB)
    {
        int xDistance = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int yDistance = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

        if (xDistance > yDistance)
        {
            return 14 * yDistance + 10 * (xDistance - yDistance);
        }

        return 14 * xDistance + 10 * (yDistance - xDistance);
    }


    /// <summary>
    /// 更新路径的每一步的坐标
    /// </summary>
    /// <param name="movementStep"></param>
    private void UpdatePathOnMovementStepStack(Stack<Vector2Int> movementStep)
    {
        Node nextNode = targetNode;

        while (nextNode != null)
        {
            var newStep = new Vector2Int(nextNode.gridPosition.x + originX,nextNode.gridPosition.y + originY);

            //压入堆栈
            movementStep.Push(newStep);
            nextNode = nextNode.parentNode;
        }
    }
}

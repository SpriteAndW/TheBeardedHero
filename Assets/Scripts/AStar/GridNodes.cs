using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNodes
{
    private int width;
    private int height;
    private Node[,] grideNode;

    public GridNodes(int width, int height)
    {
        this.width = width;
        this.height = height;

        grideNode = new Node[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grideNode[x, y] = new Node(new Vector2Int(x, y));
            }
        }
    }

    public Node GetGridNode(int xPos, int yPos)
    {
        if (xPos<width && yPos<height)
        {
            return grideNode[xPos, yPos];
        }
        Debug.Log("超出网格范围");
        return null;
    }
}

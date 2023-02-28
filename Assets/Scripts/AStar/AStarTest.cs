using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class AStarTest : MonoBehaviour
{
    private AStar aStar;
    [Header("用于测试")] public Vector2Int startPos;
    // public Vector2Int finishPos;
    public Tilemap displayMap;
    public TileBase displayTile;
    public bool displayStartAndFinish;
    public bool displayPath;

    private Stack<Vector2Int> movementStack;

    public GameObject player;
    
    
    private void Awake()
    {
        aStar = GetComponent<AStar>();
        movementStack = new Stack<Vector2Int>();
    }

    private void Update()
    {
        ShowPathOnGridMap();
    }

    private void ShowPathOnGridMap()
    {
        if (displayMap != null && displayTile != null)
        {
            if (displayStartAndFinish)
            {
                displayMap.SetTile((Vector3Int)startPos, displayTile);
                displayMap.SetTile((Vector3Int)new Vector3Int((int)player.transform.position.x-1,(int)player.transform.position.y-1,0), displayTile);
            }
            else
            {
                displayMap.SetTile((Vector3Int)startPos, null);
                displayMap.SetTile((Vector3Int)new Vector3Int((int)player.transform.position.x-1,(int)player.transform.position.y-1,0), null);
            }

            if (displayPath)
            {
                var sceneName = SceneManager.GetActiveScene().name;

                aStar.BuildPath(sceneName, startPos, new Vector2Int((int)(player.transform.position.x-1),(int)(player.transform.position.y)-1), movementStack);

                foreach (var step in movementStack)
                {
                    displayMap.SetTile((Vector3Int)step, displayTile);
                }
            }
            else
            {
                if (movementStack.Count > 0)
                {
                    foreach (var step in movementStack)
                    {
                        displayMap.SetTile((Vector3Int)step, null);
                    }

                    movementStack.Clear();
                }
            }
        }
    }
}
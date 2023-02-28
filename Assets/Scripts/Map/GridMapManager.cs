using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMapManager : Singleton<GridMapManager>
{
    [Header("地图信息")] public List<MapData_SO> mapDataList;

    private Dictionary<string, TileDetails> tileDetailsDict = new Dictionary<string, TileDetails>();


    private void Start()
    {
        foreach (var mapData in mapDataList)
        {
            InitTileDetailsDict(mapData);
        }
    }


    private void InitTileDetailsDict(MapData_SO mapData)
    {
        //循环每一个格子里的信息给到字典
        //循环每一个格子里的信息给到字典
        foreach (TileProperty tileProperty in mapData.tilePropertiesList)
        {
            TileDetails tileDetails = new TileDetails
            {
                girdX = tileProperty.tileCoordinate.x,
                gridY = tileProperty.tileCoordinate.y,
            };

            //字典的Key
            string key = tileDetails.girdX + "x" + tileDetails.gridY + "y" + mapData.sceneName;


            if (GetTileDetails(key) != null)
            {
                tileDetails = GetTileDetails(key);
            }

            switch (tileProperty.gridType)
            {
                case GridType.isObstacle:
                    tileDetails.isObstacle = tileProperty.boolTypeValue;
                    break;
                case GridType.canDig:
                    tileDetails.canDig = tileProperty.boolTypeValue;
                    break;
            }

            if (GetTileDetails(key) != null)
            {
                tileDetailsDict[key] = tileDetails;
            }
            else
            {
                tileDetailsDict.Add(key, tileDetails);
            }
        }
    }

    public TileDetails GetTileDetails(string key)
    {
        if (tileDetailsDict.ContainsKey(key))
        {
            return tileDetailsDict[key];
        }

        return null;
    }

    
    
    /// <summary>
    /// 根据场景名字构建网格范围,输出范围和原点
    /// </summary>
    /// <param name="scenName">场景名字</param>
    /// <param name="gridDimensions">网格范围</param>
    /// <param name="gridOrigin">网格原点</param>
    /// <returns>是否有当前场景的信息</returns>
    public bool GetGridDimensionS(string scenName,out Vector2Int gridDimensions,out Vector2Int gridOrigin)
    {
        gridDimensions = Vector2Int.zero;
        gridOrigin = Vector2Int.zero;
        
        foreach (var mapData in mapDataList)
        {
            if (mapData.sceneName == scenName)
            {
                gridDimensions.x = mapData.gridWidth;
                gridDimensions.y = mapData.gridHeight;

                gridOrigin.x = mapData.originX;
                gridOrigin.y = mapData.originY;

                return true;
            }
        }

        return false;
    }
}
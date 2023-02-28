using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MapData", menuName = "Map/MapData_SO")]
public class MapData_SO : ScriptableObject
{
    [SceneName] 
    public string sceneName;
    [Header("地图信息")] 
    public int gridWidth;
    public int gridHeight;
    
    [Header("左下角原点")]
    public int originX;
    public int originY;
    public List<TileProperty> tilePropertiesList;
}

[System.Serializable]
public class TileProperty
{
    public Vector2Int tileCoordinate; //网格坐标
    public GridType gridType;
    public bool boolTypeValue;
}

public class TileDetails
{
    public int girdX, gridY;
    public bool isObstacle;
    public bool canDig;
}
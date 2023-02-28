using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

[ExecuteInEditMode] //在编辑的模式下进行
public class GridMap : MonoBehaviour
{
    public MapData_SO mapData;
    public GridType gridType;
    private Tilemap currentTileMap;

    private void OnEnable()
    {
        if (!Application.isPlaying) //在编辑器中，如果编辑器处于播放模式，将返回 true
        {
            currentTileMap = GetComponent<Tilemap>();

            if (mapData != null)
            {
                mapData.tilePropertiesList.Clear();
            }
        }
    }

    private void OnDisable()
    {
        if (!Application.isPlaying)
        {
            currentTileMap = GetComponent<Tilemap>();

            UpdateTileProperties();
#if UNITY_EDITOR
            if (mapData != null)
            {
                EditorUtility.SetDirty(mapData);
            }
#endif
        }
    }

    private void UpdateTileProperties()
    {
        currentTileMap.CompressBounds(); //将网格地图的原点和大小压缩到图块所在的边界

        if (!Application.isPlaying)
        {
            if (mapData != null)
            {
                //已给到范围的左下角坐标
                Vector3Int startPos = currentTileMap.cellBounds.min; //cellBounds 以单元格大小返回 Tilemap 的边界。
                //已给到范围的右上角坐标
                Vector3Int endPos = currentTileMap.cellBounds.max;

                for (int x = startPos.x; x < endPos.x; x++)
                {
                    for (int y = startPos.y; y < endPos.y; y++)
                    {
                        TileBase tile = currentTileMap.GetTile(new Vector3Int(x, y, 0));

                        if (tile != null)
                        {
                            TileProperty newTile = new TileProperty
                            {
                                tileCoordinate = new Vector2Int(x, y),
                                gridType = this.gridType,
                                boolTypeValue = true
                            };

                            mapData.tilePropertiesList.Add(newTile);
                        }
                    }
                }
            }
        }
    }
}
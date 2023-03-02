using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : SingletonBehaviour<TileController>
{
    [SerializeField] Transform TileMapParent;
    [SerializeField] GameObject tilePrefab;

    [SerializeField] int mapSizeY = 21;
    public int MapSizeY { get { return mapSizeY; } }

    [SerializeField] int mapSizeX = 9;
    public int MaxSizeX { get { return mapSizeX; } }

    [SerializeField] Vector2 startPosition;

    int tileCount = 1;
    List<Tile> tileList = new List<Tile>();
    public List<Tile> TileList { get { return tileList; } }

    int maxTileCount;
    public int MaxTileCount { get { return maxTileCount; } }

    float tileSize;
    float distanceX;
    float distanceY;

    void Awake()
    {
        // tilePrefab.GetComponent<RectTransform>().sizeDelta = new Vector2();
        tileSize = tilePrefab.GetComponent<RectTransform>().sizeDelta.x;

        distanceX = tileSize * 1.5f;
        distanceY = tileSize / 4 * Mathf.Sqrt(3); // 1 : √3 = tileSize/4 : distanceY
        UnityEngine.Debug.Log("distanceY : " + distanceY);

        startPosition = Vector2.zero;
        maxTileCount = (mapSizeY / 2) * (mapSizeX / 2 + 1) + (mapSizeY / 2 + 1) * (mapSizeX / 2);
        UnityEngine.Debug.Log("maxTileCount : " + maxTileCount);
    }

    void Start()
    {
        MakeTile();
    }

    void MakeTile()
    {
        for (int y = 0; y < mapSizeY; y++)
        {
            if (y % 2 == 0)
            {
                for (int x = 0; x < mapSizeX/2; x++)
                {
                    Vector2 tilePosition = new Vector2(startPosition.x + distanceX / 2 + distanceX * x, distanceY * y);
                    CreateTile(tilePosition);
                }
            }
            else
            {
                for (int x = 0; x < mapSizeX/2 + 1; x++)
                {
                    Vector2 tilePosition = new Vector2(startPosition.x + distanceX * x, distanceY * y);
                    CreateTile(tilePosition);
                }
            }
        }
    }

    // 타일 생성
    void CreateTile(Vector2 _tilePosition)
    {
        GameObject newTileObject = Instantiate(tilePrefab, _tilePosition, Quaternion.identity, TileMapParent);
        Tile newTile = newTileObject.GetComponent<Tile>();
        newTile.ID = tileCount++; // 타일 ID 설정 (좌하단부터 순서대로 생성)
        TileList.Add(newTile);
    }

    void CheckTileList()
    {
        for (int i = 0; i < TileList.Count; i++)
        {
            CheckTile(TileList[i]);
        }
    }

    void CheckTile(Tile _checkTile)
    {
        if (_checkTile.NowBlock == false)
        {
            if (TileList[_checkTile.Bottom].NowBlock == false)
            {
                _checkTile.NowBlock.MoveTile.Enqueue(_checkTile.Bottom);
            }
            else if (TileList[_checkTile.BottomLeft].NowBlock == false)
            {

            }
            else if (TileList[_checkTile.BottomRight].NowBlock == false)
            {

            }
        }
    }
}

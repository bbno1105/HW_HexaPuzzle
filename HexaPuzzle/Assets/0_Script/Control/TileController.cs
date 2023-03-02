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

    int tileCount = 0;
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

        if (newTile.ID == 87) newTile.TileType = TILE_TYPE.CREATE;

        TileList.Add(newTile);
    }

    public void CheckTileList()
    {
        //for (int i = TileList.Count - 1; 0 <= i; i--) // 위에서부터, 오른쪽부터 판단
        for (int i = 0; i < TileList.Count; i++)
        {
            if (TileList[i].NowBlock) // 블록이 있는 경우
            {
                if (TileList[i].NowBlock.CanMove) // 블록이 이동할 수 있는 경우
                {
                    CheckTile(TileList[i]); // 이동 가능한 공간이 있는지 판단
                }
            }
            else if (TileList[i].TileType == TILE_TYPE.CREATE) // 블록없는 생성타일 = 블록 생성
            {
                TileList[i].NowBlock = BlockController.Instance.CreateBlock(TileList[i].transform.position);
            }
        }
    }

    void CheckTile(Tile _checkTile)
    {
        int topID = _checkTile.TrapAround[(int)TRAP_AROUND.TOP];
        int topLeftID = _checkTile.TrapAround[(int)TRAP_AROUND.TOP_LEFT];
        int topRightID = _checkTile.TrapAround[(int)TRAP_AROUND.TOP_RIGHT];
        int bottomID = _checkTile.TrapAround[(int)TRAP_AROUND.BOTTOM];
        int bottomLeftID = _checkTile.TrapAround[(int)TRAP_AROUND.BOTTOM_LEFT];
        int bottomRightID = _checkTile.TrapAround[(int)TRAP_AROUND.BOTTOM_RIGHT];


        if (bottomID != -1)
        {
            if (TileList[bottomID].NowBlock == false) // 내 아래에 블럭이 없다.
            {
                TileSetting(_checkTile, TileList[bottomID]);
                return;
            }
        }

        if (topID != -1)
        {
            if (TileList[topID].NowBlock) return; // 내 위에 블럭이 있으면 넘김
        }

        switch (Random.Range(0,2))
        {
            case 0:
                if (_checkTile.TrapAround[(int)TRAP_AROUND.BOTTOM_LEFT] != -1)
                {
                    if (TileList[bottomLeftID].NowBlock == false && TileList[topLeftID].NowBlock == false) // 왼쪽 아래/위 블럭이 없다.
                    {
                        TileSetting(_checkTile, TileList[bottomLeftID]);
                        return;
                    }
                }
                else if (bottomRightID != -1)
                {
                    if (TileList[bottomRightID].NowBlock == false && TileList[topRightID].NowBlock == false) // 오른쪽 아래/위 블럭이 없다.
                    {
                        TileSetting(_checkTile, TileList[bottomRightID]);
                        return;
                    }
                }
                break;
            case 1:
                if (bottomRightID != -1)
                {
                    if (TileList[bottomRightID].NowBlock == false && TileList[topRightID].NowBlock == false) // 오른쪽 아래/위 블럭이 없다.
                    {
                        TileSetting(_checkTile, TileList[bottomRightID]);
                        return;
                    }
                }
                else if (_checkTile.TrapAround[(int)TRAP_AROUND.BOTTOM_LEFT] != -1)
                {
                    if (TileList[bottomLeftID].NowBlock == false && TileList[topLeftID].NowBlock == false) // 왼쪽 아래/위 블럭이 없다.
                    {
                        TileSetting(_checkTile, TileList[bottomLeftID]);
                        return;
                    }
                }
                break;
            default:
                break;
        }

        //if (_checkTile.BottomLeft != -1)
        //{
        //    if (TileList[_checkTile.BottomLeft].NowBlock == false && TileList[_checkTile.TopLeft].NowBlock == false) // 왼쪽 아래/위 블럭이 없다.
        //    {
        //        TileSetting(_checkTile, TileList[_checkTile.BottomLeft]);
        //        return;
        //    }
        //}
        
        //if(_checkTile.BottomRight != -1)
        //{
        //    if (TileList[_checkTile.BottomRight].NowBlock == false && TileList[_checkTile.TopRight].NowBlock == false) // 오른쪽 아래/위 블럭이 없다.
        //    {
        //        TileSetting(_checkTile, TileList[_checkTile.BottomRight]);

        //        //Vector2 startPosition = _checkTile.NowBlock.transform.position;
        //        //Vector2 endPosition = TileList[_checkTile.BottomRight].transform.position;

        //        //TileList[_checkTile.BottomRight].NowBlock = _checkTile.NowBlock;
        //        //_checkTile.NowBlock = null;

        //        //TileList[_checkTile.BottomRight].NowBlock.MoveSetting(startPosition, endPosition);
        //        return;
        //    }
        //}
    }

    public void TileSetting(Tile _tile, Tile _nextTile)
    {
        Vector2 startPosition = _tile.NowBlock.transform.position;
        Vector2 endPosition = _nextTile.transform.position;

        _nextTile.NowBlock = _tile.NowBlock;
        _tile.NowBlock = null;

        _nextTile.NowBlock.MoveSetting(startPosition, endPosition);
    }


}

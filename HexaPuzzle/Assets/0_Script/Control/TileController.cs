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
        tileSize = tilePrefab.GetComponent<RectTransform>().sizeDelta.x;

        distanceX = tileSize * 1.5f; // 블록 가로 간격
        distanceY = tileSize / 4 * Mathf.Sqrt(3); // 블록 세로 간격 ( 1 : √3 : 2 )

        startPosition = Vector2.zero;
        maxTileCount = (mapSizeY / 2) * (mapSizeX / 2 + 1) + (mapSizeY / 2 + 1) * (mapSizeX / 2);
    }

    void Start()
    {
        // 캔버스 사이즈에 비례하여 중앙에 위치하도록 설정
        Vector2 mapTotlaSize = new Vector2(tileSize + distanceX * (mapSizeX / 2), distanceY * (mapSizeY / 2 + 1));
        Vector2 cameraSize = TileMapParent.parent.GetComponent<RectTransform>().sizeDelta;
        startPosition = new Vector2(tileSize / 2 + cameraSize.x / 2 - mapTotlaSize.x / 2, distanceY + cameraSize.y / 2 - mapTotlaSize.y);

        Initialize();
    }

    void Initialize()
    {
        // 타일 생성
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
                    Vector2 tilePosition = new Vector2(startPosition.x + distanceX / 2 + distanceX * x, startPosition.y + distanceY * y);
                    CreateTile(tilePosition);
                }
            }
            else
            {
                for (int x = 0; x < mapSizeX/2 + 1; x++)
                {
                    Vector2 tilePosition = new Vector2(startPosition.x + distanceX * x, startPosition.y + distanceY * y);
                    CreateTile(tilePosition);
                }
            }
        }

        if(StaticData.StageData.TryGetValue(21, out StageSheetData stageData))
        {
            // 맵 타일 생성
            for (int i = 0; i < stageData.Maplist.Length; i++)
            {
                TileList[stageData.Maplist[i]].gameObject.SetActive(true);
                TileList[stageData.Maplist[i]].NowBlock = BlockController.Instance.CreateBlock(TileList[stageData.Maplist[i]].transform.position, (BLOCK_TYPE)stageData.Blocklist[i]);
            }

            // 타일 타입 설정
            for (int i = 0; i < stageData.Createblocktile.Length; i++)
            {
                TileList[stageData.Createblocktile[i]].TileType = TILE_TYPE.CREATE;
            }

        }

    }

    // 타일 생성
    void CreateTile(Vector2 _tilePosition)
    {
        GameObject newTileObject = Instantiate(tilePrefab, _tilePosition, Quaternion.identity, TileMapParent);
        newTileObject.SetActive(false);

        Tile newTile = newTileObject.GetComponent<Tile>();
        newTile.ID = tileCount++; // 타일 ID 설정 (좌하단부터 순서대로 생성)

        TileList.Add(newTile);
    }

    public bool CheckTileList()
    {
        bool isMoved = false;

        for (int i = 0; i < TileList.Count; i++)
        {
            if (TileList[i].NowBlock) // 블록이 있는 경우
            {
                if (CheckTile(TileList[i])) // 이동 가능한 공간이 있는지 판단
                {
                    isMoved = true; // 한 번이라도 움직였으면 true
                }

            }
            else if (TileList[i].TileType == TILE_TYPE.CREATE) // 블록없는 생성타일 = 블록 생성
            {
                BLOCK_TYPE randomBlock = BlockController.Instance.NowBlockType[Random.Range(0,BlockController.Instance.NowBlockType.Count)];
                TileList[i].NowBlock = BlockController.Instance.CreateBlock(TileList[i].transform.position, randomBlock);
                TileList[i].NowBlock.MoveSetting(new Vector2(TileList[i].transform.position.x, TileList[i].transform.position.y + distanceY), TileList[i].transform.position);
                isMoved = true; // 생성했으면 true
            }
        }
        return isMoved;
    }

    bool CheckTile(Tile _checkTile)
    {
        int topID = _checkTile.TileAround[(int)TILE_AROUND.TOP];
        int topLeftID = _checkTile.TileAround[(int)TILE_AROUND.TOP_LEFT];
        int topRightID = _checkTile.TileAround[(int)TILE_AROUND.TOP_RIGHT];
        int bottomID = _checkTile.TileAround[(int)TILE_AROUND.BOTTOM];
        int bottomLeftID = _checkTile.TileAround[(int)TILE_AROUND.BOTTOM_LEFT];
        int bottomRightID = _checkTile.TileAround[(int)TILE_AROUND.BOTTOM_RIGHT];

        if (bottomID != -1 && TileList[bottomID].gameObject.activeSelf)
        {
            if (TileList[bottomID].NowBlock == false)
            {
                BlockMoveSetting(_checkTile, TileList[bottomID]);
                return true;
            }
        }

        if (topID != -1)
        {
            if (TileList[topID].NowBlock) return false; // 내 위에 블럭이 있으면 넘김
        }

        switch (Random.Range(0, 2)) // 좌/우 랜덤하게 떨어짐
        {
            case 0:
                if (bottomLeftID != -1 && TileList[bottomLeftID].gameObject.activeSelf)
                {
                    if (TileList[bottomLeftID].NowBlock == false && TileList[topLeftID].NowBlock == false) // 왼쪽 아래/위 블럭이 없다.
                    {
                        BlockMoveSetting(_checkTile, TileList[bottomLeftID]);
                        return true;
                    }
                }
                if (bottomRightID != -1 && TileList[bottomRightID].gameObject.activeSelf)
                {
                    if (TileList[bottomRightID].NowBlock == false && TileList[topRightID].NowBlock == false) // 오른쪽 아래/위 블럭이 없다.
                    {
                        BlockMoveSetting(_checkTile, TileList[bottomRightID]);
                        return true;
                    }
                }
                break;
            case 1:
                if (bottomRightID != -1 && TileList[bottomRightID].gameObject.activeSelf)
                {
                    if (TileList[bottomRightID].NowBlock == false && TileList[topRightID].NowBlock == false) // 오른쪽 아래/위 블럭이 없다.
                    {
                        BlockMoveSetting(_checkTile, TileList[bottomRightID]);
                        return true;
                    }
                }
                if (bottomLeftID != -1 && TileList[bottomLeftID].gameObject.activeSelf)
                {
                    if (TileList[bottomLeftID].NowBlock == false && TileList[topLeftID].NowBlock == false) // 왼쪽 아래/위 블럭이 없다.
                    {
                        BlockMoveSetting(_checkTile, TileList[bottomLeftID]);
                        return true;
                    }
                }
                break;
            default:
                break;
        }

        return false;
    }

    /// <summary>
    /// 이동 가능한 블록을 이동시키는 메소드
    /// </summary>
    /// <param name="_tile">이동 가능한 블록이 포함된 Tile</param>
    /// <param name="_nextTile">블록이 이동할 Tile</param>
    public void BlockMoveSetting(Tile _tile, Tile _nextTile)
    {
        Vector2 startPosition = _tile.NowBlock.transform.position;
        Vector2 endPosition = _nextTile.transform.position;

        _nextTile.NowBlock = _tile.NowBlock;
        _tile.NowBlock = null;

        _nextTile.NowBlock.MoveSetting(startPosition, endPosition);
    }


   
}

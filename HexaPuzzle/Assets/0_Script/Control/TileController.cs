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

        distanceX = tileSize * 1.5f; // ��� ���� ����
        distanceY = tileSize / 4 * Mathf.Sqrt(3); // ��� ���� ���� ( 1 : ��3 : 2 )

        startPosition = Vector2.zero;
        maxTileCount = (mapSizeY / 2) * (mapSizeX / 2 + 1) + (mapSizeY / 2 + 1) * (mapSizeX / 2);
    }

    void Start()
    {
        // ĵ���� ����� ����Ͽ� �߾ӿ� ��ġ�ϵ��� ����
        Vector2 mapTotlaSize = new Vector2(tileSize + distanceX * (mapSizeX / 2), distanceY * (mapSizeY / 2 + 1));
        Vector2 cameraSize = TileMapParent.parent.GetComponent<RectTransform>().sizeDelta;
        startPosition = new Vector2(tileSize / 2 + cameraSize.x / 2 - mapTotlaSize.x / 2, distanceY + cameraSize.y / 2 - mapTotlaSize.y);

        Initialize();
    }

    void Initialize()
    {
        // Ÿ�� ����
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
            // �� Ÿ�� ����
            for (int i = 0; i < stageData.Maplist.Length; i++)
            {
                TileList[stageData.Maplist[i]].gameObject.SetActive(true);
                TileList[stageData.Maplist[i]].NowBlock = BlockController.Instance.CreateBlock(TileList[stageData.Maplist[i]].transform.position, (BLOCK_TYPE)stageData.Blocklist[i]);
            }

            // Ÿ�� Ÿ�� ����
            for (int i = 0; i < stageData.Createblocktile.Length; i++)
            {
                TileList[stageData.Createblocktile[i]].TileType = TILE_TYPE.CREATE;
            }

        }

    }

    // Ÿ�� ����
    void CreateTile(Vector2 _tilePosition)
    {
        GameObject newTileObject = Instantiate(tilePrefab, _tilePosition, Quaternion.identity, TileMapParent);
        newTileObject.SetActive(false);

        Tile newTile = newTileObject.GetComponent<Tile>();
        newTile.ID = tileCount++; // Ÿ�� ID ���� (���ϴܺ��� ������� ����)

        TileList.Add(newTile);
    }

    public bool CheckTileList()
    {
        bool isMoved = false;

        for (int i = 0; i < TileList.Count; i++)
        {
            if (TileList[i].NowBlock) // ����� �ִ� ���
            {
                if (CheckTile(TileList[i])) // �̵� ������ ������ �ִ��� �Ǵ�
                {
                    isMoved = true; // �� ���̶� ���������� true
                }

            }
            else if (TileList[i].TileType == TILE_TYPE.CREATE) // ��Ͼ��� ����Ÿ�� = ��� ����
            {
                BLOCK_TYPE randomBlock = BlockController.Instance.NowBlockType[Random.Range(0,BlockController.Instance.NowBlockType.Count)];
                TileList[i].NowBlock = BlockController.Instance.CreateBlock(TileList[i].transform.position, randomBlock);
                TileList[i].NowBlock.MoveSetting(new Vector2(TileList[i].transform.position.x, TileList[i].transform.position.y + distanceY), TileList[i].transform.position);
                isMoved = true; // ���������� true
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
            if (TileList[topID].NowBlock) return false; // �� ���� ���� ������ �ѱ�
        }

        switch (Random.Range(0, 2)) // ��/�� �����ϰ� ������
        {
            case 0:
                if (bottomLeftID != -1 && TileList[bottomLeftID].gameObject.activeSelf)
                {
                    if (TileList[bottomLeftID].NowBlock == false && TileList[topLeftID].NowBlock == false) // ���� �Ʒ�/�� ���� ����.
                    {
                        BlockMoveSetting(_checkTile, TileList[bottomLeftID]);
                        return true;
                    }
                }
                if (bottomRightID != -1 && TileList[bottomRightID].gameObject.activeSelf)
                {
                    if (TileList[bottomRightID].NowBlock == false && TileList[topRightID].NowBlock == false) // ������ �Ʒ�/�� ���� ����.
                    {
                        BlockMoveSetting(_checkTile, TileList[bottomRightID]);
                        return true;
                    }
                }
                break;
            case 1:
                if (bottomRightID != -1 && TileList[bottomRightID].gameObject.activeSelf)
                {
                    if (TileList[bottomRightID].NowBlock == false && TileList[topRightID].NowBlock == false) // ������ �Ʒ�/�� ���� ����.
                    {
                        BlockMoveSetting(_checkTile, TileList[bottomRightID]);
                        return true;
                    }
                }
                if (bottomLeftID != -1 && TileList[bottomLeftID].gameObject.activeSelf)
                {
                    if (TileList[bottomLeftID].NowBlock == false && TileList[topLeftID].NowBlock == false) // ���� �Ʒ�/�� ���� ����.
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
    /// �̵� ������ ����� �̵���Ű�� �޼ҵ�
    /// </summary>
    /// <param name="_tile">�̵� ������ ����� ���Ե� Tile</param>
    /// <param name="_nextTile">����� �̵��� Tile</param>
    public void BlockMoveSetting(Tile _tile, Tile _nextTile)
    {
        Vector2 startPosition = _tile.NowBlock.transform.position;
        Vector2 endPosition = _nextTile.transform.position;

        _nextTile.NowBlock = _tile.NowBlock;
        _tile.NowBlock = null;

        _nextTile.NowBlock.MoveSetting(startPosition, endPosition);
    }


   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TILE_TYPE
{
    NORMAL = 0,
    CREATE,
}

public enum TILE_AROUND
{
    TOP = 0,
    TOP_LEFT,
    TOP_RIGHT,
    BOTTOM,
    BOTTOM_LEFT,
    BOTTOM_RIGHT
}

public class Tile : MonoBehaviour
{
    [SerializeField] TILE_TYPE tileType;
    public TILE_TYPE TileType { get { return tileType; } set { tileType = value; } }

    [SerializeField] int id;
    public int ID { get { return id; } set { id = value; SetAroundBlock(); } }

    [SerializeField] int[] tileAround;
    public int[] TileAround { get { return tileAround; } set { tileAround = value; } } 

    void Awake()
    {
        tileAround = new int[6] { -1, -1, -1, -1, -1, -1 };
    }

    Block nowBlock;
    public Block NowBlock { get { return nowBlock; } set { nowBlock = value; } }

    void SetAroundBlock()
    {
        int maxSizeX = TileController.Instance.MaxSizeX;
        if (ID % maxSizeX == maxSizeX/2) // ���� ����
        {
            TileAround[(int)TILE_AROUND.TOP] = CheckIDValue(ID + (maxSizeX));
            TileAround[(int)TILE_AROUND.TOP_RIGHT] = CheckIDValue(ID + (maxSizeX / 2 + 1));
            TileAround[(int)TILE_AROUND.BOTTOM] = CheckIDValue( ID - (maxSizeX));
            TileAround[(int)TILE_AROUND.BOTTOM_RIGHT] = CheckIDValue( ID - (maxSizeX / 2));    
        }
        else if(ID % maxSizeX == 8) // ���� ����
        {
            TileAround[(int)TILE_AROUND.TOP] = CheckIDValue( ID + (maxSizeX));
            TileAround[(int)TILE_AROUND.TOP_LEFT] = CheckIDValue( ID + (maxSizeX / 2));
            TileAround[(int)TILE_AROUND.BOTTOM] = CheckIDValue( ID - (maxSizeX));
            TileAround[(int)TILE_AROUND.BOTTOM_LEFT] = CheckIDValue( ID - (maxSizeX / 2 + 1));
        }
        else // �Ϲ�
        {
            TileAround[(int)TILE_AROUND.TOP] = CheckIDValue( ID + (maxSizeX));                  // +9
            TileAround[(int)TILE_AROUND.TOP_LEFT] = CheckIDValue( ID + (maxSizeX / 2));         // +4
            TileAround[(int)TILE_AROUND.TOP_RIGHT] = CheckIDValue( ID + (maxSizeX / 2 + 1));    // +5
            TileAround[(int)TILE_AROUND.BOTTOM] = CheckIDValue( ID - (maxSizeX));               // -9
            TileAround[(int)TILE_AROUND.BOTTOM_LEFT] = CheckIDValue( ID - (maxSizeX / 2 + 1));  // -5
            TileAround[(int)TILE_AROUND.BOTTOM_RIGHT] = CheckIDValue( ID - (maxSizeX / 2));     // -4
        }
    }

    int CheckIDValue(int _id) // ��,�Ʒ� üũ
    {
        if(_id < 0 || TileController.Instance.MaxTileCount <= _id)
        {
            return -1;
        }
        return _id;
    }
}

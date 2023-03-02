using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TILE_TYPE
{
    NORMAL = 0,
    CREATE,
}

public enum TRAP_AROUND
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

    [SerializeField] int[] trapAround;
    public int[] TrapAround { get { return trapAround; } set { trapAround = value; } } 

    void Awake()
    {
        trapAround = new int[6] { -1, -1, -1, -1, -1, -1 };
    }

    //[SerializeField] int top = -1;
    //public int Top { get { return top; } set { top = CheckIDValue(value); } }
    //[SerializeField] int topLeft = -1;
    //public int TopLeft { get { return topLeft; } set { topLeft = CheckIDValue(value); } }
    //[SerializeField] int topRight = -1;
    //public int TopRight { get { return topRight; } set { topRight = CheckIDValue(value); } }
    //[SerializeField] int bottom = -1;
    //public int Bottom { get { return bottom; } set { bottom = CheckIDValue(value); } }
    //[SerializeField] int bottomLeft = -1;
    //public int BottomLeft { get { return bottomLeft; } set { bottomLeft = CheckIDValue(value); } }
    //[SerializeField] int bottomRight = -1;
    //public int BottomRight { get { return bottomRight; } set { bottomRight = CheckIDValue(value); } }

    Block nowBlock;
    public Block NowBlock { get { return nowBlock; } set { nowBlock = value; } }

    bool isActive;
    public bool IsActive { get { return isActive; } set { isActive = value; } }

    void SetAroundBlock()
    {
        int maxSizeX = TileController.Instance.MaxSizeX;
        if (ID % maxSizeX == maxSizeX/2) // 가장 좌측
        {
            TrapAround[(int)TRAP_AROUND.TOP] = CheckIDValue(ID + (maxSizeX));
            TrapAround[(int)TRAP_AROUND.TOP_RIGHT] = CheckIDValue(ID + (maxSizeX / 2 + 1));
            TrapAround[(int)TRAP_AROUND.BOTTOM] = CheckIDValue( ID - (maxSizeX));
            TrapAround[(int)TRAP_AROUND.BOTTOM_RIGHT] = CheckIDValue( ID - (maxSizeX / 2));    
        }
        else if(ID % maxSizeX == 8) // 가장 우측
        {
            TrapAround[(int)TRAP_AROUND.TOP] = CheckIDValue( ID + (maxSizeX));
            TrapAround[(int)TRAP_AROUND.TOP_LEFT] = CheckIDValue( ID + (maxSizeX / 2));
            TrapAround[(int)TRAP_AROUND.BOTTOM] = CheckIDValue( ID - (maxSizeX));
            TrapAround[(int)TRAP_AROUND.BOTTOM_LEFT] = CheckIDValue( ID - (maxSizeX / 2 + 1));
        }
        else // 일반
        {
            TrapAround[(int)TRAP_AROUND.TOP] = CheckIDValue( ID + (maxSizeX));                  // +9
            TrapAround[(int)TRAP_AROUND.TOP_LEFT] = CheckIDValue( ID + (maxSizeX / 2));         // +4
            TrapAround[(int)TRAP_AROUND.TOP_RIGHT] = CheckIDValue( ID + (maxSizeX / 2 + 1));    // +5
            TrapAround[(int)TRAP_AROUND.BOTTOM] = CheckIDValue( ID - (maxSizeX));               // -9
            TrapAround[(int)TRAP_AROUND.BOTTOM_LEFT] = CheckIDValue( ID - (maxSizeX / 2 + 1));  // -5
            TrapAround[(int)TRAP_AROUND.BOTTOM_RIGHT] = CheckIDValue( ID - (maxSizeX / 2));     // -4
        }
    }

    int CheckIDValue(int _id) // 위,아래 체크
    {
        if(_id < 0 || TileController.Instance.MaxTileCount <= _id)
        {
            return -1;
        }
        return _id;
    }

    void FallingBlock()
    {

    }
}

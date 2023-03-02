using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] int id;
    public int ID { get { return id; } set { id = value; SetAroundBlock(); } }

    [SerializeField] int top = -1;
    public int Top { get { return top; } set { top = CheckIDValue(value); } }
    [SerializeField] int topLeft = -1;
    public int TopLeft { get { return topLeft; } set { topLeft = CheckIDValue(value); } }
    [SerializeField] int topRight = -1;
    public int TopRight { get { return topRight; } set { topRight = CheckIDValue(value); } }
    [SerializeField] int bottom = -1;
    public int Bottom { get { return bottom; } set { bottom = CheckIDValue(value); } }
    [SerializeField] int bottomLeft = -1;
    public int BottomLeft { get { return bottomLeft; } set { bottomLeft = CheckIDValue(value); } }
    [SerializeField] int bottomRight = -1;
    public int BottomRight { get { return bottomRight; } set { bottomRight = CheckIDValue(value); } }

    Block nowBlock;
    public Block NowBlock { get { return nowBlock; } set { nowBlock = value; } }

    void SetAroundBlock()
    {
        int maxSizeX = TileController.Instance.MaxSizeX;
        if (ID % maxSizeX == maxSizeX/2 + 1) // ���� ����
        {
            Top = ID + maxSizeX;
            TopRight = ID + maxSizeX / 2 + 1;
            Bottom = ID - maxSizeX;
            BottomRight = ID - maxSizeX / 2;
        }
        else if(ID % maxSizeX == 0) // ���� ����
        {
            Top = ID + maxSizeX;
            TopLeft = ID + maxSizeX / 2;
            Bottom = ID - maxSizeX;
            BottomLeft = ID - maxSizeX / 2 + 1;
        }
        else // �Ϲ�
        {
            Top = ID + maxSizeX;
            TopLeft = ID + maxSizeX / 2;
            TopRight = ID + maxSizeX / 2 + 1;
            Bottom = ID - maxSizeX;
            BottomLeft = ID - maxSizeX / 2 + 1;
            BottomRight = ID - maxSizeX / 2;
        }
    }

    int CheckIDValue(int _id) // ��,�Ʒ� üũ
    {
        if(_id < 1 || TileController.Instance.MaxTileCount < _id)
        {
            return -1;
        }
        return _id;
    }

    void FallingBlock()
    {

    }
}

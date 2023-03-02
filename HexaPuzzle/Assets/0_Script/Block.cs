using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BLOCK_TYPE
{
    BLUE = 0,
    GREEN,
    ORANGE,
    PUPPLE,
    RED,
    YELLOW
}

public class Block : MonoBehaviour
{
    BLOCK_TYPE blockType;

    Queue<int> moveTile = new Queue<int>();
    public Queue<int> MoveTile { get { return moveTile; } set { moveTile = value; } }

    Block(BLOCK_TYPE _type)
    {
        blockType = _type;
    }

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        switch (blockType)
        {
            case BLOCK_TYPE.BLUE:
                break;
            case BLOCK_TYPE.GREEN:
                break;
            case BLOCK_TYPE.ORANGE:
                break;
            case BLOCK_TYPE.PUPPLE:
                break;
            case BLOCK_TYPE.RED:
                break;
            case BLOCK_TYPE.YELLOW:
                break;
            default:
                break;
        }
    }
}

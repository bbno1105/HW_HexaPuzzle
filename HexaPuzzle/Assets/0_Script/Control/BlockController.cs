using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : SingletonBehaviour<BlockController>
{
    // 블럭 기본
    [SerializeField] List<Block> blockPool = new List<Block>();
    Queue<Block> blockPooling = new Queue<Block>();

    List<BLOCK_TYPE> nowBlockType = new List<BLOCK_TYPE>();
    public List<BLOCK_TYPE> NowBlockType { get { return nowBlockType; } set { nowBlockType = value; } }

    // 블럭 이동
    Tile nowTouchTile = null;
    public Tile NowTouchTile { get { return nowTouchTile; } set { nowTouchTile = value; } }
    Tile targetTile = null;
    public Tile TargetTile { get { return targetTile; } set { targetTile = value; } }

    bool isTouchBlock = false;
    public bool IsTouchBlock { get { return isTouchBlock; } set { isTouchBlock = value; } }

    float dragDistance;
    float checkDistance;


    void Awake()
    {
        if (StaticData.StageData.TryGetValue(21, out StageSheetData stageData))
        {
            for (int i = 0; i < stageData.Blcoktype.Length; i++)
            {
                NowBlockType.Add((BLOCK_TYPE)stageData.Blcoktype[i]);
            }
        }
    }

    void Start()
    {
        BlockPool();

        checkDistance = TileController.Instance.DistanceY * 2;
    }

    public void BlockPool()
    {
        for (int i = 0; i < blockPool.Count; i++)
        {
            if(blockPool[i].gameObject.activeSelf == false)
            {
                blockPooling.Enqueue(blockPool[i]);
            }
        }
    }

    public Block CreateBlock(Vector3 _position, BLOCK_TYPE _type)
    {
        if(blockPooling.Count == 0)
        {
            BlockPool();
        }

        Block newBlock = blockPooling.Dequeue();
        newBlock.BlockType = _type;
        newBlock.gameObject.transform.position = _position;
        newBlock.gameObject.SetActive(true);

        return newBlock;
    }

    // 블럭 이동
    void Update()
    {
        if (isTouchBlock)
        {
            MoveBlock();
        }
    }

    bool MoveBlock()
    {
        Vector2 startPosition = NowTouchTile.transform.position;
        Vector2 newPosition = Input.mousePosition;

        dragDistance = Vector3.Distance(newPosition, startPosition);

        if (checkDistance < dragDistance)
        {
            Vector2 normalVector = (newPosition - startPosition).normalized;
            IsTouchBlock = false;

            if (0 < normalVector.y) // 상
            {
                if (normalVector.x < -1 / 3f) // 좌
                {
                    TargetTile = TileController.Instance.TileList[NowTouchTile.TileAround[(int)TILE_AROUND.TOP_LEFT]];
                }
                else if (1 / 3f < normalVector.x) // 우
                {
                    TargetTile = TileController.Instance.TileList[NowTouchTile.TileAround[(int)TILE_AROUND.TOP_RIGHT]];
                }
                else // 중앙
                {
                    TargetTile = TileController.Instance.TileList[NowTouchTile.TileAround[(int)TILE_AROUND.TOP]];
                }

            }
            else // 하
            {
                if (normalVector.x < -1 / 3f) // 좌
                {
                    TargetTile = TileController.Instance.TileList[NowTouchTile.TileAround[(int)TILE_AROUND.BOTTOM_LEFT]];
                }
                else if (1 / 3f < normalVector.x) // 우
                {
                    TargetTile = TileController.Instance.TileList[NowTouchTile.TileAround[(int)TILE_AROUND.BOTTOM_RIGHT]];
                }
                else // 중앙
                {
                    TargetTile = TileController.Instance.TileList[NowTouchTile.TileAround[(int)TILE_AROUND.BOTTOM]];
                }
            }
            TileController.Instance.BlockSwap(NowTouchTile, TargetTile);
            PlayController.Instance.PlayState = PLAYSTATE.MOVEBLOCK;
        }

        return false;
    }

    public void RemoveBlock()
    {
        TileController.Instance.BlockSwap(TargetTile, NowTouchTile);
        //NowTouchTile = null;
        //TargetTile = null;
    }
}
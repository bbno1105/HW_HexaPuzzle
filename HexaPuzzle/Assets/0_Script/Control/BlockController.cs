using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : SingletonBehaviour<BlockController>
{
    [SerializeField] List<Block> blockPool = new List<Block>();
    Queue<Block> blockPooling = new Queue<Block>();

    List<BLOCK_TYPE> nowBlockType = new List<BLOCK_TYPE>();
    public List<BLOCK_TYPE> NowBlockType { get { return nowBlockType; } set { nowBlockType = value; } }

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
}

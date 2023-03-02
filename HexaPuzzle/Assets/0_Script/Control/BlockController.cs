using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : SingletonBehaviour<BlockController>
{
    [SerializeField] List<Block> blockPool = new List<Block>();
    Queue<Block> blockPooling = new Queue<Block>();

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

    public Block CreateBlock(Vector3 _position)
    {
        if(blockPooling.Count == 0)
        {
            BlockPool();
        }

        Block newBlock = blockPooling.Dequeue();
        newBlock.BlockType = (BLOCK_TYPE)Random.Range(0, 6); // 랜덤생성 // TODO : 나중에 계산해서 넣기
        newBlock.gameObject.transform.position = _position;
        newBlock.gameObject.SetActive(true);

        return newBlock;
    }

    public void CheckBlock(Block _block)
    {
        for (int i = 0; i < 6; i++)
        {

        }
    }
}

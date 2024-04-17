using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class BlockFactory
{
    public static Block SpawnBlock(BlockType blockType, int blockNumber)
    {
        Block block = new Block(blockType, blockNumber);

        //Set Breed
        if(blockType == BlockType.EMPTY)
           block.number = 0;
        return block;
    }
}
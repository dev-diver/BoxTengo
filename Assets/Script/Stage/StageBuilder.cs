using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StageBuilder
{
    int m_nStage;
    StageInfo m_StageInfo;
    public StageBuilder(int nStage){
        m_nStage = nStage;
    }

    public Stage ComposeStage(){

        Debug.Assert(m_nStage > 0, $"Invalide Stage : {m_nStage}");
 
        //0. 스테이지 정보를 로드한다.(보드 크기, Cell/블럭 정보 등)
        m_StageInfo = LoadStage(m_nStage);
 
        //1. Stage 객체를 생성한다.
        Stage stage = new Stage(this, m_StageInfo.row, m_StageInfo.col);

        //2. Cell,Block 초기 값을 생성한다.
        for(int nRow = 0; nRow < m_StageInfo.row; nRow++)
        {
            for (int nCol = 0; nCol < m_StageInfo.col; nCol++)
            {
                stage.blocks[nRow, nCol] = SpawnBlockForStage(nRow, nCol);
                stage.cells[nRow,nCol] = SpawnCellForStage(nRow, nCol);
            }
        }

        return stage;
    }

    public StageInfo LoadStage(int nStage)   //추가 메소드
    {
        StageInfo stageInfo = StageReader.LoadStage(nStage);
        if (stageInfo != null)
        {
            Debug.Log(stageInfo.ToString());
        }
 
        return stageInfo;
    }

    Block SpawnBlockForStage(int nRow, int nCol)
    {
        //data에서 가져오기
        BlockType type = m_StageInfo.GetBlockType(nRow, nCol);
        if (type == BlockType.EMPTY)
            return SpawnEmptyBlock();

        return SpawnBlock(type, m_StageInfo.GetBlockNumber(nRow, nCol));
    }

    Cell SpawnCellForStage(int nRow, int nCol)
    {
        //data에서 가져오기
        Debug.Assert(m_StageInfo != null);
        Debug.Assert(nRow < m_StageInfo.row && nCol < m_StageInfo.col);
 
        return CellFactory.SpawnCell(m_StageInfo, nRow, nCol);
    }

    public static Stage BuildStage(int nStage)
    {
        StageBuilder stageBuilder = new StageBuilder(nStage);
        Stage stage = stageBuilder.ComposeStage();

        return stage;
    }

    public Block SpawnBlock(BlockType type, int number)
    {
        return BlockFactory.SpawnBlock(type, number);
    }

    public Block SpawnEmptyBlock()
        {
            Block newBlock = BlockFactory.SpawnBlock(BlockType.EMPTY, 0);

            return newBlock;
        }
}

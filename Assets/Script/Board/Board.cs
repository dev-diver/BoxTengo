using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Board
{
    int m_nRow;
    int m_nCol;

    public int maxRow {get {return m_nRow;}}
    public int maxCol {get {return m_nCol;}}

    Cell[,] m_Cells;
    public Cell[,] cells {get {return m_Cells;}}

    Block[,] m_Blocks;
    public Block[,] blocks {get {return m_Blocks;}}

    public Board(int nRow, int nCol)
    {
        m_nRow = nRow;
        m_nCol = nCol;

        m_Cells = new Cell[nRow, nCol];
        m_Blocks = new Block[nRow, nCol];
    }

    Transform m_Container;
    GameObject m_CellPrefab;
    GameObject m_BlockPrefab;

    internal void ComposeStage(GameObject cellPrefab, GameObject blockPrefab, Transform container)
    {
        //1. 스테이지 구성에 필요한 Cell,Block, Container(Board) 정보를 저장한다.
        m_CellPrefab = cellPrefab;
        m_BlockPrefab = blockPrefab;
        m_Container = container;
 
        //2. Cell, Block Prefab을 이용해서 Board에 Cell/Block GameObject를 추가한다.
        for (int nRow = 0; nRow < m_nRow; nRow++)
            for (int nCol = 0; nCol < m_nCol; nCol++)
            {
                Cell cell = m_Cells[nRow, nCol]?.InstantiateCellObj(cellPrefab, container);
                cell?.Move(CalXPos(container, nCol), CalYPos(container, nRow));
            }
        
        for (int nRow = 0; nRow < m_nRow; nRow++)
            for (int nCol = 0; nCol < m_nCol; nCol++)
            {
                Block block = m_Blocks[nRow, nCol]?.InstantiateBlockObj(blockPrefab, container, nRow, nCol);
                block?.Move(CalXPos(container, nCol), CalYPos(container, nRow));
            }
    }

    public float CalXPos(Transform container, float x)
    {
        float boardAmount = 8;
        float Width = container.lossyScale.x;
        float halfObjScale = Width/boardAmount/2;
        float result = remap(x, 0,m_nCol-1,-halfObjScale*(m_nCol-1),halfObjScale*(m_nCol-1));
        return result;
    }

    public float CalYPos(Transform container, float y)
    {
        float boardAmount = 8;
        float Height = container.lossyScale.y;
        float halfObjScale = Height/boardAmount/2;

        return remap(y, 0,m_nRow-1,-halfObjScale*(m_nRow-1),halfObjScale*(m_nRow-1));
    }

    public static float remap(float val, float in1, float in2, float out1, float out2)  //리맵하는 함수
    {
        return out1 + (val - in1) * (out2 - out1) / (in2 - in1);
    }

}

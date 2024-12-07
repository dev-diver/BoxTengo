using System.Text;
using UnityEngine;

public class Stage
{
    public int maxRow { get { return m_Board.maxRow; } }
    public int maxCol { get { return m_Board.maxCol; } }

    Board m_Board;
    public Board board { get { return m_Board; } }

    StageBuilder m_StageBuilder;
    public Block[,] blocks { get { return m_Board.blocks; } }
    public Cell[,] cells { get { return m_Board.cells; } }

    public Stage(StageBuilder stageBuilder, int nRow, int nCol)
    {
        m_StageBuilder = stageBuilder;
        m_Board = new Board(nRow, nCol);
    }

    internal void ComposeStage(GameObject cellPrefab, GameObject blockPrefab, Transform container)
    {
        m_Board.ComposeStage(cellPrefab, blockPrefab, container);
    }
    public void PrintAll()
    {
        StringBuilder strCells = new StringBuilder();
        StringBuilder strBlocks = new StringBuilder();

        for (int nRow = maxRow - 1; nRow >= 0; nRow--)
        {
            for (int nCol = 0; nCol < maxCol; nCol++)
            {
                strCells.Append($"{cells[nRow, nCol].type}, ");
                strBlocks.Append($"{blocks[nRow, nCol].type}, ");
            }

            strCells.Append("\n");
            strBlocks.Append("\n");
        }
        Debug.Log(strCells.ToString());
        Debug.Log(strBlocks.ToString());
    }

    public int leftBlocks()
    {
        int blockAmount = 0;
        for (int nRow = maxRow - 1; nRow >= 0; nRow--)
        {
            for (int nCol = 0; nCol < maxCol; nCol++)
            {
                if (blocks[nRow, nCol] != null)
                {
                    blockAmount++;
                }
            }
        }
        return blockAmount;
    }
}



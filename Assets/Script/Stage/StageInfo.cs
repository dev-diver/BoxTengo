using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageInfo
{
    public int row;
    public int col;
    public int[] cells;
    public int[] blockTypes;
    public int[] blockNumbers;
    

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }

    public CellType GetCellType(int nRow, int nCol)
    {
        if(cells.Length==0) return CellType.EMPTY;
        Debug.Assert(cells != null && cells.Length > nRow * col + nCol);
    
        if (cells.Length > nRow * col + nCol)
            return (CellType)cells[nRow * col + nCol];
    
        Debug.Assert(false);
    
        return CellType.EMPTY;
    }

    public int GetBlockNumber(int nRow, int nCol)
    {
        Debug.Assert(blockNumbers != null && blockNumbers.Length > nRow * col + nCol);
    
        if (blockNumbers.Length > nRow * col + nCol)
            return (int)blockNumbers[nRow * col + nCol];
    
        Debug.Assert(false);
    
        return 0;
    }

    public BlockType GetBlockType(int nRow, int nCol)
    {
        if(blockTypes.Length==0) return BlockType.SETTING;
        Debug.Assert(blockTypes != null && blockTypes.Length > nRow * col + nCol);
    
        if (blockTypes.Length > nRow * col + nCol)
            return (BlockType)blockTypes[nRow * col + nCol];
    
        Debug.Assert(false);
    
        return BlockType.EMPTY;
    }
    
    public bool DoValidation()
    {
        Debug.Assert(blockNumbers.Length == row * col);
        Debug.Log($"cell length : {cells.Length}, row, col = ({row}, {col})");
    
        if (blockNumbers.Length != row * col)
            return false;
    
        return true;
    }
}
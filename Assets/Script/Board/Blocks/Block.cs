using UnityEngine;
public class Block
{
    BlockType m_BlockType;
    public BlockType type
    {
        get { return m_BlockType; }
        set { m_BlockType = value; }
    }

    protected int m_Number;   //렌더링되는 블럭 캐린터(즉, 이미지 종류)
    public int number
    {
        get { return m_Number; }
        set
        {
            m_Number = value;
            m_BlockBehaviour?.UpdateView(true);
        }
    }

    protected BlockBehaviour m_BlockBehaviour;
    public BlockBehaviour blockBehaviour
    {
        get { return m_BlockBehaviour; }
        set
        {
            m_BlockBehaviour = value;
            m_BlockBehaviour.SetBlock(this);
        }
    }

    public Vector2Int pos;

    public Block(BlockType blockType, int blockNumber)
    {
        m_BlockType = blockType;
        m_Number = blockNumber;
    }

    internal Block InstantiateBlockObj(GameObject blockPrefab, Transform containerObj, int row, int col)
    {
        //유효하지 않은 블럭인 경우, Block GameObject를 생성하지 않는다.
        if (IsValidate() == false)
            return null;

        //1. Block 오브젝트를 생성한다.
        GameObject newObj = Object.Instantiate(blockPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        //2. 컨테이너(Board)의 차일드로 Block을 포함시킨다.
        newObj.transform.parent = containerObj;

        //3. Block 오브젝트에 적용된 BlockBehaviour 컴포너트를 보관한다.
        this.blockBehaviour = newObj.transform.GetComponent<BlockBehaviour>();
        pos = new Vector2Int(row,col);
        return this;
    }

    internal void Move(float x, float y)
    {
        float boardAmount = 8;
        
        float localBlockScale = (float)1/boardAmount;
        blockBehaviour.transform.localScale = new Vector2(localBlockScale, localBlockScale);
        blockBehaviour.transform.position = new Vector3(x, y);
    }

    public bool IsValidate()
    {
        return type != BlockType.EMPTY;
    }

    
}

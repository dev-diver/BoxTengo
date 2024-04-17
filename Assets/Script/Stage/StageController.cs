using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageController : MonoBehaviour
{
    [SerializeField] Transform m_Container;
    [SerializeField] GameObject m_CellPrefab;
    [SerializeField] GameObject m_BlockPrefab;
    [SerializeField] GameObject m_StagePrefab;
    Stage m_Stage;
    [SerializeField] GameObject menuBoard;
    [SerializeField] Button nextBtn;
    [SerializeField] Button prevBtn;
    
    [SerializeField] TextMeshProUGUI stageText;
    [SerializeField] TextMeshProUGUI pageText;
    int stageNumber; //실행하는 스테이지
    int lastOpenStage; //최종 잠금해제 스테이지
    int page;

    int totalPages;
    
    [SerializeField] DrawSelection drawSelection;
    
    private void Start() {
        totalPages = 10;
        lastOpenStage = PlayerPrefs.GetInt("lastOpenStage",1);
        showStageBoard();
    }

    void makeStageBoard(int page)
    {
        clearMenuBoard();
        Transform board = menuBoard.transform.GetChild(1);
        
        for(int i = 1; i <= 10; i++)
        {
            Transform stageParent = board.transform.GetChild(i-1);
            GameObject newObj = Instantiate(m_StagePrefab, Vector2.zero, Quaternion.identity,stageParent);
            RectTransform go =newObj.GetComponent<RectTransform>();
            SetPosition(go);
            Btn_StageNumber btn = newObj.GetComponent<Btn_StageNumber>();
            int thisStage = page*10+i;
            btn.number = thisStage;
            if(lastOpenStage < thisStage)
            {
                btn.state = StageBtnType.ROCK;
            }
            else if(lastOpenStage > thisStage)
            {
                btn.state = StageBtnType.SOLVE;
            }
            else
            {
                btn.state = StageBtnType.UNLOCK;
            }
            
        }
        menuBoard.SetActive(true);

        if(page==0)
        {
            prevBtn.interactable=false;
        }
        else if(page==totalPages-1)
        {
            nextBtn.interactable=false;
        }
        else
        {
            prevBtn.interactable=true;
            nextBtn.interactable=true;
        }

        pageText.text = page+1 + "/" + totalPages;
    }

    void clearMenuBoard()
    {
        Transform board = menuBoard.transform.GetChild(1);
        foreach(Transform child in board)
        {
            Debug.Log(child.gameObject.name);
            if(child.childCount!=0)
            {
                Destroy(child.GetChild(0).gameObject);
            }
        }
    }

    private void SetPosition(RectTransform go)
    {
        go.localPosition = new Vector3(0,0,0);
        go.localRotation = Quaternion.identity;
        go.localScale = new Vector3(1,1,1);
    }

    public void makeNextStageBoard()
    {
        page++;
        makeStageBoard(page);
        Debug.Log("Next");
    }

    public void makePrevStageBoard()
    {
        page--;
        makeStageBoard(page);
        Debug.Log("Prev");
    }
    public void InitStage(int number)
    {
        clearStageBlocks();

        stageNumber = number;
        stageText.text = "STAGE " + stageNumber;
        BuildStage(stageNumber);
        m_Stage.PrintAll();
        menuBoard.SetActive(false);
    }

    public void restartStage()
    {
        clearStageBlocks();
        BuildStage(stageNumber);
    }

    void clearStageBlocks()
    {
        foreach(Transform child in m_Container.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void BuildStage(int number)
    {
        m_Stage = StageBuilder.BuildStage(nStage : number);
        m_Stage.ComposeStage(m_CellPrefab, m_BlockPrefab, m_Container);
    }

    public int leftBlocks()
    {
        int blockAmount = 0;
        for (int nRow = m_Stage.maxRow -1; nRow >=0; nRow--)
        {
            for (int nCol = 0; nCol < m_Stage.maxCol; nCol++)
            {
                if(m_Stage.blocks[nRow,nCol]!=null)
                {
                    blockAmount++;
                }
            }
        }

        Debug.Log(blockAmount);
        return blockAmount;
    }

    public void BoxingOut(int row, int col)
    {
        m_Stage.blocks[row,col] = null;
    }

    public void nextStage()
    {
        stageNumber+=1;
        if(lastOpenStage<stageNumber)
        {
            lastOpenStage = stageNumber;
            PlayerPrefs.SetInt("lastOpenStage",lastOpenStage);
        }        
        InitStage(stageNumber);
    }

    public void showStageBoard()
    {
        page = lastOpenStage/10;
        Debug.Log("Page: "+ page);
        makeStageBoard(page);
    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Btn_StageNumber : MonoBehaviour
{
    public int number;
    public StageBtnType state;
    
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] StageConfig m_StageConfig;

    Image imageComp;
    StageController stageController;

    private void Start() {
        stageController = GameObject.Find("GameBoard").GetComponent<StageController>();
        imageComp = GetComponent<Image>();
        imageComp.sprite = m_StageConfig.stageSelectSprites[(int)state];
        if(state==StageBtnType.ROCK)
        {
            text.gameObject.SetActive(false);
            GetComponent<Button>().interactable=false;
        }
        text.text = number.ToString();
    }
    public void onClickBtn()
    {
        stageController.InitStage(number);
        Debug.Log(number);
    }


}

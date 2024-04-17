using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] Button shuffleButton;
    [SerializeField] Button restartButton;
    [SerializeField] Button tutoBtn;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI bestScoreText;
    [SerializeField] TextMeshProUGUI timeText;

    [Header("Board")]
    [SerializeField] GameObject gameStartMessage;
    [SerializeField] GameObject gameOverMessage;
    [SerializeField] GameObject bestScoreMessage;
    [SerializeField] preCountDowner preCountDownScreen;

    [Header("Popup")]
    [SerializeField] GameObject PopupAskContinue;
    [SerializeField] GameObject PopupAskCoin;

    public static UIManager Instance;
    private void OnEnable()
    {
        GameManager.BeforeStart += showStartBtn;
        GameManager.BeforeStart += UIFreezeExceptRestart;

        GameManager.GameSet += closeAllPopupAndBoard;
        GameManager.PreCount += UIFreezeExceptRestart;
        GameManager.GameStart += UIUnFreeze;

        GameManager.GameOver += showGameOverMessage;

        GameManager.scoreChange += scoreChange;   
        GameManager.bestScoreChange += bestScoreChange;
        GameManager.timeChange += timeChange;

        GameManager.timeBonus += timeBonus;
    }

    private void OnDisable()
    {
        GameManager.BeforeStart -= showStartBtn;
        GameManager.BeforeStart += UIFreezeExceptRestart;

        GameManager.GameSet -= closeAllPopupAndBoard;
        GameManager.PreCount -= UIFreezeExceptRestart;
        GameManager.GameStart -= UIUnFreeze;

        GameManager.GameOver -= showGameOverMessage;

        GameManager.scoreChange -= scoreChange;   
        GameManager.bestScoreChange -= bestScoreChange;
        GameManager.timeChange -= timeChange;

        GameManager.timeBonus -= timeBonus;
    }

    private void Start()
    {
        if(PlayerPrefs.GetInt("tuto",1)!=0)
        {
            Debug.Log("Tutorial Open");
            tutoBtn.onClick.Invoke();
            PlayerPrefs.SetInt("tuto",0);
        }
    }

    void scoreChange(float score)
    {
       scoreText.text = "Score: " + score;
    }

    void bestScoreChange(float score)
    {
        bestScoreText.text ="Your Best:" + score;
    }

    void timeChange(float lastTime)
    {
        timeText.text = "Time: " + string.Format ("{0:N2}", lastTime);
    }
    void timeBonus()
    {
        timeText.gameObject.GetComponent<Animator>().SetTrigger("light");
    }

    void UIFreeze()
    {
        restartButton.interactable = false;
    }

    void UIFreezeExceptRestart(){

        UIFreeze();
        restartButton.interactable = true;
    }

    void UIUnFreeze()
    {
        shuffleButton.interactable = true;
        restartButton.interactable = true;
    }

    void closeAllPopupAndBoard()
    {
        PopupAskContinue.SetActive(false);
        PopupAskCoin.SetActive(false);
        gameStartMessage.SetActive(false);
        gameOverMessage.SetActive(false);
        bestScoreMessage.SetActive(false);
        preCountDownScreen.turnOff();
    }

    void showStartBtn()
    {
        gameStartMessage.SetActive(true);
    }

    void showGameOverMessage()
    {
        gameOverMessage.SetActive(true);
    }

}

using System;
using System.Collections.Generic;
using UnityEngine;


public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;
    GameManager gameManager;
    public int coin;
    [SerializeField] float comboTimeBonus = 0.5f;
    [SerializeField] float timeBonus = 10.0f;
    public float comboTimeBonusStart;
    [SerializeField] float comboTimeBonusMinute = 10.0f;
    public float timeBonusStart;
    [SerializeField] float timeBonusMinute = 10.0f;
    private void Awake()
	{
		if(Instance !=null)
		{
			Destroy(gameObject); //새로 생긴걸 파괴
			return;
		}
		Instance = this; //this는 현재 Instance를 가리킴.
		DontDestroyOnLoad(gameObject); //씬이 로딩, 언로딩될 때도 메모리에 기억됨.
        coin = PlayerPrefs.GetInt("coin",0);
    }

    private void Cal() 
    {
        DateTime StartDate = DateTime.Now;
        DateTime EndDate = DateTime.Now;
        TimeSpan timeCal = EndDate - StartDate;
        int timeCalManute = timeCal.Minutes; //분차이
    }

    public void continueGame()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        this.addCoin(1);
        gameManager.ContinueGame();
    }

    public void coinGameStart()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        this.addCoin(1);
        gameManager.coinGameStart();
    }
    public void addCoin(int Amount)
    {
        this.coin+=Amount;
        PlayerPrefs.SetInt("coin",this.coin);
    }

    public void addRandomCoin()
    {
        int Amount = 1;
        int pick = UnityEngine.Random.Range(0,1000);
        int[] percent = new int[] {1,2,5,10,20,50,150,400,1000};
        for (int i = 0; i<percent.Length; i++)
        {
            if(pick<=percent[i])
            {
                Amount++;
            }
        }
        Debug.Log(Amount);
        addCoin(Amount);
    }
    
    public void SetComboTimeBonus(float Bonus)
    {
        this.comboTimeBonus = Bonus;
        PlayerPrefs.SetFloat("comboTimeBonus",comboTimeBonus);
        PlayerPrefs.SetFloat("comboTimeBonusStart",comboTimeBonusStart);
    }

    public void SetTimeBonus(float Bonus)
    {
        this.timeBonus = Bonus;
        PlayerPrefs.SetFloat("timeBonus",timeBonus);
        PlayerPrefs.SetFloat("timeBonusStart",timeBonusStart);
    }

}

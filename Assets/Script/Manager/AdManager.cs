using System;
using System.Collections.Generic;

using UnityEngine;
using GoogleMobileAds.Api;
//using GoogleMobileAds.Common;
public class AdManager : MonoBehaviour
{
    public static AdManager Instance {get; private set;}
    //inspector에서 편집 가능하게
    public Dictionary<string, string> adUnitIds = new Dictionary<string, string>();
    public Dictionary<string, string> adInterUnitIds = new Dictionary<string, string>();
    public Dictionary<string, RewardedAd> rewardedAds = new Dictionary<string, RewardedAd>();
    public Dictionary<string, InterstitialAd> interAds = new Dictionary<string, InterstitialAd>();
    private Dictionary<string, int> rewardRequestCount = new Dictionary<string, int>();
    public int GameToAdCount = 7;
    public int GameCount = 0;

    public void Awake()
	{
		if(Instance != null && Instance != this)
		{
			Destroy(this); //새로 생긴걸 파괴
            print("Destroy new AdManager");
			return;
		}
        else
        {
		    Instance = this; //this는 현재 Instance를 가리킴.
        }
		DontDestroyOnLoad(gameObject); //씬이 로딩, 언로딩될 때도 메모리에 기억됨.
        setRewards();

        GameCount = PlayerPrefs.GetInt("GameCount",0);
    }

    void OnEnable(){
        GameManager.GameSet+=AdSet;
    }
    
    public void setRewards()
    {
        MobileAds.Initialize(initStatus => { });

        #if UNITY_ANDROID
            this.adUnitIds.Add("coin","ca-app-pub-7387591250664253/3052509528");
            this.adUnitIds.Add("continue","ca-app-pub-7387591250664253/6837015323");
            this.adUnitIds.Add("coinStart","ca-app-pub-7387591250664253/5149973062");
            this.adUnitIds.Add("time","ca-app-pub-7387591250664253/3038436771");
            this.adUnitIds.Add("combo","ca-app-pub-7387591250664253/7721300702");
            
            this.adInterUnitIds.Add("10game", "ca-app-pub-7387591250664253/4537092958");
        #elif UNITY_IOS
            this.adUnitIds.Add("coin","ca-app-pub-7387591250664253/4950840120");
            this.adUnitIds.Add("continue","ca-app-pub-7387591250664253/3721518930");
            this.adUnitIds.Add("coinStart","ca-app-pub-7387591250664253/2523809724");
            //this.adUnitIds.Add("time","ca-app-pub-7387591250664253/3038436771");
            //this.adUnitIds.Add("combo","ca-app-pub-7387591250664253/7721300702");

            this.adInterUnitIds.Add("10game", "ca-app-pub-7387591250664253/6237917358");
        #else
            //this.adUnitId.Add("coin","ca-app-pub-7387591250664253/3052509528");
            //this.adUnitId.Add("continue","ca-app-pub-7387591250664253/6837015323");
            //this.adUnitId.Add("time","ca-app-pub-7387591250664253/3038436771");
            //this.adUnitId.Add("combo","ca-app-pub-7387591250664253/7721300702");
        #endif

        foreach(var key in adUnitIds.Keys)
        {
            if(!adUnitIds.ContainsKey(key))
            {
                #if UNITY_ANDROID
                    this.adUnitIds[key]="ca-app-pub-7387591250664253/3038436771";
                #elif UNITY_IOS
                    this.adUnitIds[key]="ca-app-pub-7387591250664253/4950840120";
                #endif
            }
            this.rewardRequestCount.Add(key,0);
            this.rewardedAds.Add(key, CreateRewardedAd(key));
        }

        foreach(KeyValuePair<string,string> entry in adInterUnitIds)
        {   
            this.interAds.Add(entry.Key, new InterstitialAd(entry.Value));
        }

    }

    RewardedAd CreateRewardedAd(string adUnitName)
    {
        string Id = this.adUnitIds[adUnitName];
        RewardedAd Ad = new RewardedAd(Id);
        //핸들러
        // Called when an ad request has successfully loaded.
        Ad.OnAdLoaded += delegate(object sender, EventArgs args) {HandleRewardedAdLoaded(sender, args, adUnitName);};
        // Called when an ad request failed to load.
        Ad.OnAdFailedToLoad += delegate(object sender, AdFailedToLoadEventArgs args) {HandleRewardedAdFailedToLoad(sender, args, adUnitName);};
        // Called when an ad is shown.
        Ad.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        Ad.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        Ad.OnUserEarnedReward += delegate(object sender, Reward args) {HandleUserEarnedReward(sender, args, adUnitName);};
        // Called when the ad is closed.
        Ad.OnAdClosed += delegate(object sender, EventArgs e) {HandleRewardedAdClosed(adUnitName);};
        return Ad;
    }

    InterstitialAd CreateInterAd(string adUnitName)
    {
        string Id = this.adInterUnitIds[adUnitName];
        InterstitialAd Ad = new InterstitialAd(Id);
        Ad.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        Ad.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        Ad.OnAdOpening += HandleOnAdOpening;
        Ad.OnAdClosed += delegate(object sender, EventArgs e) {HandleOnAdClosed(adUnitName);};
        return Ad;
    }

    public void LoadRewardedAd(string adUnitName)
    {
        RewardedAd Ad = rewardedAds[adUnitName];
        AdRequest request = new AdRequest.Builder().Build();
        Ad.LoadAd(request);
    }

    public void LoadInterAd(string adUnitName)
    {
        InterstitialAd Ad = interAds[adUnitName];
        AdRequest request = new AdRequest.Builder().Build();
        Ad.LoadAd(request);
    }
    public RewardedAd CreateAndLoadRewardedAd(string adUnitName)
    {
        RewardedAd Ad = CreateRewardedAd(adUnitName);
        AdRequest request = new AdRequest.Builder().Build();
        Ad.LoadAd(request);
        return Ad;
    }

    public InterstitialAd CreateAndLoadInterAd(string adUnitName)
    {
        InterstitialAd Ad = CreateInterAd(adUnitName);
        AdRequest request = new AdRequest.Builder().Build();
        Ad.LoadAd(request);
        return Ad;
    }

    //Reward Handler

    public void HandleRewardedAdLoaded(object sender, EventArgs args, string name)
    {
        print("HandleRewardedAdLoaded event received");
        this.rewardRequestCount[name] = 0;
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args, string name)
    {
        print("HandleRewardedAdFailedToLoad event received with message: " + args.ToString());
        if(this.rewardRequestCount[name]>3)
        {
            this.rewardedAds[name] = CreateAndLoadRewardedAd(name);
            this.rewardRequestCount[name] += 1;
            print(name+ " Ad Max Requested");
        }
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.AdError);  
    }

    public void HandleRewardedAdClosed(string adUnitName)
    {
        print("HandleRewardedAdClosed event received");
        print(adUnitName);
        this.rewardedAds[adUnitName] = CreateAndLoadRewardedAd(adUnitName);
    }

    public void HandleUserEarnedReward(object sender, Reward args, string name)
    {
        string type = args.Type;
        double amount = args.Amount;
        print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);

        if(name=="coin")
        {
            RewardManager.Instance.addRandomCoin();
        }
        else if(name=="continue")
        {
            RewardManager.Instance.continueGame();
        }
        else if(name=="coinStart")
        {
            RewardManager.Instance.coinGameStart();
        }
        else if(name=="time")
        {   
            RewardManager.Instance.SetTimeBonus(10.0f);
        }
        else if(name=="combo")
        {
            RewardManager.Instance.SetComboTimeBonus(1.0f);
        }
    }

    //InterHandler

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.ToString());
    }

    public void HandleOnAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpening event received");
        BGMManager.Instance.BGMOff();
    }

    public void HandleOnAdClosed(string adUnitName)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        BGMManager.Instance.BGMOn();
        this.interAds[adUnitName] = CreateAndLoadInterAd(adUnitName);
    }

    public void ShowRewardedAd(string name)
    {
        RewardedAd Ad = this.rewardedAds[name];
        if (Ad.IsLoaded())
        {
            Ad.Show();
        }
        else
        {
            print("NOT Loaded Rewarded");
            this.rewardedAds[name] = CreateAndLoadRewardedAd(name);
        }
    }

    public void ShowInterAd(string name) {
        InterstitialAd Ad = this.interAds[name];
        if(Ad.IsLoaded())
        {
            Ad.Show();
        }
        else
        {
            print("NOT Loaded Interstitial");
            this.interAds[name] = CreateAndLoadInterAd(name);
        }
    }

    void AdSet()
    {
        if(GameCount >= GameToAdCount)
        {
            LoadInterAd("10game");
            //startAd = restartButton.GetComponent<Btn_AlarmImage>();
            //startAd.AlarmToggle(true);
            //게임코드 정리
        }

        PlayerPrefs.SetInt("GameCount",GameCount++);
        Debug.Log("GameCount: " + GameCount);

        if(!rewardedAds["continue"].IsLoaded()){
            LoadRewardedAd("continue");
        }
    }

    public void showEndGameAd()
    {
        if(GameCount >= GameToAdCount && interAds["10game"].IsLoaded())
        {
            ShowInterAd("10game");
            GameCount = 0;
            PlayerPrefs.SetInt("GameCount",GameCount); //setter로 변경
            RewardManager.Instance.addCoin(1);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Btn_ShowRewardAd : MonoBehaviour
{
    //private AdManager adManager;
    [SerializeField] Button button, messageButton;
    [SerializeField] GameObject Popup;
    [SerializeField] string adUnit_name;
    private float timer = 0.0f;
    private int loadTry = 0;

    void Start()
    {        
        button.onClick.AddListener(() => AdManager.Instance.ShowRewardedAd(adUnit_name));
        messageButton.onClick.AddListener(() => messagePopup());
        /*Debug.Log("key출력");
        foreach(KeyValuePair<string, RewardedAd> data in AdManager.Instance.rewardedAds)
        {
            Debug.Log("key- " + data.Key);
        }*/
        //AdManager.Instance.LoadRewardedAd(adUnit_name);
    }

    void Update()
    {
        bool loaded = false;
        try
        {
            loaded = AdManager.Instance.rewardedAds[adUnit_name].IsLoaded();
        }
        catch
        {
            AdManager.Instance.setRewards();
        }
        
        if(!loaded)
        {
            button.interactable = false;
            messageButton.gameObject.SetActive(true);

            //3회까지는 1초마다 요청
            timer += Time.deltaTime;
            if(timer > 1.0f && loadTry < 3)
            {
                AdManager.Instance.LoadRewardedAd(adUnit_name);
                timer = 0.0f;
                loadTry +=1;
            }
        }
        else
        {          
            button.interactable = true;
            messageButton.gameObject.SetActive(false);

            timer = 0.0f;
            loadTry = 0;
        }
    }

    public void messagePopup(){
        Popup.SetActive(true);
        AdManager.Instance.ShowRewardedAd(adUnit_name);
    }
    
}

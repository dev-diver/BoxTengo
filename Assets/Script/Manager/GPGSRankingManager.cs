using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class GPGSRankingManager : MonoBehaviour
{   
    public static GPGSRankingManager Instance;
    [SerializeField] GameObject popup_RecordSuccess;
    [SerializeField] GameObject popup_fail;
    private void Awake() {
        if(Instance !=null)
		{
			Destroy(gameObject); //새로 생긴걸 파괴
			return;
		}
		Instance = this; //this는 현재 Instance를 가리킴.
		DontDestroyOnLoad(gameObject);
    }

    public void ShowLeaderboardUI_Ranking()
    {
        
        if(GPGSManager.Instance.isLogin()){
            #if UNITY_ANDROID
                ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(GPGSIds.leaderboard_score_ranking);
            #endif
        }
        else
        {
            popup_fail.SetActive(true);
            GPGSManager.Instance.Login();
        }
    }
    //내 리더보드 목록중 랭킹이라는 이름의 리더보드를 바로 보여준다
    
    public void ShowLeaderboardUI() => Social.ShowLeaderboardUI(); 
    //내 리더보드 목록을 보여주고 그 중 선택할 수 있다.

    public void AddLeaderboard(float score)
    {
        Social.ReportScore((int)score ,GPGSIds.leaderboard_score_ranking,(bool success)=>{
            if(success)
            {
                popup_RecordSuccess.SetActive(true);
            }
            else
            {
                popup_fail.SetActive(true);
                GPGSManager.Instance.Login();
            }
        });
        
    }
}
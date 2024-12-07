
using UnityEngine;

using TMPro;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class GPGSManager : MonoBehaviour
{
    public static GPGSManager Instance;
    [SerializeField] GameObject popup_success;
    [SerializeField] TextMeshProUGUI loginText;
    public bool login;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); //새로 생긴걸 파괴
            return;
        }
        Instance = this; //this는 현재 Instance를 가리킴.
        DontDestroyOnLoad(gameObject);
        SocialActivate();
    }

    void SocialActivate()
    {
#if UNITY_ANDROID
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
        .Builder()
        //.RequestServerAuthCode(false)
        //.RequestEmail() // 이메일 권한을 얻고 싶지 않다면 해당 줄(RequestEmail)을 지워주세요.
        //.RequestIdToken()
        .Build();
        // 구글 게임서비스 활성화 (초기화)
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
#endif
    }
    void Start()
    {
        //기본 로그인 권유
        login = PlayerPrefs.GetInt("login", 1) != 0;
        if (login)
        {
            //PlayerPrefs 리셋
            Login();
        }
    }

    public bool isLogin()
    {
        return Social.localUser.authenticated;
    }
    // 수동로그인 
    public void Login()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(AuthenticateCallback);
        }
    }
    // 수동 로그아웃 
    public void Logout()
    {
#if UNITY_ANDROID
        ((PlayGamesPlatform)Social.Active).SignOut();
#endif
        setLoginPrefs(false);
        popup_success.SetActive(true);
    }

    void setLoginPrefs(bool log)
    {
        if (log) //로그인
        {
            PlayerPrefs.SetInt("login", 1);
            //PlayerPrefs.SetInt("AutoRecord", PlayerPrefs.GetInt("AutoRecord", 1));
            login = true;
            loginText.text = "Logout";
        }
        else //로그아웃
        {
            PlayerPrefs.SetInt("login", 0);
            login = false;
            loginText.text = "Login";
        }
    }

    public void LoginOutBtnClicked()
    {
        print("Loginout Btn clicked");
        if (isLogin())
        {
            Logout();
        }
        else
        {
            Login();
        }
    }
    // 인증 callback
    void AuthenticateCallback(bool success)
    {
        setLoginPrefs(success);
    }

}
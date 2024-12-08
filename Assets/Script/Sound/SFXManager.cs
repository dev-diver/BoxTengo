using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    public MixerController mixerController;
    public AudioClip Aud_buttonClick, Aud_Selected, Aud_BoxingOut, Aud_Combo, Aud_Clear, Aud_preCount, Aud_Count, Aud_gameStart, Aud_gameOver, Aud_gameContinue, Aud_highScore;
    AudioSource audioSource;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); //새로 생긴걸 파괴
            return;
        }
        Instance = this; //this는 현재 Instance를 가리킴.

        DontDestroyOnLoad(gameObject); //씬이 로딩, 언로딩될 때도 메모리에 기억됨.
        this.audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameManager.GameOver += GameOver;
    }
    private void Start()
    {
        mixerController.setToSavedVolume();
    }

    public void PlaySound(string action)
    {

        AudioClip clipToPlay = null;
        switch (action)
        {
            case "CLICK":
                clipToPlay = Aud_buttonClick;
                break;
            case "SELECTED":
                clipToPlay = Aud_Selected;
                break;
            case "BOXING":
                clipToPlay = Aud_BoxingOut;
                break;
            case "COMBO":
                clipToPlay = Aud_Combo;
                break;
            case "CLEAR":
                clipToPlay = Aud_Clear;
                break;
            case "PRECOUNT":
                clipToPlay = Aud_preCount;
                break;
            case "COUNT":
                clipToPlay = Aud_Count;
                break;
            case "GAMESTART":
                clipToPlay = Aud_gameStart;
                break;
            case "GAMEOVER":
                clipToPlay = Aud_gameOver;
                break;
            case "GAMECONTINUE":
                clipToPlay = Aud_gameContinue;
                break;
            case "HIGHSCORE":
                clipToPlay = Aud_highScore;
                break;
        }

        if (clipToPlay != null)
        {
            audioSource.PlayOneShot(clipToPlay);
        }
    }

    void GameOver()
    {
        SFXManager.Instance.PlaySound("GAMEOVER");
    }
}

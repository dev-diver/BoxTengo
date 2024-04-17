using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    public MixerController mixerController;
    public  AudioClip Aud_buttonClick, Aud_Selected, Aud_BoxingOut, Aud_Combo, Aud_Clear, Aud_preCount, Aud_Count, Aud_gameStart, Aud_gameOver, Aud_gameContinue, Aud_highScore;
    AudioSource audioSource;
    private void Awake()
	{
		if(Instance !=null)
		{
			Destroy(gameObject); //새로 생긴걸 파괴
			return;
		}
		Instance = this; //this는 현재 Instance를 가리킴.

		DontDestroyOnLoad(gameObject); //씬이 로딩, 언로딩될 때도 메모리에 기억됨.
        this.audioSource = GetComponent<AudioSource>();
	}

    private void OnEnable() {
        GameManager.GameOver += GameOver;
    }
    private void Start()
    {
        mixerController.setToSavedVolume();
    }

    public void PlaySound(string action)
    {
        switch(action) 
        {
            case "CLICK":
                audioSource.clip = Aud_buttonClick;
                break;
            case "SELECTED":
                audioSource.clip = Aud_Selected;
                break;
            case "BOXING":
                audioSource.clip = Aud_BoxingOut;
                break;
            case "COMBO":
                audioSource.clip = Aud_Combo;
                break;
            case "CLEAR":
                audioSource.clip = Aud_Clear;
                break;
            case "PRECOUNT":
                audioSource.clip = Aud_preCount;
                break;
            case "COUNT":
                audioSource.clip = Aud_Count;
                break;
            case "GAMESTART":
                audioSource.clip = Aud_gameStart;
                break;
            case "GAMEOVER":
                audioSource.clip = Aud_gameOver;
                break;
            case "GAMECONTINUE":
                audioSource.clip = Aud_gameContinue;
                break;
            case "HIGHSCORE":
                audioSource.clip = Aud_highScore;
                break;            
        }
        audioSource.PlayOneShot(audioSource.clip);
    }

    void GameOver(){
        SFXManager.Instance.PlaySound("GAMEOVER");
    }
}


using UnityEngine;
using UnityEngine.SceneManagement;
public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;
    [SerializeField] MixerController mixerController;
    private AudioSource audioSource;
    [SerializeField] AudioClip audioClip1, audioClip2, audioClip3;
    Coroutine InCo, OutCo;
    private float fadeTime = 5.0f;
    private void Awake()
	{
		if(Instance !=null)
		{
			Destroy(gameObject); //새로 생긴걸 파괴
			return;
		}
		Instance = this; //this는 현재 Instance를 가리킴.
		DontDestroyOnLoad(gameObject); //씬이 로딩, 언로딩될 때도 메모리에 기억됨.
        audioSource = GetComponent<AudioSource>();
	}

    private void Start() {
        mixerController.setToSavedVolume();
    }

    void OnEnable()
    {
    	  // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        GameManager.GamePause += GamePause;
        GameManager.GameStart += GameStart;
        GameManager.GameOver += GameOver;
        GameManager.PreCount += BGMOff;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameManager.GamePause -= GamePause;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch(scene.name)
        {
            case "MainMenu":
                print("Mainload");
                BGMChange("MAIN");
                break;
        }
    }
    public void BGMChange(string action)
    {
        switch(action)
        {   
            case "MAIN":
                audioSource.clip = audioClip1;
                break;
            case "GAME":       
                audioSource.clip = audioClip2;
                break;
            case "PAUSE":
                audioSource.clip = audioClip3;
                break;
            case "OVER":
                audioSource.clip = audioClip3;
                break;
        }
        InCo = StartCoroutine(AudioHelper.FadeIn(audioSource, fadeTime));
    }

    public void BGMOff()
    {
        StopCoroutine(InCo);
        audioSource.volume = 0;
    }

    public void BGMOn()
    {
        InCo = StartCoroutine(AudioHelper.FadeIn(audioSource, fadeTime));
    }
    
    void GamePause()
    {
        BGMChange("PAUSE");
    }

    void GameOver()
    {
        BGMChange("OVER");
    }

    void GameStart()
    {
        BGMChange("START");
    }

}

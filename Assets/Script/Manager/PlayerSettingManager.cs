using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerSettingManager : MonoBehaviour
{
    public static PlayerSettingManager Instance;
    [SerializeField] GameObject popup_setting;
    [SerializeField] Toggle AutoRecordToggle;
    
    public bool recordOn;
    private void Awake() {
        if(Instance !=null)
		{
			Destroy(gameObject); //새로 생긴걸 파괴
			return;
		}
		Instance = this; //this는 현재 Instance를 가리킴.
		DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(popup_setting);
    }

    void Start() 
    {
        setPlayerToggle();
    }

    void setPlayerToggle()
    {   
        AutoRecordToggle.isOn = getPlayerAutoRecordOpt() && GPGSManager.Instance.login;
        recordOn = AutoRecordToggle.isOn;
    }

    public bool getPlayerAutoRecordOpt()
    {
        return PlayerPrefs.GetInt("AutoRecord", 1) != 0;
    }

    public void ChangedAutoRecordToggle(bool change){
        if(change)
        {
            GPGSManager.Instance.Login();
            if(GPGSManager.Instance.login)
            {
                setRecordPrefs(change);
            }
            else
            {
                AutoRecordToggle.isOn = false;
            }
        }
        else
        {
            setRecordPrefs(change);
        }
    }

    void setRecordPrefs(bool record)
    {
        PlayerPrefs.SetInt("AutoRecord", record ? 1 :0);
        recordOn = record;
    }
}

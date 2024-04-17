using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneBridge : MonoBehaviour
{
    Button btn_setting;
    GameObject popup;
    Canvas canvas;
    Camera mainCam;
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {   
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        GameObject gpgsManager = gameObject;
        canvas = gpgsManager.GetComponentInChildren<Canvas>();
        popup = canvas.transform.GetChild(0).gameObject;
        btn_setting = GameObject.Find("Btn_setting").GetComponent<Button>();

        canvas.worldCamera = mainCam;
        btn_setting.onClick.AddListener(()=>openPopup());
    }

    public void openPopup()
    {
        popup.SetActive(true);
    }

    void OnDisable(){
        btn_setting.onClick.RemoveAllListeners();
    }

}

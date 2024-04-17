using UnityEngine;
using UnityEngine.UI;

public class Btn_OpenPopup : MonoBehaviour
{
    public GameObject Popup;
    void OnEnable() {
        GetComponent<Button>().onClick.AddListener(()=>openPopup());
    }
    public void openPopup()
    {
        Popup.SetActive(true);
    }

    void OnDisable() {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }
    
}

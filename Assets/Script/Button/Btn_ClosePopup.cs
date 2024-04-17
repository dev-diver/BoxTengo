using UnityEngine;

public class Btn_ClosePopup : MonoBehaviour
{
    public void closePopup()
    {
        gameObject.SetActive(false);
    }
    public void closeAllPopup()
    {
        GameObject[] popups = GameObject.FindGameObjectsWithTag("Popup");
        
        foreach(var popup in popups)
        {
            popup.SetActive(false);
        }
        
    }
}

using UnityEngine;

public class AppleSound : MonoBehaviour
{
    public void playSound(string action){
        switch(action) 
        {
            case "SELECTED":
                SFXManager.Instance.PlaySound("SELECTED");
                break;
            case "REMOVE":
                SFXManager.Instance.PlaySound("BOXING");
                break;
        }
    }

}
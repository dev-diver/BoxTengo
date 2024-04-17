using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Btn_Exit : MonoBehaviour
{
    public void exitgame() {  
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}





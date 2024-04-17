using UnityEngine;
using UnityEngine.SceneManagement;

public class Btn_ChangeScene : MonoBehaviour
{
    public string sceneToLoad;
    public void changeScene()
    {
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync(sceneToLoad);
    }

}

using UnityEngine;
using UnityEngine.UI;

public class Btn_AlarmImage : MonoBehaviour
{
    Image AlarmImage;
    private void Awake()
    {
        Debug.Log("Btn_AlarmImage Start");
        AlarmImage = gameObject.transform.Find("alarmImage").GetComponent<Image>();
        AlarmToggle(false);
    }

    public void AlarmToggle(bool opt)
    {
        AlarmImage.enabled = opt;
        print("toggle: " + opt);
    }

}

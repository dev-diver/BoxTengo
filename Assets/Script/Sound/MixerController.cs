using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MixerController : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider audioSlider;
    [SerializeField] Toggle toggle;
    [SerializeField] string groupName;

    float beforeVolume = 0;
    void Awake() {
        setToSavedVolume();
    }
    public void setToSavedVolume() {
        audioSlider.value = getSavedVolume();
    }
    public float getSavedVolume() {
        return PlayerPrefs.GetFloat(groupName+"Volume", 1);
    }

    public void OnChangedValue()
    {
        setVolume(audioSlider.value);
    }
    public void ToggleValueChanged(bool change)
    {
        if(toggle.isOn)
        {
            if(beforeVolume != audioSlider.minValue)
            {
                audioSlider.value = beforeVolume;
            }
        }
        else{
            beforeVolume = audioSlider.value;
            audioSlider.value = audioSlider.minValue;
        }
    }

    void setVolume(float value)
    {
        PlayerPrefs.SetFloat(groupName+"Volume", value);
        audioMixer.SetFloat(groupName, Mathf.Log10(value)*20);
        if(value == audioSlider.minValue)
        {
            toggle.isOn = false;
        }else{
            toggle.isOn = true;
        }
    }

    public void OnPointerUp(BaseEventData eventData){
        SFXManager.Instance.PlaySound("CLICK");
    }
}

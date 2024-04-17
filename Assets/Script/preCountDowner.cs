using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class preCountDowner : MonoBehaviour
{
    TextMeshProUGUI countText;
    void Awake()
    {   
        GameObject textObj = gameObject.transform.Find("text").gameObject;
        this.countText = textObj.GetComponent<TextMeshProUGUI>();
        this.countText.text = "yo";
    }

    public void setTime(float time)
    {
        this.countText.text = time.ToString();
    }

    public void turnOff()
    {
        gameObject.SetActive(false);
    }

    public void turnOn()
    {
        gameObject.SetActive(true);
    }

}

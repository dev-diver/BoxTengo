using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Btn_ContinueCoin : MonoBehaviour
{
    Button btn;
    TextMeshProUGUI coinText;
    void Awake()
    {
        btn = GetComponent<Button>();
        coinText = transform.Find("CoinAmount").GetComponent<TextMeshProUGUI>();
    }

    void Update() //onEnable과 event로 교체
    {
        coinText.text ="("+RewardManager.Instance.coin+"/1)";
        if(RewardManager.Instance.coin<=0)
        {
            btn.interactable=false;
        }
        else{
            btn.interactable=true;
        }
    }

}

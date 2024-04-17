using UnityEngine;
using TMPro;

public class checkCoinAmount : MonoBehaviour
{
    TextMeshProUGUI coinText;
    // Start is called before the first frame update
    void Start()
    {
        coinText = GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        coinText.text="("+RewardManager.Instance.coin+")";
    }

}

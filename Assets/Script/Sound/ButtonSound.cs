using UnityEngine;
using UnityEngine.UI;
public class ButtonSound : MonoBehaviour
{
    void Start() {
        GetComponent<Button>().onClick.AddListener(()=>SFXManager.Instance.PlaySound("CLICK"));
    }

}

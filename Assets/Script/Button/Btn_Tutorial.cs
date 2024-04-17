using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Btn_Tutorial : MonoBehaviour
{
    [SerializeField] GameObject Popup;
    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(()=>this.btnClick());
    }

    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }

    //바구니에 떨어지는 애니메이션
    void AnimationStart(){
        Animator tutorial_Animator = GameObject.FindGameObjectWithTag("StartButton").GetComponent<RectTransform>().GetComponent<Animator>();
        tutorial_Animator.SetTrigger("Drop");
    }

    void btnClick(){
        AnimationStart();
        StartCoroutine(openPopup());
    }

    IEnumerator openPopup()
    {
        yield return new WaitForSeconds(0.58f);
        Popup.SetActive(true);
    }

}

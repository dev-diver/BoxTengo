using System.Collections;
using UnityEngine;
using TMPro;

public class ClearManager : MonoBehaviour
{
    [SerializeField] GameObject clearPrefab;
    [SerializeField] GameObject clearParent;
    [SerializeField] float animTime = 1.0f;

    public void showClearText()
    {
        GameObject comboObj = Instantiate(clearPrefab, Vector2.zero, clearParent.transform.rotation, clearParent.transform);
        
        RectTransform comboObjRect = comboObj.GetComponent<RectTransform>();
        Animation anim = comboObjRect.GetComponent<Animation>();
        anim.Play();
   
        TextMeshProUGUI comboText = comboObj.GetComponent<TextMeshProUGUI>();
        //comboText.text = " CLEAR";

        SFXManager.Instance.PlaySound("CLEAR");
        StartCoroutine(PointDestroyRoutine(comboObj));
    }

    public IEnumerator PointDestroyRoutine(GameObject pointObj)
    {
        yield return new WaitForSeconds(animTime);
        Destroy(pointObj);
        Debug.Log("Clear Destroy");
    }

}

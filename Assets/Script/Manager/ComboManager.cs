using System.Collections;
using UnityEngine;
using TMPro;

public class ComboManager : MonoBehaviour
{
    [SerializeField] GameObject ComboPrefab;
    [SerializeField] GameObject comboParent;
    [SerializeField] public int combo;
    float baseComboTime;

    [Tooltip("Choose setting")]
    public GameConfig _settings;
    GameConfig s;

    void Start(){
        s=_settings;
    }
    public void showComboText(int comboCount)
    {

        GameObject comboObj = Instantiate(ComboPrefab, Vector2.zero, comboParent.transform.rotation, comboParent.transform);
        
        RectTransform comboObjRect = comboObj.GetComponent<RectTransform>();
        Animation anim = comboObjRect.GetComponent<Animation>();
        anim.Play();
   
        TextMeshProUGUI comboText = comboObj.GetComponent<TextMeshProUGUI>();
        comboText.text = comboCount + " Combo!";

        SFXManager.Instance.PlaySound("COMBO");
        StartCoroutine(ComboDestroyRoutine(comboObj));
    }
    public IEnumerator ComboCountDownRoutine()
    {
        yield return new WaitForSeconds(s.comboTime);
        combo = 0;
    }

    public IEnumerator ComboDestroyRoutine(GameObject comboObj)
    {
        yield return new WaitForSeconds(s.comboTime);
        Destroy(comboObj);
    }

    public void CalBaseComboTime(int leftApples)
    {
        baseComboTime = s.baseMaxComboTime/(leftApples+1);
    }

    public float comboAddTime ()
    {
        float addTime = baseComboTime + combo * s.plusComboTime;
        if(addTime>=s.maxComboTime)
        {
            addTime = s.maxComboTime;
        }
        return addTime;
    }


}

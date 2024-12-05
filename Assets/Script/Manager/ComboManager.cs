using System.Collections;
using UnityEngine;
using TMPro;

public class ComboManager : MonoBehaviour
{
    [SerializeField] GameObject ComboPrefab;
    [SerializeField] GameObject ComboParent;
    public int Combo;
    float _baseComboTime;

    [Tooltip("Choose setting")]
    public GameConfig GameSettings;

    public void ShowComboText(int comboCount)
    {

        GameObject comboObj = Instantiate(ComboPrefab, Vector2.zero, ComboParent.transform.rotation, ComboParent.transform);

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
        yield return new WaitForSeconds(GameSettings.comboTime);
        Combo = 0;
    }

    public IEnumerator ComboDestroyRoutine(GameObject comboObj)
    {
        yield return new WaitForSeconds(GameSettings.comboTime);
        Destroy(comboObj);
    }

    public void CalBaseComboTime(int leftApples)
    {
        _baseComboTime = GameSettings.baseMaxComboTime / (leftApples + 1);
    }

    public float comboAddTime()
    {
        float addTime = _baseComboTime + Combo * GameSettings.plusComboTime;
        if (addTime >= GameSettings.maxComboTime)
        {
            addTime = GameSettings.maxComboTime;
        }
        return addTime;
    }


}

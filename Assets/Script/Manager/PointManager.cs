using System.Collections;
using UnityEngine;
using TMPro;

public class PointManager : MonoBehaviour
{
    [SerializeField] GameObject PointPrefab;
    [SerializeField] float pointTime = 1.0f;
    public void showPointText(float Point, Vector2 position)
    {
        GameObject pointObj = Instantiate(PointPrefab, position, Quaternion.identity, this.transform);
        GameObject pointTextObj = pointObj.transform.GetChild(0).gameObject;

        TextMeshProUGUI pointText = pointTextObj.GetComponent<TextMeshProUGUI>();
        pointText.text = Point.ToString();

        Animation anim = pointTextObj.GetComponent<Animation>();
        anim.Play();
        
        StartCoroutine(PointDestroyRoutine(pointObj));
    }

    public IEnumerator PointDestroyRoutine(GameObject pointObj){
        yield return new WaitForSeconds(pointTime);
        Destroy(pointObj);
    }

    public static float remap(float val, float in1, float in2, float out1, float out2)  //리맵하는 함수
    {
        return out1 + (val - in1) * (out2 - out1) / (in2 - in1);
    }

}

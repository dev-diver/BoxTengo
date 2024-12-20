using UnityEngine;

public class Apple : MonoBehaviour
{
    public Vector2 coord;
    private int number;
    public int Number
    {
        get { return number; } // getter
        set
        {
            number = value;
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = AppleManager.Instance.AppleSprite[value - 1];
        } // setter
    }

    private Animator animator;
    private AppleSound appleAudio;
    void Start()
    {
        GameObject gameBoard = transform.parent.gameObject;
        int AppleGridSize = AppleManager.Instance.AppleGridSize;
        animator = transform.GetChild(0).GetComponent<Animator>();
        appleAudio = GetComponent<AppleSound>();

        float halfWidth = gameBoard.transform.localScale.x / 2;
        float halfHeight = gameBoard.transform.localScale.y / 2;
        float appleScale = halfWidth / AppleGridSize;
        float localAppleScale = (float)1 / AppleGridSize;
        //Debug.Log(halfWidth +", " +halfHeight);
        transform.position = new Vector3(remap(coord.x, 0, AppleGridSize - 1, -halfWidth + appleScale, halfWidth - appleScale),
                                        remap(coord.y, 0, AppleGridSize - 1, -halfHeight + appleScale, halfHeight - appleScale), 1);
        transform.localScale = new Vector2(localAppleScale, localAppleScale);
    }
    // Update is called once per frame
    public void Selected()
    {
        animator.SetBool("selected", true);
        appleAudio.playSound("SELECTED");
    }

    public void Unselected()
    {
        animator?.SetBool("selected", false);
    }

    public void BoxingOut()
    {
        //collider만 제거하고 Destroy는 AllClear시 실행
        Destroy(GetComponent<CircleCollider2D>());
        appleAudio.playSound("REMOVE");
        animator?.SetTrigger("disappear");
    }

    public static float remap(float val, float in1, float in2, float out1, float out2)  //리맵하는 함수
    {
        return out1 + (val - in1) * (out2 - out1) / (in2 - in1);
    }
}

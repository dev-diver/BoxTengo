using System.Linq;
using System.Collections.Generic;
using UnityEngine;
// using System.Diagnostics;

public class DrawSelection : MonoBehaviour
{
    private Touch theTouch;
    private Vector2 touchStartPosition, touchEndPosition;
    private LineRenderer lineRend;
    private List<Vector2> colliderPoints = new List<Vector2>(4);
    private PolygonCollider2D colliderShape;
    private List<Collider2D> beforeCollider;
    public GameManager GameManagerInstance;
    public AppleManager AppleManagerInstance;

    private void OnEnable()
    {
        GameManager.GameStart += Enable;
        GameManager.GamePause += Disable;
        GameManager.GameOver += Disable;
    }

    private void OnDisable()
    {
        GameManager.GameStart -= Enable;
        GameManager.GamePause -= Disable;
        GameManager.GameOver -= Disable;
    }

    void Start()
    {
        Init();
        Disable();
    }

    private void Init()
    {
        lineRend = GetComponent<LineRenderer>();
        lineRend.positionCount = 0;
        colliderShape = GetComponent<PolygonCollider2D>();
        colliderShape.pathCount = 0;
        colliderShape.isTrigger = true;
    }

    public void Enable()
    {
        Init();
        lineRend.enabled = true;
    }

    public void Disable()
    {
        EraseRectangle();
        lineRend.enabled = false;
    }

    void Update()
    {
        if (Input.touchCount > 0 && lineRend.enabled)
        {
            theTouch = Input.GetTouch(0);
            if (theTouch.phase == TouchPhase.Began)
            {
                EraseRectangle();
                touchStartPosition = Camera.main.ScreenToWorldPoint(theTouch.position);
                touchEndPosition = Camera.main.ScreenToWorldPoint(theTouch.position);
                DrawRectangle();
                CheckOverlap();
            }
            else if (theTouch.phase == TouchPhase.Moved)
            {
                touchEndPosition = Camera.main.ScreenToWorldPoint(theTouch.position);
                DrawRectangle();
                CheckOverlap();
            }

            if (theTouch.phase == TouchPhase.Ended)
            {
                touchEndPosition = Camera.main.ScreenToWorldPoint(theTouch.position);
                AppleManagerInstance.BoxingApple();
                EraseRectangle();

            }
        }
    }

    private void DrawRectangle()
    {
        //Debug.Log(colliderPoints.Count);
        colliderPoints.Clear();

        colliderPoints.Add(new Vector2(touchStartPosition.x, touchStartPosition.y));
        colliderPoints.Add(new Vector2(touchStartPosition.x, touchEndPosition.y));
        colliderPoints.Add(new Vector2(touchEndPosition.x, touchEndPosition.y));
        colliderPoints.Add(new Vector2(touchEndPosition.x, touchStartPosition.y));
        ApplyCollider();

        lineRend.positionCount = 4;
        for (int i = 0; i < colliderPoints.Count; i++)
        {
            lineRend.SetPosition(i, colliderPoints[i]);
        }

    }

    private void ApplyCollider()
    {
        colliderShape.pathCount = colliderPoints.Count;
        colliderShape.points = colliderPoints.ToArray();
    }

    private void EraseRectangle()
    {
        lineRend.positionCount = 0;
        colliderPoints.Clear();
        ApplyCollider();
        AppleManagerInstance.UnSelectAllApples();
    }

    private void CheckOverlap()
    {

        // Stopwatch stopwatch = new Stopwatch();
        // stopwatch.Start();

        List<Collider2D> colliders = new List<Collider2D>();
        colliderShape.OverlapCollider(new ContactFilter2D(), colliders);

        HashSet<Apple> collidingAppleSet = new HashSet<Apple>();

        foreach (var collider in colliders)
        {
            if (collider?.gameObject.CompareTag("Apple") == true)
            {
                Apple apple = collider.gameObject.GetComponent<Apple>();
                if (apple != null)
                {
                    collidingAppleSet.Add(apple);
                }
            }
        }

        // 제거 경우
        var unselectedApples = AppleManagerInstance.SelectedApples
            .Where(selected => !collidingAppleSet.Contains(selected))
            .ToList();

        foreach (var apple in unselectedApples)
        {
            AppleManagerInstance.UnSelectApple(apple);
        }

        // 추가 경우
        foreach (var colliding in collidingAppleSet)
        {
            if (!AppleManagerInstance.SelectedApples.Contains(colliding))
            {
                AppleManagerInstance.SelectApple(colliding);
            }
        }

        // stopwatch.Stop();

        // 실행 시간 로그 출력
        // UnityEngine.Debug.Log($"Execution Time: {stopwatch.Elapsed.TotalMilliseconds:F6} ms");
        // UnityEngine.Debug.Log($"Execution Time: {stopwatch.ElapsedTicks} ticks");

    }
}

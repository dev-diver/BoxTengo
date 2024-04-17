using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DrawSelection : MonoBehaviour
{
    private Touch theTouch;
    public Vector2 touchStartPosition, touchEndPosition;
    public LineRenderer lineRend;
    private List<Vector2> colliderPoints = new List<Vector2>(4);
    private PolygonCollider2D colliderShape;
    private List<Collider2D> beforeCollider;
    public GameManager gameManager;
    public AppleManager appleManager;

    private void OnEnable()
    {
        GameManager.GameStart += Enable;
        GameManager.GamePause += Disable;
        GameManager.GameOver += Disable;
    }

    private void OnDisable() {
        GameManager.GameStart -= Enable;
        GameManager.GamePause -= Disable;
        GameManager.GameOver -= Disable;
    }

    void Start()
    {
        Init();
        Disable();
    }

    public void Init()
    {
        lineRend.positionCount = 0;
        colliderShape = GetComponent<PolygonCollider2D>();
        colliderShape.pathCount = 0;
        colliderShape.isTrigger = true;  
    }

    public void Enable(){
        Init();
        lineRend.enabled = true;
    }

    public void Disable(){
        EraseRectangle();
        lineRend.enabled = false;
    }

    void Update()
    {
        if(Input.touchCount > 0 && lineRend.enabled){
            theTouch = Input.GetTouch(0);            
            if(theTouch.phase == TouchPhase.Began)
            {
                EraseRectangle();
                touchStartPosition = Camera.main.ScreenToWorldPoint(theTouch.position);
                touchEndPosition = Camera.main.ScreenToWorldPoint(theTouch.position);
                DrawRectangle();
                CheckOverlap();
            }
            else if(theTouch.phase == TouchPhase.Moved)
            {
                touchEndPosition = Camera.main.ScreenToWorldPoint(theTouch.position);
                DrawRectangle();                   
                CheckOverlap();
            }

            if(theTouch.phase == TouchPhase.Ended)
            {
                touchEndPosition = Camera.main.ScreenToWorldPoint(theTouch.position);
                appleManager.BoxingApple();
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
        for(int i = 0; i<colliderPoints.Count;i++){
            lineRend.SetPosition(i, colliderPoints[i]);
        }
        
    }

    private void ApplyCollider()
    {   
        colliderShape.pathCount = colliderPoints.Count;
        colliderShape.points = colliderPoints.ToArray();
    }

    public void EraseRectangle()
    {
        lineRend.positionCount = 0;
        colliderPoints.Clear();
        ApplyCollider();
        appleManager.ClearSelected();
    }
    
    void CheckOverlap() 
    {
        List<Collider2D> colliders = new List<Collider2D>();       
        colliderShape.OverlapCollider(new ContactFilter2D(), colliders); //colliders에 충돌 배열 넣음

        List<Apple> collidingApples = new List<Apple>();

        foreach(var collider in colliders)
        {
            if(collider?.gameObject.tag=="Apple")
            {
                Apple apple = collider.gameObject.GetComponent<Apple>();
                collidingApples.Add(apple);
            }
        }

        if(appleManager.selectedApples.Count!=collidingApples.Count)
        {
            //제거 경우
            for(int i=0;i<appleManager.selectedApples.Count;i++)
            {
                Apple selected = appleManager.selectedApples[i];
                bool find = false;
                foreach(var colliding in collidingApples){
                    if(System.Object.ReferenceEquals(colliding,selected))
                    {
                        find = true;
                        break;
                    }
                }
                if(find == false)
                {
                    selected.Unselect();        
                }
            }
            //추가 경우
            foreach(var colliding in collidingApples)
            {
                bool find = false;
                foreach(var selected in appleManager.selectedApples){
                    if(System.Object.ReferenceEquals(colliding,selected))
                    {
                        find = true;
                        break;
                    }
                }
                //Debug.Log(find + "," + newCollid.gameObject.tag);
                if(find == false)
                {
                    colliding.Select();
                }
            }
        }
    }
}

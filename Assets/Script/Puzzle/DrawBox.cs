using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DrawBox : MonoBehaviour
{
    private Touch theTouch;
    private Vector2 touchStartPosition, touchEndPosition;
    [SerializeField] LineRenderer lineRend;
    private List<Vector2> colliderPoints = new List<Vector2>(4);
    private PolygonCollider2D colliderShape;
    private List<Collider2D> beforeCollider;

    public List<BlockBehaviour> selectedBlocks = new List<BlockBehaviour>();

    [SerializeField] StageController stageController;

    void Start()
    {
        Init();
    }
    public void Init()
    {
        lineRend.positionCount = 0;
        colliderShape = lineRend.gameObject.AddComponent<PolygonCollider2D>();
        colliderShape.pathCount = 0;
        colliderShape.isTrigger = true;  
    }
    void Update()
    {
        if(Input.touchCount > 0){
            theTouch = Input.GetTouch(0);            
            if(theTouch.phase == TouchPhase.Began)
            {
                EraseRectangle();
                ClearSelected();
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
                EraseRectangle();
                BoxingBlock();
                ClearSelected();
            }      
        }
    }

    private void DrawRectangle()
    {
        //Debug.Log(colliderPoints.Count);
        colliderPoints.Clear();
        lineRend.positionCount = 4;
        colliderPoints.Add(new Vector2(touchStartPosition.x, touchStartPosition.y));
        colliderPoints.Add(new Vector2(touchStartPosition.x, touchEndPosition.y));
        colliderPoints.Add(new Vector2(touchEndPosition.x, touchEndPosition.y));
        colliderPoints.Add(new Vector2(touchEndPosition.x, touchStartPosition.y));
        for(int i = 0; i<colliderPoints.Count;i++){
            lineRend.SetPosition(i, colliderPoints[i]);
        }
        ApplyCollider();
    }

    private void ApplyCollider()
    {   
        colliderShape.points = colliderPoints.ToArray();
    }

    public void EraseRectangle()
    {
        lineRend.positionCount = 0;
        colliderPoints.Clear();
        ApplyCollider();
    }
    
    void CheckOverlap() 
    {
        List<Collider2D> colliders = new List<Collider2D>();       
        colliderShape.OverlapCollider(new ContactFilter2D(), colliders); //colliders에 충돌 배열 넣음

        List<BlockBehaviour> collidingBlocks = new List<BlockBehaviour>();

        foreach(var collider in colliders)
        {
            if(collider?.gameObject.tag=="Block")
            {
                BlockBehaviour block = collider.gameObject.GetComponent<BlockBehaviour>();
                collidingBlocks.Add(block);
            }
        }

        if(selectedBlocks.Count!=collidingBlocks.Count)
        {
            //제거 경우
            for(int i=0;i<selectedBlocks.Count;i++)
            {
                BlockBehaviour selected = selectedBlocks[i];
                bool find = false;
                foreach(var colliding in collidingBlocks){
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
            foreach(var colliding in collidingBlocks)
            {
                bool find = false;
                foreach(var selected in selectedBlocks){
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

    public void ClearSelected()
    {
        for(var i = selectedBlocks.Count-1; i>=0; i--)
        {
            selectedBlocks[i]?.Unselect();
        }
    }

    private int CalSelect(List<BlockBehaviour> Blocks)
    {
        int sum= 0;
        foreach(var block in Blocks)
        {
            sum+=block.m_Block.number;
        }
        return sum;
    }

    public void BoxingBlock()
    {
        if(CalSelect(selectedBlocks)==10)
        {
            foreach(var block in selectedBlocks)
            {
                block.BoxingOut();
            }
            
            if(stageController.leftBlocks()==0){
                Debug.Log("Clear!");
                stageController.nextStage();
            }
        }
    }
}

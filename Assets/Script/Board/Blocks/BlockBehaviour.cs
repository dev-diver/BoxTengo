using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BlockBehaviour : MonoBehaviour
{
    public Block m_Block;
    SpriteRenderer m_SpriteRenderer;
    [SerializeField] BlockConfig m_BlockConfig;

    private Animator animator;
    private AppleSound appleAudio;
    [SerializeField] DrawBox drawBox;
    StageController stageController;


    void Start()
    {
        stageController = transform.parent.parent.GetComponent<StageController>();
        drawBox = GameObject.Find("drawBox").GetComponent<DrawBox>();
        m_SpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        appleAudio = GetComponent<AppleSound>();
        
        UpdateView(false);
    }

    internal void SetBlock(Block block)
    {
        m_Block = block;
    }

    public void UpdateView(bool bValueChanged)
    {
        if (m_Block.type == BlockType.EMPTY)
        {
            m_SpriteRenderer.sprite = null;
        }
        else if(m_Block.type == BlockType.SETTING || m_Block.type == BlockType.APPLE)
        {
            m_SpriteRenderer.sprite = m_BlockConfig.appleBlockSprites[(int)m_Block.number];
        }
    }

    public void Select()
    {
        animator.SetBool("selected",true);
        //appleAudio?.playSound("SELECTED");
        drawBox.selectedBlocks.Add(this);
    }

    public void Unselect()
    {
        animator?.SetBool("selected",false);
        drawBox.selectedBlocks.Remove(this);
    }

    public void BoxingOut()
    {
        Debug.Log("BoxingOut");
        Destroy(GetComponent<CircleCollider2D>());
        stageController.BoxingOut(m_Block.pos.x,m_Block.pos.y);
        //appleAudio?.playSound("REMOVE");
        animator?.SetTrigger("disappear");
    }
}
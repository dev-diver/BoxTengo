using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBehaviour : MonoBehaviour
{
    Cell m_Cell;
    SpriteRenderer m_SpriteRenderer;

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        UpdateView(false);
    }

    public void SetCell(Cell cell)
    {
        m_Cell = cell;
    }

    public void UpdateView(bool bValueChanged)
    {
        if (m_Cell.type == CellType.EMPTY)
        {
            m_SpriteRenderer.sprite = null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[AddComponentMenu("UI/Single Scroll View", 39)]
[SelectionBase]
[ExecuteInEditMode]
[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
[XLua.LuaCallCSharp]
public class SingleScrollView : ScrollView
{
    struct CellData
    {
        public Vector2 cellSize;
        public Vector2 cellPos;
    }

    List<CellData> m_CellDataList = new List<CellData>();
    bool m_IsVertical;

    public delegate int CellGetSizeDelegate(int index);

    // 获取元素高度回调
    public CellGetSizeDelegate onGetCellHeight;
    // 获取元素宽度回调
    public CellGetSizeDelegate onGetCellWidth;

    protected override void OnRefillCell(int index)
    {
        RefreshSizeAndPos();
    }

    protected override void OnRefreshSize()
    {
        m_IsVertical = this.vertical;

        if (m_IsVertical)
        {
            RefreshVerticalSize();
        }
        else
        {
            RefreshHorizontalSize();
        }
    }

    protected override Vector2 OnGetPos(int row, int col)
    {
        int index = m_IsVertical ? row : col;
        if (index >= m_CellDataList.Count)
        {
            return Vector2.zero;
        }

        CellData cellData = m_CellDataList[index];

        Vector2 pos = cellData.cellPos;
        // 处理居中
        pos += new Vector2(cellData.cellSize.x * 0.5f, -cellData.cellSize.y * 0.5f);
        // 处理偏移
        pos += new Vector2(padding.left, -padding.top);

        return pos;
    }

    protected override bool OnGetWidthHeight(int row, int col, out Vector2 sizeDelta)
    {
        sizeDelta = Vector2.zero;
        int index = m_IsVertical ? row : col;
        if (index >= m_CellDataList.Count)
        {
            return true;
        }

        CellData cellData = m_CellDataList[index];
        sizeDelta = cellData.cellSize;
        return true;
    }

    protected override int OnGetPosIndex(Vector2 pos)
    {
        pos -= new Vector2(padding.left, -padding.top);

        if (m_IsVertical && pos.y > 0)
        {
            return -1;
        }

        if (!m_IsVertical && pos.x < 0)
        {
            return -1;
        }

        for (int i = 0; i < m_CellDataList.Count; ++i)
        {
            CellData cellData = m_CellDataList[i];
            Vector2 tempPos = pos - new Vector2(cellData.cellSize.x * 0.5f, -cellData.cellSize.y * 0.5f);

            if (m_IsVertical)
            {
                if (tempPos.y >= cellData.cellPos.y - cellData.cellSize.y * 0.5f &&
                    tempPos.y <= cellData.cellPos.y + cellData.cellSize.y * 0.5f)
                {
                    return i;
                }
            }
            else
            {
                if (tempPos.x >= cellData.cellPos.x - cellData.cellSize.x * 0.5f &&
                    tempPos.x <= cellData.cellPos.x + cellData.cellSize.x * 0.5f)
                {
                    return i;
                }
            }
        }

        return m_CellDataList.Count - 1;
    }

    void RefreshVerticalSize()
    {
        rowCount = cellCount;
        colCount = 1;

        int contentHeight = 0;
        m_CellDataList.Clear();

        for (int i = 0; i < cellCount; ++i)
        {
            int cellHeight = this.onGetCellHeight != null ? this.onGetCellHeight(i) : base.cellHeight;

            CellData data = new CellData();
            data.cellSize = new Vector2(cellWidth, cellHeight);
            data.cellPos = new Vector2(0, -contentHeight);
            m_CellDataList.Add(data);

            contentHeight += cellHeight;
        }

        int veiwWidth = Mathf.CeilToInt(this.viewport.rect.width);
        int contentWidth = veiwWidth - padding.left - padding.right;

        contentHeight = contentHeight + padding.top + padding.bottom;

        this.content.sizeDelta = new Vector2(contentWidth, contentHeight);

        if (disableDragIfFits)
        {
            bool fits = this.content.sizeDelta.y <= this.viewport.rect.height;
            this.movementType = (fits ? MovementType.Clamped : MovementType.Elastic);
        }
    }

    void RefreshHorizontalSize()
    {
        rowCount = 1;
        colCount = cellCount;

        int contentWidth = 0;
        m_CellDataList.Clear();

        for (int i = 0; i < cellCount; ++i)
        {
            int cellWidth = this.onGetCellWidth != null ? this.onGetCellWidth(i) : CellWidth;

            CellData data = new CellData();
            data.cellSize = new Vector2(cellWidth, cellHeight);
            data.cellPos = new Vector2(contentWidth, 0);
            m_CellDataList.Add(data);

            contentWidth += cellWidth;
        }

        contentWidth = contentWidth + padding.left + padding.right;

        int viewHeight = Mathf.CeilToInt(this.viewport.rect.height);
        int contentHeight = viewHeight - padding.top - padding.bottom;

        this.content.sizeDelta = new Vector2(contentWidth, contentHeight);

        if (disableDragIfFits)
        {
            bool fits = this.content.sizeDelta.x <= this.viewport.rect.width;
            this.movementType = (fits ? MovementType.Clamped : MovementType.Elastic);
        }
    }

    void RefreshSizeAndPos()
    {
        OnRefreshSize();

        for (int i = 0; i < cellCount; ++i)
        {
            Transform cellTrans = GetCell(i);
            if (cellTrans == null)
            {
                continue;
            }

            if (cellTrans is RectTransform)
            {
                (cellTrans as RectTransform).anchoredPosition = m_IsVertical ? OnGetPos(i, 0) : OnGetPos(0, i);
            }
        }
    }
}
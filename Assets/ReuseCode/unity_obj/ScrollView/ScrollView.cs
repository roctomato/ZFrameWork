using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[AddComponentMenu("UI/Scroll View", 38)]
[SelectionBase]
[ExecuteInEditMode]
[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
[XLua.LuaCallCSharp]
public class ScrollView : ScrollRect
{
    [SerializeField]
    protected RectOffset padding = new RectOffset();
    [SerializeField]
    protected GameObject cellTemplate;
    [SerializeField]
    protected int maxWidth;
    [SerializeField]
    protected int cellWidth = 50;
    [SerializeField]
    protected int cellHeight = 50;
    [SerializeField]
    protected int maxCreateCountPerFrame = 1;
    [SerializeField]
    protected bool disableDragIfFits;
    [SerializeField]
    protected bool isResizeCell = true;  // 是否重新设置cell大小
    [SerializeField]
    protected GameObject notDataObject;   // 无数据时显示节点

    protected int cellCount;
    protected int colCount;
    protected int rowCount;

    List<GameObject> allCells = new List<GameObject>();
    GameObjectCache cellCache;

    List<GameObject> destroyingList = new List<GameObject>();

    int showStartIndex = -1;
    int showEndIndex = -1;

    bool needCreateNew;
    bool isDragging;
    bool dragEnable = true;

    DragStateEvent m_DragStateEvent = new DragStateEvent();

    public class DragStateEvent : UnityEvent<bool> { };
    [XLua.CSharpCallLua]
    public delegate void CellUpdateDelegate(Transform cell, int index);

    // 元素刷新回调
    public CellUpdateDelegate onCellUpdate;

    public RectOffset Padding { get { return padding; } }

    // 拖拽事件
    public DragStateEvent onDragStateChanged { get { return m_DragStateEvent; } }

    public void SetCellUpdate(CellUpdateDelegate cellUpdateEvent)
    {
        onCellUpdate = cellUpdateEvent;
    }

    // 元素模版
    public GameObject CellTemplate
    {
        get { return cellTemplate; }
        set
        {
            if (Application.isPlaying)
            {
                Debug.LogErrorFormat(null,"Scroll view can only set cell template in editor mode.");
                return;
            }

            cellTemplate = value;

            if (cellTemplate != null)
            {
                cellTemplate.SetActive(false);
            }
        }
    }

    // 元素总数量
    public int CellCount
    {
        get { return cellCount; }
    }

    public int CellWidth
    {
        get { return cellWidth; }
    }

    // 每帧最大创建数量
    public int MaxCreateCountPerFrame
    {
        get { return maxCreateCountPerFrame; }
        set { maxCreateCountPerFrame = value; }
    }

    public bool IsDragging
    {
        get { return isDragging; }
    }

    /// <summary>
    /// 最大宽度
    /// </summary>
    public int MaxWidth
    {
        get { return maxWidth; }
        set { maxWidth = value; }
    }

    /// <summary>
    /// 是否开启滑动
    /// </summary>
    public bool scrollEnable
    {
        get { return dragEnable; }
        set { dragEnable = value; }
    }

    public int defaultCellHeight
    {
        get { return cellHeight; }
    }

    public int ShowStartIndex
    {
        get { return Math.Max(showStartIndex, 0); }
    }

    public int ShowEndIndex
    {
        get { return Math.Min(showEndIndex, cellCount - 1); }
    }

    /// <summary>
    /// 刷新全部列表(设置resetPosition和JumpTo有冲突，如果Refresh后要调用JumpToStart或JumpToEnd请设置resetPosition为false)
    /// </summary>
    public void Init(int count, bool resetPosition = true)
    {
        count = Mathf.Max(count, 0);
        OnRefreshAll(count, resetPosition);
    }

    // 重新填充所有元素
    public void Refill()
    {
        OnRefreshAll(cellCount, false);
    }

    // 重新填充某个元素
    public void RefillCell(int index)
    {
        Transform cell = GetCell(index);
        if (cell != null && this.onCellUpdate != null)
        {
            this.onCellUpdate(cell, index);
        }

        OnRefillCell(index);
    }

    // 获取元素对象，该元素不在显示区间时，会不存在的
    public Transform GetCell(int index)
    {
        if (index < 0 || index >= allCells.Count)
        {
            return null;
        }

        GameObject cell = allCells[index];

        if (cell == null)
        {
            return null;
        }

        return cell.transform;
    }

    public List<GameObject> GetAllCell()
    {
        return allCells;
    }

    // 跳到该元素起始位置
    public void JumpToStart(int index)
    {
        if (colCount <= 0)
        {
            return;
        }

        float horizontalPosition, verticalPosition;
        GetCellStartNormalizedPosition(index, out horizontalPosition, out verticalPosition);
        this.horizontalNormalizedPosition = horizontalPosition;
        this.verticalNormalizedPosition = verticalPosition;

        // 刷新显示
        RefreshShowCell(false);
    }

    public void ScrollToStart(int index, float duration = 0.5f)
    {
        if (colCount <= 0)
        {
            return;
        }

        float horizontalPosition, verticalPosition;
        GetCellStartNormalizedPosition(index, out horizontalPosition, out verticalPosition);

        content.DOKill();
        DOTween.To(() => horizontalNormalizedPosition, x => horizontalNormalizedPosition = x,
            horizontalPosition, duration).SetTarget(content);
        DOTween.To(() => verticalNormalizedPosition, x => verticalNormalizedPosition = x,
            verticalPosition, duration).SetTarget(content);
    }

    void GetCellStartNormalizedPosition(int index, out float endX, out float endY)
    {
        endX = this.horizontalNormalizedPosition;
        endY = this.verticalNormalizedPosition;

        if (colCount <= 0)
        {
            return;
        }

        int row = index / colCount;
        int col = index % colCount;

        Vector2 pos = OnGetPos(row, col);

        Vector2 contentSize = this.content.sizeDelta;
        Vector2 viewSize = this.viewRect.rect.size;
        Vector2 cellSize;
        if (!OnGetWidthHeight(row, col, out cellSize))
        {
            cellSize.x = cellWidth;
            cellSize.y = cellHeight;
        }


        if (this.vertical)
        {
            float startHeight = -pos.y - cellSize.y * 0.5f - padding.top;
            float moveHeight = contentSize.y - viewSize.y;

            if (moveHeight > 0)
            {
                if (startHeight > moveHeight)
                {
                    endY = 0;
                }
                else
                {
                    endY = 1f - startHeight / moveHeight;
                }
            }
        }

        if (this.horizontal)
        {
            float startWidth = pos.x - cellSize.x * 0.5f - padding.left;
            float moveWidth = contentSize.x - viewSize.x;

            if (moveWidth > 0)
            {
                if (startWidth > moveWidth)
                {
                    endX = 1;
                }
                else
                {
                    endX = startWidth / moveWidth;
                }
            }
        }
    }

    // 跳到该元素终止位置
    public void JumpToEnd(int index)
    {
        if (colCount <= 0)
        {
            return;
        }

        int row = index / colCount;
        int col = index % colCount;

        Vector2 pos = OnGetPos(row, col);

        Vector2 contentSize = this.content.sizeDelta;
        Vector2 viewSize = this.viewRect.rect.size;
        Vector2 cellSize;
        if (!OnGetWidthHeight(row, col, out cellSize))
        {
            cellSize.x = cellWidth;
            cellSize.y = cellHeight;
        }

        if (this.vertical)
        {
            float startHeight = -pos.y + cellSize.y * 0.5f - padding.top - viewSize.y;
            float moveHeight = contentSize.y - viewSize.y;

            if (moveHeight > 0)
            {
                if (startHeight < 0)
                {
                    this.verticalNormalizedPosition = 1;
                }
                else if (startHeight > moveHeight)
                {
                    this.verticalNormalizedPosition = 0;
                }
                else
                {
                    this.verticalNormalizedPosition = 1f - startHeight / moveHeight;
                }
            }
        }

        if (this.horizontal)
        {
            float startWidth = pos.x + cellSize.x * 0.5f - padding.left - viewSize.x;
            float moveWidth = contentSize.x - viewSize.x;

            if (moveWidth > 0)
            {
                if (startWidth < 0)
                {
                    this.horizontalNormalizedPosition = 0;
                }
                else if (startWidth > moveWidth)
                {
                    this.horizontalNormalizedPosition = 1;
                }
                else
                {
                    this.horizontalNormalizedPosition = startWidth / moveWidth;
                }
            }
        }

        // 刷新显示
        RefreshShowCell(false);
    }

    /// <summary>
    /// 显示内容超过显示范围时, 开启竖直滚动; 否则关闭竖直滚动
    /// </summary>
    public void ClampVerticalScrolling()
    {
        vertical = viewport.rect.height < content.rect.height;
    }

    /// <summary>
    /// 显示内容超过显示范围时, 开启水平滚动; 否则关闭水平滚动
    /// </summary>
    public void ClampHorizontalScrolling()
    {
        horizontal = viewport.rect.width < content.rect.width;
    }

    protected override void Awake()
    {
        base.Awake();

        ValidateTransfroms();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        if (needCreateNew)
        {
            UpdateCreateNew();
        }

        if (!Application.isPlaying)
        {
            for (int i = 0; i < destroyingList.Count; ++i)
            {
                if (destroyingList[i] != null)
                {
                    GameObject.DestroyImmediate(destroyingList[i]);
                }
            }

            destroyingList.Clear();
        }
    }

    protected virtual void OnRefreshAll(int count, bool resetPosition)
    {
        if (cellTemplate == null)
        {
            //AppLog.Warn("[ScrollView:Refresh]template is null.");
            return;
        }

        // 回收全部使用过的
        ReleaseAllUsed();

        // 重新设置数量
        ResizeCellCount(count);

        // 刷新无数据节点显示
        RefreshNotDataObj(count);

        // 刷新大小
        OnRefreshSize();

        if (maxCreateCountPerFrame <= 0)
        {
            maxCreateCountPerFrame = colCount;
        }

        if (resetPosition)
        {
            JumpToStart(0);
#if UNITY_2018
            if (horizontal == true)
            {
                this.content.anchoredPosition = new Vector2(softnessClip.x, 0);
            }
            else
            {
                this.content.anchoredPosition = new Vector2(0, -softnessClip.y);
            }
#endif
            //this.content.anchoredPosition = Vector2.zero;
            //JumpToStart(0);
        }

        RefreshShowCell(true);
    }

    protected virtual void OnRefillCell(int index)
    {

    }

    protected virtual void OnRefreshSize()
    {
        int maxWidth = this.maxWidth;
        if (maxWidth <= 0)
        {
            maxWidth = Mathf.CeilToInt(this.viewport.rect.width);
        }

        int width = maxWidth - padding.left - padding.right;
        colCount = Mathf.CeilToInt(width / (float)cellWidth) - 1;
        colCount = Mathf.Clamp(colCount, 1, 1000);
        rowCount = Mathf.CeilToInt(cellCount / (float)colCount);

        int maxHeight = rowCount * cellHeight + padding.top + padding.bottom;
        this.content.sizeDelta = new Vector2(maxWidth, maxHeight);

        if (Application.isPlaying && disableDragIfFits)
        {
            bool fits = (!this.horizontal || this.content.sizeDelta.x <= this.viewport.rect.width) &&
                (!this.vertical || this.content.sizeDelta.y <= this.viewport.rect.height);
            this.movementType = (fits ? MovementType.Clamped : MovementType.Elastic);
        }
    }

    protected virtual Vector2 OnGetPos(int row, int col)
    {
        int x = col * cellWidth + padding.left;
        int y = row * cellHeight + padding.top;

        return new Vector2(x + cellWidth * 0.5f, -y - cellHeight * 0.5f);
    }

    /// <summary>
    /// 获取cell的宽和高
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns>true:需要刷新cell的宽和高; fasle:不需要刷新cell的宽高</returns>
    protected virtual bool OnGetWidthHeight(int row, int col, out Vector2 sizeDelta)
    {
        sizeDelta = Vector2.zero;

        // 默认不需要刷新
        return false;
    }

    protected virtual int OnGetPosIndex(Vector2 pos)
    {
        int col = (int)((pos.x - padding.left) / cellWidth);
        int row = (int)((-pos.y - padding.top) / cellHeight);

        col = Mathf.Clamp(col, 0, colCount - 1);
        row = Mathf.Clamp(row, 0, rowCount - 1);

        int index = row * colCount + col;

        return Mathf.Max(index, -1);
    }

    protected virtual bool OnCreateCell(int row, int col)
    {
        int cellIndex = row * colCount + col;
        return CreateCellWithIndex(cellIndex, row, col);
    }

    protected bool CreateCellWithIndex(int cellIndex, int row, int col)
    {
        if (cellIndex < 0 || cellIndex >= allCells.Count)
        {
            return false;
        }

        if (allCells[cellIndex] != null)
        {
            return false;
        }

        GameObject go = cellCache.Get();
        allCells[cellIndex] = go;

        RectTransform rectTransfrom = go.transform as RectTransform;
        rectTransfrom.localRotation = cellTemplate.transform.localRotation;
        rectTransfrom.localScale = cellTemplate.transform.localScale;
        rectTransfrom.anchoredPosition = OnGetPos(row, col);
        Vector2 sizeDelta;
        if (isResizeCell && OnGetWidthHeight(row, col, out sizeDelta))
        {
            rectTransfrom.sizeDelta = sizeDelta;
        }

        if (this.onCellUpdate != null)
        {
            //try
            //{
                this.onCellUpdate(go.transform, cellIndex);
            //}
            //catch (System.Exception ex)
            //{
            //    ZLog.E(this, ex.StackTrace);
            //    ZLog.E(this, ex.Message);
                //AppLog.Error(ex.StackTrace);
                //AppLog.Error(ex.Message);
            //}
        }

        return true;
    }

    protected ScrollView()
    {
        this.onValueChanged.AddListener(OnValueChanged);
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        if (Application.isPlaying)
        {
            return;
        }

        ValidateTransfroms();
        Init(cellCount, false);
    }
#endif

    public override void OnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (!dragEnable)
        {
            return;
        }

        base.OnBeginDrag(eventData);
        isDragging = true;
        m_DragStateEvent.Invoke(true);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (!dragEnable)
        {
            return;
        }

        base.OnDrag(eventData);
    }

    public override void OnEndDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (!dragEnable)
        {
            return;
        }

        base.OnEndDrag(eventData);
        isDragging = false;
        m_DragStateEvent.Invoke(false);
    }

    void OnValueChanged(Vector2 value)
    {
        RefreshShowCell(false);
    }

    void ValidateTransfroms()
    {
        if (cellTemplate != null)
        {
            cellTemplate.SetActive(false);

            RectTransform rectTransform = cellTemplate.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchorMin = new Vector2(0.0f, 1.0f);
                rectTransform.anchorMax = new Vector2(0.0f, 1.0f);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
            }
        }

        if (viewport != null)
        {
            viewport.anchorMin = new Vector2(0.0f, 0.0f);
            viewport.anchorMax = new Vector2(1.0f, 1.0f);
            viewport.pivot = new Vector2(0.0f, 1.0f);
        }

        if (content != null)
        {
            content.anchorMin = new Vector2(0.0f, 1.0f);
            content.anchorMax = new Vector2(0.0f, 1.0f);
            content.pivot = new Vector2(0.0f, 1.0f);
        }
    }

    bool IsInShowRange(int index, int startIndex, int endIndex)
    {
        int row = index / colCount;
        int col = index % colCount;

        int startRow = startIndex / colCount;
        int startCol = startIndex % colCount;

        if (row < startRow || col < startCol)
        {
            return false;
        }

        int endRow = endIndex / colCount;
        int endCol = endIndex % colCount;

        if (row > endRow || col > endCol)
        {
            return false;
        }

        return true;
    }

    void RefreshShowCell(bool forceUpdate)
    {
        if (cellCount <= 0)
        {
            return;
        }

        // 获取新的显示区间
        int startIndex;
        int endIndex;
        CalcShowRange(out startIndex, out endIndex);

        // 显示区间未改变
        if (!forceUpdate && showStartIndex == startIndex && showEndIndex == endIndex)
        {
            return;
        }

        // 回收无效的
        for (int i = showStartIndex; i <= showEndIndex; ++i)
        {
            if (i < 0 || i >= allCells.Count)
            {
                continue;
            }

            if (!IsInShowRange(i, startIndex, endIndex))
            {
                if (allCells[i] != null)
                {
                    ReleaseCell(allCells[i]);
                    allCells[i] = null;
                }
            }
        }

        // 记录新的显示区间
        showStartIndex = startIndex;
        showEndIndex = endIndex;

        // 创建新的
        UpdateCreateNew();
    }

    void CalcShowRange(out int startIndex, out int endIndex)
    {
        // 内容起始位置
        Vector2 startPos = this.content.anchoredPosition * -1;
        // 内容结束位置
        Vector2 endPos = startPos + new Vector2(this.viewport.rect.width, -this.viewport.rect.height);

        startIndex = OnGetPosIndex(startPos);
        endIndex = OnGetPosIndex(endPos);
    }

    /// <summary>
    /// 刷新无数据节点
    /// </summary>
    void RefreshNotDataObj(int dataCount)
    {
        if (notDataObject != null)
        {
            notDataObject.SetActive(dataCount <= 0);
        }
    }

    void ResizeCellCount(int cellCount)
    {
        if (cellCache == null)
        {
            cellCache = new GameObjectCache();
            cellCache.autoUnactive = false;
            cellCache.Init(this.content, cellTemplate);
        }

        if (allCells.Count < cellCount)
        {
            int addCount = cellCount - allCells.Count;
            for (int i = 0; i < addCount; ++i)
            {
                allCells.Add(null);
            }
        }
        else if (allCells.Count > cellCount)
        {
            for (int i = allCells.Count - 1; i >= cellCount; --i)
            {
                ReleaseCell(allCells[i]);
                allCells.RemoveAt(i);
            }
        }

        this.cellCount = allCells.Count;
    }

    void UpdateCreateNew()
    {
        if (cellCount <= 0)
        {
            return;
        }

        needCreateNew = true;

        int startRow = showStartIndex / colCount;
        int startCol = showStartIndex % colCount;

        int endRow = showEndIndex / colCount;
        int endCol = showEndIndex % colCount;

        int createCount = maxCreateCountPerFrame + cellCache.count;

        for (int r = startRow; r <= endRow; ++r)
        {
            if (r < 0)
            {
                continue;
            }

            for (int c = startCol; c <= endCol; ++c)
            {
                if (c < 0)
                {
                    continue;
                }

                if (OnCreateCell(r, c))
                {
                    if (Application.isPlaying && --createCount <= 0)
                    {
                        return;
                    }
                }
            }
        }

        needCreateNew = false;
    }

    void ReleaseAllUsed()
    {
        for (int i = allCells.Count - 1; i >= 0; --i)
        {
            if (allCells[i] != null)
            {
                ReleaseCell(allCells[i]);
                allCells[i] = null;
            }
        }
    }

    void ReleaseCell(GameObject cell)
    {
        if (cell == null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            if (cellCache != null)
            {
                cellCache.Release(cell);
            }
        }
        else
        {
            destroyingList.Add(cell);
        }
    }
}
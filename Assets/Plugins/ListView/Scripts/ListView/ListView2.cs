﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ListView2 : UIBehaviour
{
    public delegate int ListViewDataCount();
    public delegate void ListViewOnChange(RectTransform item, int index);
    public delegate void ListViewSetHighLight(RectTransform item, bool selected);
    static ListViewDataCount DefaultGetDataCount = () => 0;
    static ListViewSetHighLight DefaultSetHighLight = (rectTrs, selected) => { };
    static ListViewOnChange DefaultOnChange = (rectTrs, index) => { };
    public ListViewDataCount GetDataCount = DefaultGetDataCount;
    ListViewOnChange OnChange = DefaultOnChange;
    ListViewSetHighLight SetHighLight = DefaultSetHighLight;
    [SerializeField]
    bool IsVertical = false;
    [SerializeField]
    bool FullLine = false;
    [SerializeField]
    RectTransform _baseItem;
    [SerializeField]
    int maxLineCount = 0;
    [SerializeField]
    ScrollRect _scroll;


    Vector2 itemSize;
    Vector2 itemSpace;
    RectOffset Padding;
    TextAnchor _alignment;

    public DelegateOnClickItem OnClickItem;

    private float _contentWidth;
    private float _contentHeight;
    private int _rowCellCount;
    private int _colCellCount;
    private List<RectTransform> _items = new List<RectTransform>();
    private List<RectTransform> _cacheItems = new List<RectTransform>();
    private Transform _cacheRoot;
    private int _curFirstIndex;
    private GridLayoutGroup gridLayout;
    public void SetHighLightCallback(ListViewSetHighLight setHighLight = null)
    {
       if(setHighLight != null)
        {
            this.SetHighLight = setHighLight;
        }
    }
    public void Refresh(ListViewDataCount getDataCount, ListViewOnChange onChange)
    {
        this.GetDataCount = getDataCount;
        this.OnChange = onChange;
        DataChanged = true;
    }
    public void Refresh()
    {
        DataChanged = true;
    }
    protected override void Awake()
    {
        base.Awake();
        (_baseItem.transform as RectTransform).sizeDelta = itemSize;
        _baseItem.gameObject.SetActive(false);
        _scroll.onValueChanged.AddListener(OnScroll);
        _cacheRoot = (new GameObject("CacheRoot")).GetComponent<Transform>();
        _cacheRoot.SetParent(transform, false);
        _cacheRoot.gameObject.SetActive(false);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        ResetUI();
    }
    void ResetUI()
    {
        gridLayout = _scroll.content.GetComponent<GridLayoutGroup>();
        itemSize = gridLayout.cellSize;
        itemSpace = gridLayout.spacing;
        _alignment = gridLayout.childAlignment;
        Padding = gridLayout.padding;
        gridLayout.enabled = false;
        _scroll.vertical = IsVertical;
        _scroll.horizontal = !IsVertical;
        if (FullLine)
        {
            if (IsVertical)
            {
                itemSize.x = _scroll.viewport.rect.size.x - Padding.left - Padding.right;
            }
            else
            {
                itemSize.y = _scroll.viewport.rect.size.y - Padding.top - Padding.bottom;
            }
        }
        CalcRowAndCol();
        ResetContentSize();
        UpdateVisibleItem(true);
        ResetItemsSize();
    }
    void ResetItemsSize()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            (_items[i].transform as RectTransform).sizeDelta = itemSize;
        }
        for (int i = 0; i < _cacheItems.Count; i++)
        {
            (_cacheItems[i].transform as RectTransform).sizeDelta = itemSize;
        }
    }
    void CalcRowAndCol()
    {
        Vector2 viewPortSize = _scroll.viewport.rect.size;
        if (IsVertical)
        {
            float rowCellCount = (viewPortSize.x - Padding.left - Padding.right + itemSpace.x) / (itemSize.x + itemSpace.x);
            _rowCellCount = Mathf.FloorToInt(rowCellCount);
            float colCellCount = 1.0f * GetDataCount() / _rowCellCount;
            _colCellCount = Mathf.CeilToInt(colCellCount);
        }
        else
        {
            float colCellCount = (viewPortSize.y - Padding.top - Padding.bottom + itemSpace.y) / (itemSize.y + itemSpace.y);
            _colCellCount = Mathf.FloorToInt(colCellCount);
            float rowCellCount = 1.0f * GetDataCount() / _colCellCount;
            _rowCellCount = Mathf.CeilToInt(rowCellCount);
        }
        if (_colCellCount < 1) _colCellCount = 1;
        if (_rowCellCount < 1) _rowCellCount = 1;
    }
    void ResetContentSize()
    {
        float sizeX, sizeY;
        if (IsVertical)
        {
            sizeX = _scroll.viewport.rect.width;
            sizeY = _colCellCount * (itemSize.y + itemSpace.y) - itemSpace.y + Padding.top + Padding.bottom;

        }
        else
        {
            sizeX = _rowCellCount * (itemSize.x + itemSpace.x) - itemSpace.x + Padding.right + Padding.left;
            sizeY = _scroll.viewport.rect.height;

        }
        //_scroll.content.localPosition = Vector3.zero;
        _scroll.content.sizeDelta = new Vector2(sizeX, sizeY);

        CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(_scroll);
    }
    int curVisibleStartIndex = 0;
    int curVisibleEndIndex = -1;
    int calcVisibleMinIndex(float visibleMax, float space, float padding, int rowCellCount, float itemsize)
    {
        return Mathf.CeilToInt((visibleMax + space - padding + 0.1f) / (itemsize + space) - 1) * rowCellCount;
    }
    int CalcVisibleMaxIndex(float visibleMin, float space, float padding, int rowCellCount, float itemsize)
    {
        return Mathf.CeilToInt((visibleMin - padding - 0.1f) / (itemsize + space)) * rowCellCount - 1;
    }
    void UpdateVisibleItem(bool forceUpdate)
    {
        int newVisibleStartIndex;
        int newVisibleEndIndex;
        Vector2 viewPortSize = _scroll.viewport.rect.size;
        Vector2 contentSize = _scroll.content.rect.size;
        if (IsVertical)
        {
            float visibleStartY = (1 - _scroll.verticalNormalizedPosition) * (contentSize.y - viewPortSize.y);
            float visibleEndY = visibleStartY + viewPortSize.y;

            newVisibleStartIndex = calcVisibleMinIndex(visibleStartY, itemSpace.y, Padding.top, _rowCellCount, itemSize.y);
            newVisibleEndIndex = CalcVisibleMaxIndex(visibleEndY, itemSpace.y, Padding.top, _rowCellCount, itemSize.y);
        }
        else
        {
            float visibleStartX = (_scroll.horizontalNormalizedPosition) * (contentSize.x - viewPortSize.x);
            newVisibleStartIndex = Mathf.CeilToInt((visibleStartX + itemSpace.x - Padding.left + 0.1f) / (itemSize.x + itemSpace.x) - 1) * _colCellCount;
            float visibleEndX = visibleStartX + viewPortSize.x;
            newVisibleEndIndex = Mathf.CeilToInt((visibleEndX + itemSpace.x - Padding.left) / (itemSize.x + itemSpace.x)) * _colCellCount;

            newVisibleStartIndex = calcVisibleMinIndex(visibleStartX, itemSpace.x, Padding.left, _colCellCount, itemSize.x);
            newVisibleEndIndex = CalcVisibleMaxIndex(visibleEndX, itemSpace.x, Padding.left, _colCellCount, itemSize.x);
        }
        newVisibleStartIndex = Mathf.Clamp(newVisibleStartIndex, 0, GetDataCount() - 1);
        newVisibleEndIndex = Mathf.Clamp(newVisibleEndIndex, 0, GetDataCount() - 1) ;
        if (GetDataCount() == 0)
        {
            newVisibleStartIndex = 0;
            newVisibleEndIndex = -1;
        }

        if (!forceUpdate && curVisibleStartIndex == newVisibleStartIndex && curVisibleEndIndex == newVisibleEndIndex) return;

        for (int i = curVisibleStartIndex; i <= curVisibleEndIndex; i++)
        {
            if (i < newVisibleStartIndex)
            {
                RectTransform item = _items[0];
                _items.Remove(item);
                RecycleItem(item);

            }
        }
        for (int i = curVisibleEndIndex; i >= curVisibleStartIndex; i--)
        {
            if (i > newVisibleEndIndex)
            {
                RectTransform item = _items[_items.Count - 1];
                _items.Remove(item);
                RecycleItem(item);
            }
        }
        for (int i = newVisibleStartIndex; i <= newVisibleEndIndex; i++)
        {
            if (i < 0) continue;
            if ((i < curVisibleStartIndex || i > curVisibleEndIndex))
            {
                RectTransform item = GetItemFromCache();
                item.transform.SetParent(_scroll.content, false);
                _items.Insert(i - newVisibleStartIndex, item);
                bool selected;
                if (IsMultiSelect)
                {
                    selected = _selectedIndex == i;
                }
                else
                {
                    selected = MultiSelectedIndexs.Contains(i);
                }
                OnChange(item, i);
                if (!forceUpdate)
                {
                    UpdateItemPos(item, i);
                }
            }
            if (forceUpdate)
            {
                OnChange(_items[i - newVisibleStartIndex], i);
                UpdateItemPos(_items[i - newVisibleStartIndex], i);
            }
        }
        curVisibleStartIndex = newVisibleStartIndex;
        curVisibleEndIndex = newVisibleEndIndex;
        for (int i = curVisibleStartIndex; i <= curVisibleEndIndex; i++)
        {
            UpdateSelected(i);
        }
        if (onUpdateVisibleItem != null)
        {

            onUpdateVisibleItem(curVisibleStartIndex, curVisibleEndIndex);
        }
    }
    public delegate void OnUpdateVisibleItem(int curVisibleMin, int curVisibleMax);
    public OnUpdateVisibleItem onUpdateVisibleItem;
    void UpdateItemPos(RectTransform item, int index)
    {
        float posX, posY;
        CalcItemPos(index, out posX, out posY);
        item.transform.localPosition = new Vector2(posX, posY) + new Vector2(_scroll.content.pivot.x * _scroll.content.rect.width + itemSize.x * item.pivot.x, (1 - _scroll.content.pivot.y) * _scroll.content.rect.height + itemSize.y * (-1 + item.pivot.y));
    }
    void CalcItemPos(int index, out float posX, out float posY)
    {
        if (IsVertical)
        {
            posX = GetStartPos() + Padding.left + index % _rowCellCount * (itemSize.x + itemSpace.x);
            posY = -(Padding.top + index / _rowCellCount * (itemSize.y + itemSpace.y));
        }
        else
        {
            posY = GetStartPos() - (Padding.top + index % _colCellCount * (itemSize.y + itemSpace.y));
            posX = (Padding.left + index / _colCellCount * (itemSize.x + itemSpace.x));
        }
    }
    private void UpdateSelected(int i)
    {
        if (i >= curVisibleStartIndex && i <= curVisibleEndIndex)
        {
            bool selected;
            if (!IsMultiSelect)
            {
                selected = SelectedIndex == i;
            }
            else
            {
                selected = MultiSelectedIndexs.Contains(i);
            }
            SetHighLight(_items[i - curVisibleStartIndex], selected);
        }
    }
    private void UpdateSelected(int i, bool selected)
    {
        if (i >= curVisibleStartIndex && i <= curVisibleEndIndex)
        {
            SetHighLight(_items[i - curVisibleStartIndex], selected);
        }
    }
    float GetStartPos()
    {
        if (IsVertical)
        {
            switch (_alignment)
            {
                case TextAnchor.UpperLeft:
                case TextAnchor.MiddleLeft:
                case TextAnchor.LowerLeft: return 0;
                case TextAnchor.UpperCenter:
                case TextAnchor.MiddleCenter:
                case TextAnchor.LowerCenter: return _scroll.viewport.rect.width / 2 - (itemSize.x * _rowCellCount + itemSpace.x * (_rowCellCount - 1)) / 2 - Padding.left;
                default: return _scroll.viewport.rect.width - itemSize.x * _rowCellCount - itemSpace.x * (_rowCellCount - 1) - Padding.left - Padding.right;
            }
        }
        else
        {
            switch (_alignment)
            {
                case TextAnchor.UpperLeft:
                case TextAnchor.UpperCenter:
                case TextAnchor.UpperRight: return 0;
                case TextAnchor.MiddleLeft:
                case TextAnchor.MiddleCenter:
                case TextAnchor.MiddleRight: return -_scroll.viewport.rect.height / 2 + (itemSize.y * _colCellCount + itemSpace.y * (_colCellCount - 1)) / 2 + Padding.top;
                default: return -_scroll.viewport.rect.height + itemSize.y * _colCellCount + itemSpace.y * (_colCellCount - 1) + Padding.top + Padding.bottom;
            }
        }
    }
    RectTransform GetItemFromCache()
    {
        if (_cacheItems.Count <= 0)
        {
            RectTransform item = (GameObject.Instantiate<GameObject>(_baseItem.gameObject)).GetComponent<RectTransform>();
            item.transform.SetParent(_cacheRoot, false);
            (item.transform as RectTransform).sizeDelta = itemSize;
            item.gameObject.SetActive(true);

            return item;
        }
        else
        {
            RectTransform item = _cacheItems[0];
            _cacheItems.RemoveAt(0);
            return item;
        }
    }
    void RecycleItem(RectTransform item)
    {
        _cacheItems.Add(item);
        item.transform.SetParent(_cacheRoot, false);

    }

    private void OnScroll(Vector2 arg0)
    {
        UpdateVisibleItem(false);
    }
    private bool SizeChanged = true;
    private bool DataChanged = true;
    private bool PosChanged = true;
    private bool SelectChanged = false;
    public bool TestRefreshLayout = false;
    private Vector2 lastViewPortSize;
    private void LateUpdate()
    {
        if (lastViewPortSize != _scroll.viewport.rect.size)
        {
            lastViewPortSize = _scroll.viewport.rect.size;
            ResetUI();
        }
    }
    public void Update()
    {
        if (lastViewPortSize != _scroll.viewport.rect.size)
        {
            SizeChanged = true;
            lastViewPortSize = _scroll.viewport.rect.size;
        }
        if (SizeChanged)
        {
            ResetUI();
            SizeChanged = false;
            if (Moving)
            {
                ScrollTo(targetIndex);
            }
        }
        if (DataChanged)
        {
            ResetUI();
            DataChanged = false;
        }
        if (TestRefreshLayout)
        {
            TestRefreshLayout = false;
            ResetUI();
            _scroll.content.localPosition = Vector3.zero;
        }
        if (Moving)
        {
            movingPos = Vector2.Lerp(movingPos, targetPos, 5.0f * Time.deltaTime);
            _scroll.normalizedPosition = movingPos;
            if ((movingPos - targetPos).magnitude < 0.0001f)
            {
                Moving = false;
            }
        }
    }
    public bool IsMultiSelect = false;
    private int _selectedIndex = -1;
    public int SelectedIndex
    {
        get
        {
            return _selectedIndex;
        }
    }
    public List<int> MultiSelectedIndexs;

    public void Select(int index)
    {
        int lastSelected = _selectedIndex;
        if (IsMultiSelect == true)
        {
            if (!MultiSelectedIndexs.Contains(index))
            {
                MultiSelectedIndexs.Add(index);
                UpdateSelected(index, true);
            }
            else
            {
                MultiSelectedIndexs.Remove(index);
                UpdateSelected(index, false);
            }
        }
        else
        {
            _selectedIndex = index;
            UpdateSelected(_selectedIndex);
            UpdateSelected(lastSelected);
        }
    }
    public void DeselectedAll()
    {
        _selectedIndex = -1;
        for (int i = 0; i < MultiSelectedIndexs.Count; i++)
        {
            UpdateSelected(i, false);
        }
        MultiSelectedIndexs.Clear();
    }
    bool Moving = false;
    Vector2 targetPos = new Vector2();
    Vector2 movingPos;
    int targetIndex;
    public void ScrollTo(int index)
    {
        movingPos = _scroll.normalizedPosition;
        Moving = true;
        targetIndex = index;
        float posX, posY;
        CalcItemPos(index, out posX, out posY);
        if (IsVertical)
        {
            targetPos.x = _scroll.normalizedPosition.x;
            targetPos.y = (_scroll.content.rect.height + (posY - _scroll.viewport.rect.height / 2 - itemSize.y / 2)) / (_scroll.content.rect.height - _scroll.viewport.rect.height);
            targetPos.y = Mathf.Clamp(targetPos.y, 0, 1);
        }
        else
        {
            targetPos.x = (posX - _scroll.viewport.rect.width / 2 + itemSize.x / 2) / (_scroll.content.rect.width - _scroll.viewport.rect.width);
            targetPos.y = _scroll.normalizedPosition.y;
            targetPos.x = Mathf.Clamp(targetPos.x, 0, 1);
            //_scroll.horizontalNormalizedPosition = (posX-_scroll.viewport.rect.width/2+itemSize.x/2) / (_scroll.content.rect.width - _scroll.viewport.rect.width);
        }
    }
    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        ResetUI();
    }
    public void ScrollToZero()
    {
        _scroll.normalizedPosition = Vector2.zero;
        _scroll.content.anchoredPosition = Vector2.zero;
    }

}

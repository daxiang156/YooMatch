//===============================================================
//描述：背包物品无限滚动(content顶点设置在左上,ScrollView上面挂mask，ScrollView作为content父节点)
//作者：朱素云
//创建时间：2020/12/03 18:19:18
//版本：v 1.0
//===============================================================
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

namespace ET
{
    public class ItemCell : object
    {
        public ETListBaseItem item;
        public int _index;//项目中可以通过_index取到数据层对应的对象给item赋值
    }
    public enum Arrangement { Horizontal, Vertical, }

    public class ETScrollRectLayOut
    {
        //水平滑动还是纵向滑动

        public Arrangement _movement = Arrangement.Horizontal;
        //单行或单列的Item数量
        [Range(1, 20)]
        public int maxPerLine = 2;
        //Item之间的距离
        [Range(0, 50)]
        public int cellPadiding = 5;
        //Item的宽高
        public int cellWidth = 100;
        public int cellHeight = 100;


        public Vector3 scale = Vector3.one;
        //默认加载的行数（缓存行数），一般比可显示行数大2~3行，防止滑动的时候显示空白(如界面上显示5行，该值可设置9（留上下各缓存两行）)
        [Range(0, 20)]
        public int viewCount = 3;
        public RectTransform _content;


        //记录当前滑动行列序号
        private int _index = -1;
        private List<ItemCell> _itemList;
        private int _dataCount;

        // 刷新某个item回调<刷新的物体，索引>
        public Action<ItemCell, int> OnItemUpdateEvent;
        //创建Item回调<索引，回调>
        public Action<int, Action<ETListBaseItem>> OnCreateitemobjectEvent;

        //删除某个item回调
        public Action<ItemCell> OnDeleteitemobjectEvent;

        public Action OnAfterIniteEvent;


        //道具是否是同一类型
        public delegate bool ISsynchysis(int oldIndex, int nowIndex);
        public ISsynchysis issynchysis;
        public GameObject gameObject;
        //缓存池
        public List<ItemCell> PoolItems = new List<ItemCell>();

        public ETScrollRectLayOut()
        {
            
        }
        public RectTransform content
    {
            set{
                this._content = value;
                this._content.anchorMin = new Vector2(0, 1);
                this._content.anchorMax = new Vector2(0, 1);
                this._content.pivot = new Vector2(0, 1);
            }
    }

        public Transform transform
        {
            get
            {
                return this._content.gameObject.transform;
            }
        }

    //顶置
    public void ResetPos()
    {
        this._content.localPosition = new Vector3(this._content.localPosition.x, 0);
    }

    public int DataCount
    {
        get { return _dataCount; }
        set
        {
            _dataCount = value;
            UpdateTotalWidth();
        }
    }
    void SetItemIndex(ItemCell itemcell, int value)
    {
        itemcell._index = value;
        itemcell.item.transform.localPosition = GetPosition(value);
    }
    public int GetItemIndex(ItemCell itemcell)
    {
        return itemcell._index;
    }

    public void Init(int DataCount)
    {
        DestroyAllItems();
        _index = -1;
        _itemList = new List<ItemCell>();
        this.DataCount = DataCount;


        OnValueChange(Vector2.zero);
        if (OnAfterIniteEvent != null)
            OnAfterIniteEvent();
    }


    List<ItemCell> hideitems = new List<ItemCell>();
    public void OnValueChange(Vector2 pos)
    {
        if (_itemList == null)
            return;


        int index = GetPosIndex();

        if (_index != index && index > -1)
        {
            _index = index;

            //计算不显示对象
            for (int i = 0; i < _itemList.Count; i++)
            {
                ItemCell item = _itemList[i];
                int itemIndex = GetItemIndex(item);
                if (itemIndex < index * maxPerLine || (itemIndex >= (index + viewCount) * maxPerLine))
                    hideitems.Add(item);
            }
            foreach (ItemCell o in hideitems)
                _itemList.Remove(o);


            //Debug.Log("index:" + index + ",hideitems:" + hideitems.Count);


            //计算显示对象
            for (int i = _index * maxPerLine; i < (_index + viewCount) * maxPerLine; i++)
            {
                if (i < 0)
                    continue;
                if (i > _dataCount - 1)
                    continue;
                

                if (GetItemByIndex(i) == null)
                {
                    if (hideitems.Count > 0)
                    {
                        bool issys = false;
                        if (issynchysis != null)
                        {
                            issys = issynchysis(hideitems[0]._index, i);
                        }
                        if (!issys)
                        {
                            ItemCell o = hideitems[0];
                            hideitems.Remove(o);
                            _itemList.Add(o);
                            SetItemIndex(o, i);
                            OnItemUpdateEvent(o, i);
                        }
                        else
                        {
                            GameObject.Destroy(hideitems[0].item.gameObject);
                            hideitems.RemoveAt(0);
                            CreateItem(i);
                        }
                    }
                    else if (PoolItems.Count > 0)
                    {
                        ItemCell o = PoolItems[0];
                        o.item.gameObject.SetActive(true);
                        PoolItems.RemoveAt(0);
                        _itemList.Add(o);
                        SetItemIndex(o, i);
                        if (OnItemUpdateEvent != null)
                        {
                            OnItemUpdateEvent(o, i);
                        }
                    }
                    else
                    {
                        CreateItem(i);
                    }
                }
            }
        }
        else
        {
            //_content.position = Vector3.zero;
        }
        foreach (ItemCell item in this.PoolItems)
        {
            GameObject.Destroy(item.item.gameObject);
        }
        this.PoolItems.Clear();
    }


    /// <summary>
    /// 提供给外部的方法，添加指定位置的Item
    /// </summary>
    public void AddItem(int index)
    {
        if (index > _dataCount || index < 0)
        {
            Debug.LogError("添加错误:" + index);
            return;
        }

        AddItemIntoPanel(index);
        DataCount += 1;
    }
    /// <summary>
    /// 添加一个物体到末尾
    /// </summary>
    public void AddItemEnd()
    {
        AddItemIntoPanel(DataCount);
        DataCount += 1;
    }


    /// <summary>
    /// 提供给外部的方法，删除指定位置的Item
    /// </summary>
    public void DelItem(int index)
    {
        if (index < 0 || index > _dataCount - 1)
        {
            Debug.LogError("删除错误:" + index);
            return;
        }
        DataCount -= 1;
        DelItemFromPanel(index);
    }


    private void AddItemIntoPanel(int index)
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            ItemCell item = _itemList[i];
            int itemIndex = GetItemIndex(item);

            if (itemIndex >= index)
                this.SetItemIndex(item, itemIndex + 1);
        }
        CreateItem(index);
    }


    private void DelItemFromPanel(int index)
    {
        int maxIndex = -1;
        int minIndex = int.MaxValue;
        List<ItemCell> delitems = new List<ItemCell>();
        for (int i = 0; i < _itemList.Count; i++)
        {
            ItemCell item = _itemList[i];
            int itemIndex = GetItemIndex(item);


            if (itemIndex == index)
            {
                delitems.Add(item);
            }
            if (itemIndex > maxIndex)
            {
                maxIndex = itemIndex;
            }
            if (itemIndex < minIndex)
            {
                minIndex = itemIndex;
            }
            if (itemIndex > index)
            {
                SetItemIndex(item, itemIndex - 1);
            }
        }


        foreach (ItemCell item in delitems)
        {
            _itemList.Remove(item);
            GameObject.Destroy(item.item.gameObject);//GameObject.Destroy(item.gameObject);
        }
        if (maxIndex < DataCount - 1)
        {
            CreateItem(maxIndex);
        }
    }


    private void CreateItem(int index)
    {
        OnCreateitemobjectEvent(index, (obj) =>
        {
            if (obj != null)//实例化之后调用这里
            {
                obj.transform.SetParent(this._content.transform);
                obj.GetRectTransform().pivot = new Vector2(0f, 1f);
                obj.GetRectTransform().sizeDelta = new Vector2(this.cellWidth, this.cellHeight);
                obj.transform.localScale = this.scale;
                obj.gameObject.SetActive(true);
                ItemCell itemcell = new ItemCell();
                itemcell._index = index;
                itemcell.item = obj;
                SetItemIndex(itemcell, index);
                if (OnItemUpdateEvent != null)
                {
                    OnItemUpdateEvent(itemcell, index);
                }
                _itemList.Add(itemcell);
            }
        });
    }

    public void UpdateItemCells()
    {
        if (_itemList != null)
        {
            for (int i = 0; i < _itemList.Count; i++)
            {
                ItemCell item = _itemList[i];
                OnItemUpdateEvent(item, item._index);
            }
        }
    }
    private int GetPosIndex()
    {
        switch (_movement)
        {
            case Arrangement.Horizontal:
                if (_content.anchoredPosition.x > 0)
                    return 0;
                return Mathf.FloorToInt(_content.anchoredPosition.x / -(cellWidth * scale.x + cellPadiding));
            case Arrangement.Vertical:
                if (_content.anchoredPosition.y < 0)
                    return 0;
                return Mathf.FloorToInt(_content.anchoredPosition.y / (cellHeight * scale.y + cellPadiding));
        }
        return 0;
    }


    public Vector3 GetPosition(int i)
    {
        switch (_movement)
        {
            case Arrangement.Horizontal:
                return new Vector3(cellWidth * scale.x * (i / maxPerLine) + cellPadiding, -(cellHeight * scale.y + cellPadiding) * (i % maxPerLine), 0f);
            case Arrangement.Vertical:
                //增加列表居中对齐计算
                float leftrightspace = 0;
                leftrightspace = (_content.sizeDelta.x - cellWidth * scale.x * maxPerLine - cellPadiding * maxPerLine * 2) / 2;
                if (leftrightspace < 0)
                    leftrightspace = 0;
                else if (leftrightspace > cellPadiding)
                    leftrightspace = leftrightspace - cellPadiding;
                else
                    leftrightspace = 0;


                return new Vector3(leftrightspace + cellWidth * scale.x * (i % maxPerLine) + (i % maxPerLine) * cellPadiding + cellPadiding, -(cellHeight * scale.y + cellPadiding) * (i / maxPerLine) - cellPadiding, 0f);
        }
        return Vector3.zero;
    }




    private void UpdateTotalWidth()
    {
        int lineCount = Mathf.CeilToInt((float)_dataCount / maxPerLine);
        switch (_movement)
        {
            case Arrangement.Horizontal:
                _content.sizeDelta = new Vector2(cellWidth * scale.y * lineCount + cellPadiding * (lineCount - 1), _content.sizeDelta.y);
                break;
            case Arrangement.Vertical:
                _content.sizeDelta = new Vector2(_content.sizeDelta.x, cellHeight * scale.y * lineCount + cellPadiding * (lineCount + 3));
                break;
        }
    }


    public List<ItemCell> ItemList
    {
        get
        {
            return this._itemList;
        }
    }

    public void DestroyAllItems()
    {
        if (_itemList == null)
        {
            return;
        }


        if (issynchysis == null)
        {
            foreach (ItemCell item in this._itemList)
            {
                if (item != null && item.item != null)
                {
                    PoolItems.Add(item);
                    item.item.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            foreach (ItemCell item in this._itemList)
            {
                GameObject.Destroy(item.item.gameObject);
            }
        }


        foreach (ItemCell item in this.hideitems)
        {
            if (item.item != null)
            {
                GameObject.Destroy(item.item.gameObject);
            }
        }
        this.hideitems.Clear();
        _itemList = null;
    }


    public void Destroy()
    {
        DestroyAllItems();
        foreach (ItemCell item in this.PoolItems)
        {
            GameObject.Destroy(item.item.gameObject);
        }
        this.PoolItems.Clear();
    }


    public ItemCell GetItemByIndex(int index)
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            ItemCell item = _itemList[i];
            int itemIndex = GetItemIndex(item);
            if (itemIndex == index)
                return item;


        }
        return null;
    }
    public Vector3 GetIndexPos(int index)
    {
        if (index > 0)
        {
            switch (_movement)
            {
                case Arrangement.Horizontal:
                    return new Vector3(-cellWidth * scale.x * (index / maxPerLine) + cellPadiding, -(cellHeight * scale.y + cellPadiding) * (index % maxPerLine), 0f);
                case Arrangement.Vertical:
                    //增加列表居中对齐计算
                    float leftrightspace = 0;
                    leftrightspace = (_content.sizeDelta.x - cellWidth * scale.x * maxPerLine - cellPadiding * maxPerLine * 2) / 2;
                    if (leftrightspace < 0)
                        leftrightspace = 0;
                    else if (leftrightspace > cellPadiding)
                        leftrightspace = leftrightspace - cellPadiding;
                    else
                        leftrightspace = 0;


                    return new Vector3(0f, (cellHeight * scale.y + cellPadiding) * (index / maxPerLine) - cellPadiding, 0f);
            }
        }
        return Vector3.zero;
    }
    public void PrintList()
    {
        foreach (ItemCell icell in _itemList)
        {
            Debug.LogError("Item Name: " + icell.item.name + ", Index: " + icell._index);
        }
    }
}
}


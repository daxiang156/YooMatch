using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ET
{
    public class UIMBEditCoponent: Entity, IAwake, IUpdate
    {
        public Transform ContentItem;
        public Button btnItem;
        public GameObject oneItem;
        public GameObject gameSelf;
        public GameObject selectItem;
        public Transform fruitParent;
        public Transform ContentLevel;
        public Button btnSave;
        public Button btnDel;
        public Button btnPlay;
        public Button btnOffset;
        public InputField input;
        public InputField excelInput;

        /// <summary>
        /// 大于10000表示y
        /// </summary>
        public int offset = 0;
        public MBGridItem selectDeleteItem;
        /// <summary>
        /// 所有水果Ojb，层级-Obj列表
        /// </summary>
        public Dictionary<int, List<GameObject>> levelFruits = new Dictionary<int, List<GameObject>>();
        /// <summary>
        /// 层级，一行Excel
        /// </summary>
        public Dictionary<int, List<string>> excelData = new Dictionary<int, List<string>>();
        public int currentLevel = 1;
    }
}
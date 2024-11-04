using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UITabsBtn
    {
        private List<string> names;
        private GameObject goTabs;
        private GameObject btnItem;
        private List<TabButton> btnList;
        private int curIndex = -1;
        /// <summary>
        /// 点击分页回调
        /// </summary>
        public Action<int> CallBack;
        public UITabsBtn(GameObject goTabs,GameObject btnItem,List<string> names,Action<int> callBack)
        {
            this.goTabs = goTabs;
            this.names = names;
            this.btnItem = btnItem;
            this.CallBack = callBack;
            Init();
        }

        public void SetBtnShow(int index,bool isShow)
        {
            for (int i = 0; i < this.btnList.Count; i++)
            {
                if (this.btnList[i].index == index)
                {
                    this.btnList[i].SetShow(isShow);
                    break;
                }
            }
        }

        public void SetShow(bool isShow)
        {
            this.goTabs.SetActive(isShow);
        }

        public void BtnClickCall(int index)
        {
            if (index == curIndex)
                return;
            for (int i = 0; i < this.btnList.Count; i++)
            {
                if (index == this.btnList[i].index)
                {
                    this.btnList[i].SetSelct(true);
                    this.CallBack.Invoke(index);
                }
                else
                {
                    this.btnList[i].SetSelct(false);
                }
            }
        }
        
        private void Init()
        {
            btnList = new List<TabButton>();
            for (int i = 0; i < this.names.Count; i++)
            {
                GameObject go = GameObject.Instantiate(this.btnItem);
                go.SetActive(true);
                TabButton btn = new TabButton(go,i,this.names[i],BtnClickCall);
                go.transform.SetParent(this.goTabs.transform);
                go.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                this.btnList.Add(btn);
            }
        }
    }

    public class TabButton
    {
        private GameObject go_btn;
        public int index;
        private Action<int> callBack;

        private GameObject go_nomal;
        private GameObject go_select;
        private Text txt_word;
        private Button btn_word;
        public TabButton(GameObject go_btn,int index,string name,Action<int> callBack)
        {
            this.go_btn = go_btn;
            this.index = index;
            this.callBack = callBack;
            this.go_nomal = GameObjectMgr.Refer(go_btn, "img_nomal");
            this.go_select = GameObjectMgr.Refer(go_btn, "img_select");
            this.txt_word = GameObjectMgr.Refer(go_btn,"txt_word").GetComponent<Text>();
            this.btn_word = this.txt_word.gameObject.GetComponent<Button>();
            this.txt_word.text = name;
            this.btn_word.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            this.callBack.Invoke(this.index);
        }

        public void SetSelct(bool select)
        {
            this.go_select.gameObject.SetActive(select);
            this.go_nomal.gameObject.SetActive(!select);
        }

        public void SetShow(bool isShow)
        {
            this.go_btn.SetActive(isShow);
        }
    }
}

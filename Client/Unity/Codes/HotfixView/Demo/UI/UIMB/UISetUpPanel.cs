using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UISetUpPanel
    {
        private GameObject gameObject;
        private List<SetUpItem> itemList = new List<SetUpItem>();
        private GameObject btnSetUp;
        private bool state = false; //是否展开
        public UISetUpPanel(GameObject gameObject,GameObject btnSetUp)
        {
            this.gameObject = gameObject;
            this.btnSetUp = btnSetUp;
            InitUI();
        }

        private void InitUI()
        {
            this.btnSetUp.GetComponent<Button>().onClick.AddListener(OnSetUp);
            for (int i = 0; i < 3; i++)
            {
                GameObject go = GameObjectMgr.Refer(this.gameObject, "btnItem" + i + "_" + 0);
                go.SetActive(false);
                SetUpItem item = new SetUpItem(go,i,0,CallBack);
                itemList.Add(item);
                go = GameObjectMgr.Refer(this.gameObject, "btnItem" + i + "_" + 1);
                go.SetActive(false);
                item = new SetUpItem(go, i,1,CallBack);
                this.itemList.Add(item);
            }

            Button btnClick = GameObjectMgr.Refer(this.gameObject, "Background").GetComponent<Button>();
            btnClick.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            OnSetUp();
        }

        private void OnSetUp()
        {
            state = !this.state;
            if (this.state)
            {
                this.btnSetUp.transform.DOLocalRotate(new Vector3(0,0,180), 0.7f);
            }
            else
            {
                this.btnSetUp.transform.DOLocalRotate(new Vector3(0,0,0), 0.7f);
            }

            PlayAni(this.state);
        }

        public async void PlayAni(bool expend)
        {
            if (expend)
            {
                this.gameObject.SetActive(true);
            }
            this.PlayBgAni(expend);
            for (int i = 0; i < this.itemList.Count; i++)
            {
                this.itemList[i].PlayAni(expend).Coroutine();
                await TimerComponent.Instance.WaitAsync(100);
            }

            if (!expend)
            {
                await TimerComponent.Instance.WaitAsync(300);
                this.gameObject.SetActive(false);
            }
        }

        private async void PlayBgAni(bool expend)
        {
            Image imgBg = GameObjectMgr.Refer(this.gameObject, "Background").GetComponent<Image>();
            if (expend)
            {
                UIAniHelper.GetInstance().PlayUIDOFade(imgBg,0.6f);
            }
            else
            {
                UIAniHelper.GetInstance().PlayUIDOFade(imgBg,0,false);
            }
            await ETTask.CompletedTask;
        }

        public void CallBack(int type, int subType)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (type == this.itemList[i].type)
                {
                    this.itemList[i].RefreshState();
                }
            }
        }
    }

    public class SetUpItem
    {
        public GameObject gameObject;
        public int type;
        public int subType; //0 关闭，1 ，开启
        private Vector3 initPos;
        private Action<int, int> callBack;
        public SetUpItem(GameObject gameObject,int type,int subType,Action<int,int> callBack)
        {
            this.gameObject = gameObject;
            this.callBack = callBack;
            gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
            this.initPos = this.gameObject.transform.localPosition;
            Vector3 pos = this.initPos - new Vector3(300,0,0);
            this.gameObject.transform.localPosition = pos;
            this.type = type;
            this.subType = subType;

            RefreshState();
        }

        private void OnClick()
        {
            if (type == 0)  //背景声音
            {
                if (this.subType == 0)
                    SoundManager.Instance.SetUIVolume(1);
                else
                    SoundManager.Instance.SetUIVolume(0);
            }
            else if (type == 1) //音效
            {
                if (this.subType == 0)
                    SoundManager.Instance.SetMusicVolume(1);
                else
                    SoundManager.Instance.SetMusicVolume(0);
            }
            else if (type == 2)
            {
                if (this.subType == 0)
                    HallInfoComponent.Instance.VibrationState = 1;
                else
                    HallInfoComponent.Instance.VibrationState = 0;
            }

            this.callBack(this.type,this.subType);
        }

        public void SetOpenState(bool flag)
        {
            this.gameObject.SetActive(flag);
        }

        public async ETTask PlayAni(bool expend)
        {
            if (expend)
            {
                gameObject.transform.DOLocalMove(this.initPos, 1f).SetEase(Ease.OutBounce);
            }
            else
            {
                Vector3 pos = this.initPos - new Vector3(300,0,0);
                var tween = gameObject.transform.DOLocalMove(pos, 1f).SetEase(Ease.OutBounce);
            }


            await ETTask.CompletedTask;
        }

        public void RefreshState()
        {
            // if (type == 0)
            // {
            //     var volumeUI = SoundManager.Instance.GetUIVolume();
            //     if (volumeUI > 0)    //背景音乐开启
            //     {
            //         if (subType == 0)
            //         {
            //             this.gameObject.SetActive(false);
            //         }
            //         else
            //         {
            //             this.gameObject.SetActive(true);
            //         }
            //     }
            //     else    //关闭
            //     {
            //         if (subType == 0)
            //         {
            //             this.gameObject.SetActive(true);
            //         }
            //         else
            //         {
            //             this.gameObject.SetActive(false);
            //         }
            //     }
            // }else if (type == 1)
            // {
            //     var volumeMusic = SoundManager.Instance.GetMusicVolume();
            //     if (volumeMusic > 0)
            //     {
            //         if (subType == 0)
            //         {
            //             this.gameObject.SetActive(false);
            //         }
            //         else
            //         {
            //             this.gameObject.SetActive(true);
            //         }
            //     }
            //     else
            //     {
            //         if (subType == 0)
            //         {
            //             this.gameObject.SetActive(true);
            //         }
            //         else
            //         {
            //             this.gameObject.SetActive(false);
            //         }
            //     }
            // }
            // else if (type == 2)
            // {
            //     if (HallInfoComponent.Instance.VibrationState == 1)
            //     {
            //         if (subType == 0)
            //         {
            //             this.gameObject.SetActive(false);
            //         }
            //         else
            //         {
            //             this.gameObject.SetActive(true);
            //         }
            //     }
            //     else
            //     {
            //         if (subType == 0)
            //         {
            //             this.gameObject.SetActive(true);
            //         }
            //         else
            //         {
            //             this.gameObject.SetActive(false);
            //         }
            //     }
            // }
        }
    }
}
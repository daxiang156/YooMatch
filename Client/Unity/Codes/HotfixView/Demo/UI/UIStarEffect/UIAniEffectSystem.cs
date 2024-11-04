using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ET
{
    public class UIStarEffectSystem : AwakeSystem<UIAniEffectComponent,object>
    {
        private UIAniEffectComponent self;
        private ReferenceCollector rc;
        private UIAniEffectData effectData;
        private List<GameObject> listStar;
        private List<UIGoldEffectItem> listEffectStar;
        public override void Awake(UIAniEffectComponent self, object data)
        {
            this.self = self;
            effectData = (UIAniEffectData) data;
            this.rc = this.self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.aniParent = this.rc.Get<GameObject>("aniParent");
            this.listEffectStar = new List<UIGoldEffectItem>();
            this.PlayAni();
        }

        private void PlayAni()
        {
            string abName = "anidollar.unity3d";
            ResourcesComponent.Instance.LoadBundle(abName);
            self.bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset(abName, "AniDollar");
            PlayStarffect();
        }
        private bool IsCameraModel()
        {
            Canvas canvas = GameObjectMgr.Refer(Init.Global,"Mid").GetComponent<Canvas>();
            if (!canvas)
            {
                Debug.Log("canvas == null");
                return true;
            }
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                return false;
            else
                return true;
        }

        private Dictionary<int, List<GameObject>> goDic = new Dictionary<int, List<GameObject>>();
        private async void PlayStarffect()
        {
            goDic.Clear();
            listStar = new List<GameObject>();
            for (int i = 0; i < effectData.resoucePos.Count; i++)
            {
                if (!goDic.ContainsKey(i))
                {
                    this.goDic[i] = new List<GameObject>();
                }

                for (int n = 0; n < 4; n++)
                {
                    GameObject goldEffect;
                    goldEffect = GameObject.Instantiate(self.bundleGameObject);
                    goldEffect.SetActive(true);
                    Vector3 randomPos = new Vector3(effectData.resoucePos[i].x,this.effectData.resoucePos[i].y,0);
                    randomPos.z = 0;
                    goldEffect.transform.SetParent(this.self.aniParent.transform);
                    goldEffect.transform.localScale = new Vector3(1f, 1f, 1f);
                    goldEffect.transform.position = randomPos;
                    goldEffect.transform.localPosition = new Vector3(goldEffect.transform.localPosition.x,goldEffect.transform.localPosition.y,0);
                    goldEffect.SetActive(false);
                    this.goDic[i].Add(goldEffect);
                    // if(i < 1)
                    //     SoundComponent.Instance.PlayActionSound("Music4", "Money_Start");
                }
            }
            
            foreach (var value in this.goDic)
            {
                this.PlayAni(value.Key);
            }

            await ETTask.CompletedTask;
        }
        
        private async void PlayAni(int key)
        {
            List<GameObject> list = this.goDic[key];
            for (int i = 0; i < list.Count; i++)
            {
                if(list[i] == null)
                    continue;
                list[i].SetActive(true);
                await TimerComponent.Instance.WaitAsync(10);
                StartPlayAni(list[i], key).Coroutine();
                await TimerComponent.Instance.WaitAsync(30);
            }
            SoundComponent.Instance.PlayActionSound("Music3", "coin");
            //SoundComponent.Instance.PlayActionSound("Music4","Money2");
        }
        
        public async ETTask StartPlayAni(GameObject gameObject,int index)
        {
            UIGoldEffectItem uiGoldEffectItem = new UIGoldEffectItem(gameObject,AniFinish);
            uiGoldEffectItem.aniTime = 1.2f;
            Vector3 p0 = effectData.resoucePos[index];// + new Vector3(Random.Range(0f, 0.5f), Random.Range(-0.5f, 0.5f), 0);;
            Vector3 p1 = this.effectData.rotationPos;

           Vector3 p2 = this.effectData.targetPos;
           //Vector3 p1 = (effectData.resoucePos[index] + p2) / 2 + new Vector3(Random.Range(0f, 0.5f), Random.Range(-0.5f, 0.5f), 0);//new Vector3(this.self.upGrid.position.x + 1, this.self.upGrid.position.y + 2, 0);
            await TimerComponent.Instance.WaitAsync(10);
            uiGoldEffectItem.SetPath(p0,p1,p2,50);
            this.listEffectStar.Add(uiGoldEffectItem);
            await ETTask.CompletedTask;
        }

        private int finishCount;
        private async void AniFinish()
        {
            //SoundComponent.Instance.PlayActionSound("Music4", "Money2");
            this.finishCount++;
            //UIGoldBoomEffect.GetInstance().PlayBoom(this.self.aniParent.transform,this.effectData.targetPos);
            if (this.finishCount >= this.effectData.resoucePos.Count)
            {
                await TimerComponent.Instance.WaitAsync(300);
                effectData.callBack?.Invoke(this.effectData.resoucePos.Count);
                CloseUI();
            }
        }

        private void CloseUI()
        {
            UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIAniEffect).Coroutine();
        }
    }
}
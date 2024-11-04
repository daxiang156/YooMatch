using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

namespace ET
{
    public class UIGoldEffectSystem : AwakeSystem<UIGoldEffectComponent,object>
    {
        private UIGoldEffectComponent self;
        private ReferenceCollector rc;
        private UIGoldEffectData effectData;
        private List<UIGoldEffectItem> listEffectGold = new List<UIGoldEffectItem>();
        private int finishCount;
        public override void Awake(UIGoldEffectComponent self,object data)
        {
            this.self = self;
            effectData = (UIGoldEffectData) data;
            this.rc = this.self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.goldParent = this.rc.Get<GameObject>("goldParent");
            this.self.InitPosition = this.rc.gameObject.transform.localPosition;

            this.listEffectGold = new List<UIGoldEffectItem>();
            self.listGold = new List<GameObject>();
            this.PlayGoldEffect().Coroutine();
        }
        
        public async ETTask PlayGoldEffect()
        {
            await TimerComponent.Instance.WaitAsync(1);
            int randomValue;
            if (IsCameraModel())
            {
                randomValue = 50;
            }
            else
            {
                randomValue = 7000;
                // this.effectData.resoucePos -= tempVec;
                // this.effectData.rotationPos -= tempVec;
                // this.effectData.targetPos -= tempVec;
            }

            this.listEffectGold.Clear();
            this.self.listGold.Clear();
            finishCount = 0;
            string abName = "goldeffect.unity3d";
            ResourcesComponent.Instance.LoadBundle(abName);
            GameObject bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset(abName, "GoldEffect");
            Random random = new Random();
            GameObject lastGo = null;
            for (int i = 0; i < this.effectData.showNum; i++)
            {
                if(i != 0 )
                    await TimerComponent.Instance.WaitAsync(80);
                GameObject goldEffect;
                if (lastGo == null)     //为了每个旋转金币有层次
                {
                    goldEffect = GameObject.Instantiate(bundleGameObject);
                }
                else
                {
                    goldEffect = GameObject.Instantiate(lastGo);
                }


                goldEffect.SetActive(true);
                int posX = random.Next(-randomValue, randomValue);
                int posY = random.Next(-randomValue, randomValue);
                Vector3 randomPos = new Vector3(effectData.resoucePos.x + posX/100f,this.effectData.resoucePos.y + posY/100f,0);
                //Vector3 localPostion = this.self.goldParent.transform.InverseTransformVector(randomPos);
                randomPos.z = 0;
                goldEffect.transform.SetParent(this.self.goldParent.transform);
                goldEffect.transform.localScale = new Vector3(1, 1, 1);
                goldEffect.transform.position = randomPos;
                goldEffect.transform.localPosition = new Vector3(goldEffect.transform.localPosition.x,goldEffect.transform.localPosition.y,0);
                
                self.listGold.Add(goldEffect);
                if(i < 1)
                    SoundComponent.Instance.PlayActionSound("Music3", "coinInit");
                
            }
           
            for (int i = 0; i < this.self.listGold.Count; i++)
            {
                await TimerComponent.Instance.WaitAsync(100);
                StartPlayAni(this.self.listGold[i]).Coroutine();
            }
        }

        public async ETTask StartPlayAni(GameObject gameObject)
        {
            UIGoldEffectItem uiGoldEffectItem = new UIGoldEffectItem(gameObject,AniFinish);
            Vector3 p0 = effectData.resoucePos;
            Vector3 p1 = this.effectData.rotationPos;
            Vector3 p2 = this.effectData.targetPos;
            await TimerComponent.Instance.WaitAsync(100);
            uiGoldEffectItem.SetPath(p0,p1,p2,50);
            this.listEffectGold.Add(uiGoldEffectItem);
            await ETTask.CompletedTask;
        }

        /// <summary>
        /// 爆炸特效完成回调
        /// </summary>
        private void AniFinish()
        {
            SoundComponent.Instance.PlayActionSound("Music3", "coin");
            UIGoldBoomEffect.GetInstance().PlayBoom(this.self.goldParent.transform,this.effectData.targetPos);
            this.finishCount++;
            this.effectData.callBack?.Invoke(this.finishCount);
            VibrationHelper.GetInstance().SendVibration();
            if (this.finishCount >= this.effectData.showNum)
            {
                UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIGoldEffect).Coroutine();
            }
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
    }
}
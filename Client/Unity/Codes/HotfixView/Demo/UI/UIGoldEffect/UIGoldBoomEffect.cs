using UnityEngine;

namespace ET
{
    public class UIGoldBoomEffect
    {
        public static UIGoldBoomEffect _instance;

        public static UIGoldBoomEffect GetInstance()
        {
            if (_instance == null)
                _instance = new UIGoldBoomEffect();
            return _instance;
        }

        /// <summary>
        /// parent 特效父节点， position；本地坐标系
        /// </summary>
        public void PlayBoom(Transform parent,Vector3 position)
        {
            string abName = "gold_boom.unity3d";
            ResourcesComponent.Instance.LoadBundle(abName);
            GameObject bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset(abName, "Gold_Boom");
            GoldEffect goldEffect = new GoldEffect(GameObject.Instantiate(bundleGameObject),parent,position);
            
        }
    }

    internal class GoldEffect
    {
        private GameObject goEffect;
        public GoldEffect(GameObject goEffect,Transform parent,Vector3 position)
        {
            goEffect.SetActive(false);
            goEffect.SetActive(true);
            this.goEffect = goEffect;
            goEffect.transform.SetParent(parent);
            goEffect.transform.localScale = new Vector3(100, 100, 100);
            goEffect.transform.position = position;

            ShowEffect().Coroutine();
        }

        private async ETTask ShowEffect()
        {
            this.goEffect.SetActive(true);
            await TimerComponent.Instance.WaitAsync(2000);
            if (this.goEffect != null)
            {
                this.goEffect.SetActive(false);
                GameObject.Destroy(this.goEffect);
            }
        }
    }
}
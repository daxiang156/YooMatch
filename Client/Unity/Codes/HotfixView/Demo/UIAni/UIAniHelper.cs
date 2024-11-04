using DG.Tweening;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace ET
{
    public class UIAniHelper
    {
        private static UIAniHelper _instance;

        public static UIAniHelper GetInstance()
        {
            if (_instance == null)
            {
                _instance = new UIAniHelper();
            }
            return _instance;
        }

        public void AddListner()
        {
            
        }
        /// <summary>
        /// scale  0 - 1
        /// </summary>
        public void PlayUIAni(GameObject uiGameObject,float time,Action PlayFinish = null)
        {
            uiGameObject.transform.localScale = new Vector3(0,0,0);
            var tweenScale3 = uiGameObject.transform.DOScale(new Vector3(1.1f,1.1f,1.1f),time);
            tweenScale3.onComplete = () =>
            {
                this.PlayUIAni1(uiGameObject,PlayFinish);
            };
        }
        /// <summary>
        /// scale 1.1 - 1
        /// </summary>
        private void PlayUIAni1(GameObject uiGameObject,Action PlayFinish = null)
        {
            var tweenScale3 = uiGameObject.transform.DOScale(new Vector3(1.0f,1.0f,1.0f),0.1f);
            tweenScale3.onComplete = () =>
            {
                PlayFinish?.Invoke();
            };
        }
        
        /// <summary>
        /// scale 1 - 0
        /// </summary>
        public void PlayUIAni2(GameObject uiGameObject,Action PlayFinish = null)
        {
            var tweenScale3 = uiGameObject.transform.DOScale(new Vector3(0,0,0),0.5f);
            tweenScale3.onComplete = () =>
            {
                PlayFinish?.Invoke();
            };
        }

        public async void PlayUIDOFade(Image img,float toValue,bool isOpen = true,float time=0.5f)
        {
            if (isOpen)
            {
                img.color = new Color(0,0,0,0);  
            }
            img.DOFade(toValue,time);
            await ETTask.CompletedTask;
        }
    }
}
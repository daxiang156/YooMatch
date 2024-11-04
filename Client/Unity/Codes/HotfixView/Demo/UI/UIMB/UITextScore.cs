using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UITextScore
    {
        public Text txtScore;
        private Action callBack;
        public UITextScore(Text txtScore)
        {
            this.txtScore = txtScore;
        }

        public async void PlayAni(int startValue,int targetValue,Action callBack = null,int delayTime = 0)
        {
            if (delayTime != 0)
                await TimerComponent.Instance.WaitAsync(delayTime);
            if (targetValue < startValue)
            {
                callBack.Invoke();
            }

            this.callBack = callBack;
            var doTween = DOTween.To(delegate(float value)
            {
                txtScore.text = Math.Floor(value).ToString();
            }, startValue, targetValue, 0.7f);
            doTween.onComplete = () =>
            {
                this.callBack?.Invoke();
            };
        }
    }
}
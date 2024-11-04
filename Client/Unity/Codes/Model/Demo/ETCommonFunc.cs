using System;
using UnityEngine;
using Random = System.Random;

namespace ET
{
    public class ETCommonFunc
    {
        
        private static ETCommonFunc instance;
        public static ETCommonFunc Instance
        {
            get
            {
                if (instance == null)
                    instance = new ETCommonFunc();
                return instance;
            }
        }

        
        public async ETTask DelayAction(int waitTime, Action callback)
        {
            await TimerComponent.Instance.WaitAsync(waitTime);
            callback();
            await ETTask.CompletedTask;
        }

        public static bool IsTablet()
        {
            int width = UnityEngine.Screen.width;
            int height = UnityEngine.Screen.height;
            if (height / (float) width > 2 / 3f)
            {
                return true;
            }
            else
                return false;
        }

        public static LanguageSelect CountrySelect()
        {
            if(GameDataMgr.Instance.languageSelect == LanguageSelect.Indonesia)
                return LanguageSelect.Indonesia;
            else if(GameDataMgr.Instance.languageSelect == LanguageSelect.Ukraine)
                return LanguageSelect.Ukraine;
            else if(GameDataMgr.Instance.languageSelect == LanguageSelect.Thailand)
                return LanguageSelect.Thailand;
            else if(GameDataMgr.Instance.languageSelect == LanguageSelect.Japan)
                return LanguageSelect.Japan;
            else if (GameDataMgr.Instance.languageSelect == LanguageSelect.AutoSelectByIP)
            {
                if (GameDataMgr.Instance.country == CountryLanguage.Indonesia)
                {
                    return LanguageSelect.Indonesia;
                }
                else if (GameDataMgr.Instance.country == CountryLanguage.Ukraine)
                {
                    return LanguageSelect.Ukraine;
                }
                else
                {
                    return LanguageSelect.English;
                }
            }
            else
            {
                return LanguageSelect.English;
            }
        }

        public static bool IsAttackFruit(int itemId)
        {
            if (MBMoreDataComponent.Instance.curMBMoreMode == MBMoreMode.Normal)
                return false;
            // if (itemId == 6 || itemId == 7 || itemId == 8 || itemId == 9 || itemId == 5)
            //     return true;
            else
            {
                return false;
            }
        }
        
        public static int IsItemFruit(int itemId)
        {
            if (MBMoreDataComponent.Instance.curMBMoreMode == MBMoreMode.Normal)
                return 0;
            int rangNum = UnityEngine.Random.Range(0, 100);
            // if (rangNum <= 10)
            //     return MBItemType.re1;
            if ( rangNum <= 20)
                return MBItemType.add1;
            // if (rangNum > 20 && rangNum <= 30)
            //     return MBItemType.bomb;
            // if (rangNum > 20 && rangNum <= 30)
            //     return MBItemType.ice;
            else
            {
                return 0;
            }
        }

        public static FunyLevelEvent FunnyEvent(int id, int value)
        {
            FunyLevelEvent funyLevelEvent = new FunyLevelEvent();
            funyLevelEvent.event_id = MBSignType.FailTime;
            funyLevelEvent.event_value = value;
            return funyLevelEvent;
        }
        
        public Vector3[] BezierPath(Vector3 p0,Vector3 p1,Vector3 p2,int segmentNum )
        {
            Vector3[] path = new Vector3[segmentNum];
            for (int i = 1; i <= segmentNum; i++)
            {
                float t = i / (float)segmentNum;
                Vector3 pixel = BezierUtility.CalculateBezierPoint( p0,
                    p1, p2,t);
                path[i - 1] = pixel;
            }
            return path;
        }
    }
}
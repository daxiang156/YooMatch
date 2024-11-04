using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIMBGoTips
    {
        private GameObject goTips;
        private Dictionary<int, int> bossTipsDic = new Dictionary<int, int>();
        public UIMBGoTips(GameObject goTips)
        {
            this.goTips = goTips;
            GameObjectMgr.Refer(goTips, "btnTipsClick").GetComponent<Button>().onClick.AddListener(OnClickTips);
        }

        public async void SetCurLv(int lv)
        {
            bool isBossLv = false;
            InitConfig cfg = InitConfigCategory.Instance.Get(1);
            string[] bossLvs = cfg.boosLevel.Split(',');
            for (int i = 0; i < bossLvs.Length; i++)
            {
                if (lv == int.Parse(bossLvs[i]))
                {
                    isBossLv = true;
                    if (bossTipsDic.ContainsKey(lv))
                    {
                        this.bossTipsDic[lv]++;
                    }
                    else
                    {
                        this.bossTipsDic[lv] = 1;
                    }

                    break;
                }
            }

            if (isBossLv)
            {
                if (bossTipsDic[lv] < 2)
                {
                    GameObjectMgr.Refer(this.goTips, "txtTips").GetComponent<Text>().text = "Hard Level";
                    GameObjectMgr.Refer(this.goTips, "txtTitle").GetComponent<Text>().text = "Attention";
                    SetShow(false,true);
                    await TimerComponent.Instance.WaitAsync(1500);
                    SetShow(false,false);
                }
            }
        }

        private bool IsGuid;
        public void SetShow(bool isGuid,bool isShow)
        {
            this.IsGuid = isGuid;
            this.goTips.SetActive(isShow);
        }

        private void OnClickTips()
        {
            goTips.SetActive(false);
            if (this.IsGuid)
            {
                UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIGuide, UILayer.Mid).Coroutine();
                this.IsGuid = false;
            }
        }
    }
}
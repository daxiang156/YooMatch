using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{

     [ObjectSystem]
     public class UIMBItemAwakeSystem : AwakeSystem<UIMBItemComponent>
     {
         private UIMBItemComponent self;

         public override void Awake(UIMBItemComponent self)
         {
             this.self = self;
             ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
             self.grey = rc.Get<GameObject>("grey");
             self.btnItem = rc.Get<GameObject>("btnItem").GetComponent<Image>();
             self.bgImg = rc.Get<GameObject>("bgImg").GetComponent<Image>();
             self.grey.SetActive(false);
             self.isCanRotate = true;
         }

         public void SetHeroRoadItemInfo()
         {
             
         }
     }
     
     public static class UIMBItemSystem
     {
         public static void SetMBItemInfo(this UIMBItemComponent self,  int row, int num, int configId, int level, int itemId, Transform obj, int dictId = 0)
         {
             self.itemId = itemId;
             self.level = level;
             self.row = row;
             self.num = num;
//             self.configId = configId;
             self.obj = obj;
             if(dictId != 0)
                 self.itemUnitId = dictId;
             //await ETTask.CompletedTask;
         }

         public static void SetGrey(this UIMBItemComponent self, bool isGrey)
         {
             if (self != null && self.grey != null)
             {
                 self.grey.SetActive(isGrey);
                 // if (isGrey)
                 // {
                 //     self.btnItem.material = AppInfoComponent.Instance.grey;
                 //     self.bgImg.material = AppInfoComponent.Instance.grey;
                 // }
                 // else
                 // {
                 //     self.btnItem.material = AppInfoComponent.Instance.grey;
                 //     self.bgImg.material = AppInfoComponent.Instance.grey;
                 // }
             }
             else
             {
                 // self.btnItem.material = null;
                 // self.bgImg.material = null;
                 Log.Error(isGrey + " ：置灰失败，已经被删除，id=" + self.itemUnitId);
             }
         }

         public static void AddOverGrid(this UIMBItemComponent self, int id, UIMBItemComponent other)
         {
             if (self.overGrid.ContainsKey(id))
             {
                 Log.Error("已经添加上层格子");
             }
             else
             {
                 self.overGrid.Add(id, other);
             }

             self.SetGrey(true);
         }
         
         public static bool RemoveGrid(this UIMBItemComponent self, int id)
         {
             if(self.overGrid.ContainsKey(id))
                 self.overGrid.Remove(id);
             else
             {
                 return false;
                 //Log.Error("没有格子可删除，id=" + id);
             }

             if (self.overGrid.Count <= 0)
             {
                 self.SetGrey(false);
                 return true;
             }

             return false;
         }
         
         public static int GetDictId(this UIMBItemComponent self)
         {
             int dicId = CommonFuc.SetItemId(self.row, self.num, self.level);
             return dicId;
         }
         
         public static async void GoldRotate(this UIMBItemComponent self, bool isRotate)
         {
             // if (self.IsGold() && isRotate)
             // {
             //     await TimerComponent.Instance.WaitAsync(UnityEngine.Random.Range(1000, 10000));
             //     self.isCanRotate = true;
             //     while (self != null && self.obj != null && self.isCanRotate)
             //     {
             //         self.btnItem.transform.DOScaleX(0, 1).OnComplete(() =>
             //         {
             //             self.btnItem.transform.DOScaleX(1, 1);
             //         });
             //         await TimerComponent.Instance.WaitAsync(7000);
             //     }
             // }
             // else
             // {
             //     self.isCanRotate = false;
             //     self.btnItem.DOKill();
             //     self.btnItem.transform.localScale = Vector3.one;
             // }
             await ETTask.CompletedTask;
         }
         
         public static bool IsGold(this UIMBItemComponent self)
         {
             return false;//self.btnItem.sprite.name == "item17";
         }

         public static void SetSpecialItemNum(this UIMBItemComponent self, int mBItemType, GameObject over, int iceTime, Action callback = null)
         {
             self.itemEffectNum = mBItemType;
             if (self.itemEffectNum == MBItemType.beIce)
             {
                 if (callback != null)
                     iceTime += 50;
                 ETCommonFunc.Instance.DelayAction(iceTime, () =>
                 {
                     Log.Console("时间到：" + self.itemId);
                     self.itemEffectNum = 0;
                     if (over != null)
                     {
                         GameObject.Destroy(over);
                         over = null;
                         if (callback != null)
                             callback();
                     }
                 }).Coroutine();
             }
         }

         public static bool CanMatch(this UIMBItemComponent self)
         {
             if (self.itemEffectNum == MBItemType.beIce)
             {
                 return false;
             }

             return true;
         }
     }
}
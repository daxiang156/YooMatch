using System;
using System.Collections.Generic;
using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace ET
{
    public class ModelInfor
    {
        public GameObject gameObject;
        public int angryNum = 0;
        public SkeletonGraphic skeletonGraphic;
        public string name;
        public long orderStartTime;
        public ModelInfor(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public void Destory()
        {
            GameObject.Destroy(this.gameObject);
            this.gameObject = null;
        }
    }

    public class ModelMgr
    {
        private Transform parent;
        private int dogNum = 0;
        private int lastDogNum = 0;
        private List<Animator> animalList = new List<Animator>();
        private List<ModelInfor> spList = new List<ModelInfor>();
        private Dictionary<string, GameObject> modelList = new Dictionary<string, GameObject>();
        private GameObject MenuParent;
        private SkeletonGraphic skeleton;
        private Transform spParent;
        private float curTime;
        public ModelMgr(GameObject gameObj,GameObject MenuParent)
        {
            curTime = Time.time;
            parent = gameObj.transform;
            this.MenuParent = MenuParent;
            this.skeleton = GameObjectMgr.Refer(MenuParent, "goMenuStar").GetComponent<SkeletonGraphic>();
        }

        // private Vector3[] posList = {
        //     new Vector3(1.64f, 0, 0.73f), 
        //     new Vector3(1.12f, 0, -2.5f),
        //     new Vector3(1.89f, 0, -4.92f),
        //     new Vector3(3.23f, 0, -6.77f), 
        // };
        // private Vector3[] rotationList = {
        //     new Vector3(0f, -96, 0), 
        //     new Vector3(0, 0, 0),
        //     new Vector3(0, 0, 0),
        //     new Vector3(0, 0,0), 
        // };
        //private Vector3 originPos = new Vector3(11, 0, -7.3f);
        //private Vector3 outPos = new Vector3(-4.3f, 0, 7.3f);
        private Vector3[] posList = {
            new Vector3(-170, 0, 0),
            new Vector3(-700, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
            new Vector3(-800, 0, 0),
        };
        private Vector3 originPos = new Vector3(-800, 0, 0);
        private Vector3 outPos = new Vector3(800, 0, 0);
        
        public void InitModel(int dogNum, Transform parent)
        {
            Log.Info("start initmodel");
            Dispose();
            this.dogNum = dogNum;
            // if (this.dogNum > 4)
            //     this.dogNum = 4;
            this.lastDogNum =dogNum- this.dogNum;
            this.spParent = parent;
            this.LoadModelList(this.dogNum);
        }

        private void LoadModelList(int dogNum)
        {
            curModelIndex = 0;
            Log.Info("dogNum::" + dogNum );
//            Dictionary<string,List<string>> modelDic = ModelHelper.Instance.RandomModelList(dogNum);
            int realIndex = 0;
            List<int> peoList = new List<int>();
            for (int i = 1; i <= 6; i++)
            {
                peoList.Add(i);
            }

            Outoforder(peoList);
            //foreach (var value in modelDic)
            {
                //for (int i = 0; i < value.Value.Count; i++)
                for (int i = 1; i <= dogNum; i++)
                {
                    // string abName = value.Key;
                    // string modelName = value.Value[i];
                    // Log.Info("abName::" + abName);
                    // Log.Info("modelName::" + modelName);
                    int k = i;
                    if (i >= peoList.Count)
                        k = i - peoList.Count;
                    if (k >= peoList.Count)
                        k = k - peoList.Count;
                    if (k >= peoList.Count)
                        k = k - peoList.Count;
                    if (k >= peoList.Count)
                        k = k - peoList.Count;
//                    Log.Console("K:" + k);
                    string str = "Guke/people" + peoList[k];
                    GameObject obj0 = Resources.Load(str, typeof(GameObject)) as GameObject;
                    //DynamicDownLoadMgr.GetInstance().LoadModel<GameObject>(abName,modelName,(GameObject obj) => {
                    GameObject obj = GameObject.Instantiate(obj0, this.spParent);
                    //obj.transform.parent = this.spParent;
                        obj.gameObject.SetActive(true);
                        obj.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                        if (posList.Length <= realIndex)
                            obj.transform.localPosition = this.originPos;
                        else
                            obj.transform.localPosition = posList[realIndex];
                        // if(realIndex == 0)
                        //     obj.transform.localRotation = Quaternion.Euler(rotationList[0]);
                        // else
                        //     obj.transform.localRotation = Quaternion.Euler(rotationList[1]);
                        //animalList.Add(obj.GetComponent<Animator>());

                        ModelInfor modelInfor = new ModelInfor(obj);
                        modelInfor.gameObject = obj;
                        modelInfor.skeletonGraphic = obj.GetComponent<SkeletonGraphic>();
                        modelInfor.name = obj.name;
                        spList.Add(modelInfor);
                        // string key = modelName + IdGenerater.Instance.GenerateId();
                        // obj.name = key;
                        //AddMenu(animalList.Count - 1,obj);
                        AddMenu(spList.Count - 1,obj.gameObject);
                        realIndex++;
                    //});
                }

                this.spList[0].orderStartTime = TimeHelper.ClientNowSeconds();
            }
            this.RefreshModelAni();
        }

        public List<T> Outoforder<T>(List<T> bag)
        {
            Random randomNum = new Random();
            int index = 0;
            T temp;
            for (int i = 0; i < bag.Count; i++)
            {
                index = randomNum.Next(0, bag.Count - 1);
                if (index != i)
                {
                    temp = bag[i];
                    bag[i] = bag[index];
                    bag[index] = temp;
                }
            }
            return bag;
        }

        void Dispose()
        {
            Log.Console("清理所有模型");
            // for (int i = this.animalList.Count - 1; i >= 0; i--)
            // {
            //     GameObject.Destroy(animalList[i].gameObject);
            // }
            // animalList.Clear();
            for (int i = this.spList.Count - 1; i >= 0; i--)
            {
                this.spList[i].Destory();
            }
            spList.Clear();
            DestoryCurMenu();
            // if(this.curMenu != null)
            //     GameObject.Destroy(this.curMenu);
            //this.ClearModel();
        }
        
        public void DestoryCurMenu()
        {
            foreach (var value in this.menuDic)
            {
                GameObject.Destroy(value.Value);
            }
            this.menuDic.Clear();
        }

        public void BuyFinished(bool isNoMode)
        {
            //spList[0].skeletonGraphic.AnimationState.SetAnimation(0, "Veryhappy", true);
            var go = spList[0].gameObject;
            spList.RemoveAt(0);
            //go.transform.localRotation = Quaternion.Euler(0, -45, 0);
            Log.Console("5555555555");
            go.transform.DOLocalMove(outPos, 1.6f).onComplete = () =>
            {
                Log.Console("4444444444");
                GameObject.Destroy(go);
            };
            if (!isNoMode)
            {
                string abName = "";
                string modelName = "";
                ModelHelper.Instance.GetModelName(out abName,out modelName);
                while (IsSamModel(modelName))
                {
                    ModelHelper.Instance.GetModelName(out abName,out modelName);
                }

                if (this.lastDogNum > 0)
                {
                    this.lastDogNum--;
                    DynamicDownLoadMgr.GetInstance().LoadModel<GameObject>(abName,modelName,(GameObject obj) => {
                        //obj.transform.parent = parent;
                        obj.SetActive(true);
//                        obj.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                        obj.transform.localPosition = this.originPos;
//                        obj.transform.localRotation = Quaternion.Euler(rotationList[1]);
                        //obj.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("UI");
                        string key = modelName + IdGenerater.Instance.GenerateId();
                        obj.name = key;
                        // animalList.Add(obj.GetComponent<Animator>());
                        // AddMenu(animalList.Count - 1,obj);
                        ModelInfor modelInfor = new ModelInfor(obj);
                        modelInfor.angryNum = 0;
                        modelInfor.skeletonGraphic = obj.GetComponent<SkeletonGraphic>();
                        modelInfor.name = obj.name;
                        modelInfor.orderStartTime = TimeHelper.ClientNowSeconds();
                        this.spList.Add(modelInfor);
                        AddMenu(spList.Count - 1,obj);
                        this.SortBuyList(true);
                    }).Coroutine();
                }
                else
                {
                    this.SortBuyList(true);
                }
            }
            else
            {
                //if(go.name == )
                //Log.Console("11111111111111  279" + go.name);
                this.SortBuyList(false);
            }
            Log.Console("name:" + go.name);
            if(go.name == "people2(Clone)" || go.name == "people7(Clone)" || go.name == "people9(Clone)")
                SoundComponent.Instance.PlayActionSound("Music2","yeahFemale" + UnityEngine.Random.Range(1, 5));
            else
                SoundComponent.Instance.PlayActionSound("Music2","yeahmale" + UnityEngine.Random.Range(1, 5));
            // if (this.animalList.Count > 0)
            // {
            //     if (this.menuDic.ContainsKey(this.animalList[0].name))
            //     {
            //         this.curMenu = this.menuDic[this.animalList[0].name];
            //         PlayMenuAniIn(this.menuDic[this.animalList[0].name]);
            //         this.menuDic.Remove(this.animalList[0].name);
            //     }
            // }
            if (this.spList.Count > 0)
            {
                this.spList[0].orderStartTime = TimeHelper.ClientNowSeconds();
                // if (this.menuDic.ContainsKey(this.spList[0].name))
                // {
                //     this.curMenu = this.menuDic[this.spList[0].name];
                //     PlayMenuAniIn(this.menuDic[this.spList[0].name]);
                //     this.menuDic.Remove(this.spList[0].name);
                // }
            }//else
        }

        private void SortBuyList(bool isHaveNew)
        {
            // for (int k = 0; k < animalList.Count; k++)
            // {
            //     if(animalList[k] == null) continue;
            //     int index = k;
            //     this.animalList[k].Play("Run");
            //     float moveTime = 0.5f;
            //     if (isHaveNew && k == animalList.Count - 1)
            //         moveTime = 1.5f;
            //     this.animalList[k].transform.DOLocalMove(posList[k], moveTime).SetEase(Ease.Linear).onComplete = () =>
            //     {
            //         if (index < animalList.Count)
            //         {
            //             this.animalList[index].Play("Idle");
            //             if(index == 0)
            //                 this.animalList[index].transform.localRotation = Quaternion.Euler(rotationList[0]);
            //         }
            //     };
            // }
            for (int k = 0; k < this.spList.Count; k++)
            {
                if(spList[k] == null) continue;
                int index = k;
                this.spList[k].skeletonGraphic.AnimationState.SetAnimation(0, "idle", true);
                float moveTime = 0.2f;
                // if (isHaveNew && k == spList.Count - 1)
                //     moveTime = 1.5f;
                this.spList[k].gameObject.transform.DOLocalMove(posList[k], moveTime).SetEase(Ease.Linear).onComplete = () =>
                {
                    if (spList.Count > 0)
                    {
                        //this.spList[index].Play("Idle");
                        this.spList[0].skeletonGraphic.AnimationState.SetAnimation(0, "move", false).Complete += (TrackEntry trackEntry) =>
                        {
                            this.spList[0].skeletonGraphic.AnimationState.SetAnimation(0, "idle", true);
                        };
                        ETCommonFunc.Instance.DelayAction(600, () =>
                        {
                            this.spList[0].skeletonGraphic.AnimationState.SetAnimation(0, "idle", true);
                        }).Coroutine();
                        // if(index == 0)
                        //     this.spList[index].transform.localRotation = Quaternion.Euler(rotationList[0]);
                    }
                };
            }
        }
        
        public void UpDate()
        {
            //生气不用
            // if (this.spList.Count != 0)
            // {
            //     long disTime = TimeHelper.ClientNowSeconds() - this.spList[0].orderStartTime;
            //     if (this.spList[0].angryNum == 0 && disTime >= 30)
            //     {
            //         this.spList[0].skeletonGraphic.AnimationState.SetAnimation(0, "Angry", true).Complete+= (TrackEntry state) =>
            //         {
            //             this.spList[0].skeletonGraphic.AnimationState.SetAnimation(0, "idle", true);
            //         };
            //         ETCommonFunc.Instance.DelayAction(5000, () =>
            //         {
            //             this.spList[0].skeletonGraphic.AnimationState.SetAnimation(0, "idle", true);
            //         }).Coroutine();
            //         this.spList[0].angryNum++;
            //     }
            // }

            if (this.menuDic == null || this.menuDic.Count == 0)
                return;
            for (int i = 0; i < this.spList.Count; i++)
            {
                if (this.spList[i] == null)
                {
                    //Log.Error("this.spList[i] == null");
                    continue;
                }
                string key = this.spList[i].name;
                if (this.menuDic.ContainsKey(this.spList[i].name))
                {
                    Camera camera = GlobalComponent.Instance.mainCamera.GetComponent<Camera>();
                    Vector2 screenPos = camera.WorldToScreenPoint(spList[i].gameObject.transform.position);
                    Vector3 pos;
                    if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.menuDic[key].GetComponent<RectTransform>(), screenPos, GlobalComponent.Instance.UICamera.GetComponent<Camera>(), out pos))
                    {
                        this.menuDic[key].transform.position = pos + new Vector3(0,1.2f,0);
                    }
                }
            }
        }

        //private GameObject curMenu;
        private Dictionary<string,GameObject> menuDic = new Dictionary<string, GameObject>();
        private void AddMenu(int index,GameObject goTarget)
        {
            // if (spList[index] == null)
            // {
            //     //Log.Error("spList[index] == null:" + index);
            //     return;
            // }
            // if (this.menuDic.ContainsKey(this.spList[index].name))
            //     return;
            // GameObject goMenu = GameObjectMgr.Refer(this.MenuParent, "goMenu");
            // GameObject tempGoMenu = GameObject.Instantiate(goMenu);
            // tempGoMenu.SetActive(true);
            // tempGoMenu.transform.SetParent(this.MenuParent.transform);
            // tempGoMenu.transform.localPosition = Vector3.zero;
            // if (index == 0)
            // {
            //     curMenu = tempGoMenu;
            //     GameObject upGrid = GameObjectMgr.Refer(this.parent.gameObject, "upGrid");
            //     GameObject targetGo = GameObjectMgr.Refer(upGrid, "1");
            //     tempGoMenu.gameObject.transform.position = targetGo.transform.position + new Vector3(-0.3f,+0.44f,0);
            //     tempGoMenu.transform.localScale = new Vector3(0.6f,0.6f,0.6f);
            //     return;
            // }
            // tempGoMenu.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
            // Camera camera = GlobalComponent.Instance.mainCamera.GetComponent<Camera>();
            // Vector2 screenPos = camera.WorldToScreenPoint(goTarget.gameObject.transform.position);
            // Vector3 pos;
            // if (RectTransformUtility.ScreenPointToWorldPointInRectangle(tempGoMenu.GetComponent<RectTransform>(), screenPos, GlobalComponent.Instance.UICamera.GetComponent<Camera>(), out pos))
            // {
            //     tempGoMenu.transform.position = pos + new Vector3(0,1.2f,0);
            // }
            // menuDic[goTarget.name] = tempGoMenu;
        }
        
        /// <summary>
        /// 订单飞进来
        /// </summary>
        /// <param name="goMenu"></param>
        private async void PlayMenuAniIn(GameObject goMenu)
        {
            GameObject upGrid = GameObjectMgr.Refer(this.parent.gameObject, "upGrid");
            GameObject targetGo = GameObjectMgr.Refer(upGrid, "1");
            Vector3 p0 = goMenu.gameObject.transform.position;
            Vector3 p1 = p0 + new Vector3(-1,-2,0);
            Vector3 p2 = targetGo.transform.position + new Vector3(-0.3f,+0.38f,0);
            UIGoldEffectItem effect = new UIGoldEffectItem(goMenu, () =>
            {
                goMenu.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                goMenu.transform.DOScale(0.6f, 0.3f).SetEase(Ease.InBounce);
            });
            effect.SetPath(p0,p1,p2,50);
            this.RefreshModelAni();
            await ETTask.CompletedTask;
        }

        public async void PlayMenuStarAni()
        {
            await TimerComponent.Instance.WaitAsync(300);
            GameObject goMenuStar = GameObjectMgr.Refer(MenuParent, "goMenuStar");
            GameObject tempMenuStar = GameObject.Instantiate(goMenuStar);
            tempMenuStar.transform.SetParent(goMenuStar.transform.parent);
            tempMenuStar.transform.localScale = new Vector3(1, 1, 1);
            tempMenuStar.transform.localPosition = goMenuStar.transform.localPosition;
            tempMenuStar.SetActive(true);
            SkeletonGraphic skeletonGraphic = tempMenuStar.GetComponent<SkeletonGraphic>();
            skeletonGraphic.AnimationState.SetAnimation(0, "star", false);
            await TimerComponent.Instance.WaitAsync(2000);
            GameObject.Destroy(tempMenuStar);
        }

        private int curModelIndex = 0;
        private void RefreshModelAni()
        {
            if (this.spList.Count < 1)
                return;
            if (curModelIndex >= this.spList.Count)
                return;
            var sp = spList[curModelIndex];
            sp.skeletonGraphic.AnimationState.SetAnimation(0, "move", false);
            Log.Console(sp.name + ":move======================");
            ETCommonFunc.Instance.DelayAction(1000, () =>
            {
                sp.skeletonGraphic.AnimationState.SetAnimation(0, "idle", true);
            }).Coroutine();
        }

        private bool IsSamModel(string name)
        {
            // for (int i = 0; i < this.animalList.Count; i++)
            // {
            //     string[] str = this.animalList[i].name.Split(':');
            //     if (str[0].Equals(name))
            //         return true;
            // } 
            for (int i = 0; i < this.spList.Count; i++)
            {
                if(spList[i] == null)
                    continue;
                string[] str = this.spList[i].name.Split(':');
                if (str[0].Equals(name))
                    return true;
            }
            return false;
        }
    }
}
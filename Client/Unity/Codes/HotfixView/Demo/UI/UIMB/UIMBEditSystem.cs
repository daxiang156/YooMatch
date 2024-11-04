
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [ObjectSystem]
    public class UIMBEditAwakeSystem : AwakeSystem<UIMBEditCoponent>
    {
        private UIMBEditCoponent self;
        public override void Awake(UIMBEditCoponent self)
        {
            this.self = self;
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            //self.btnClose = rc.Get<GameObject>("btnClose").GetComponent<Button>();
            self.ContentLevel = rc.Get<GameObject>("ContentLevel").transform;
            self.ContentItem = rc.Get<GameObject>("ContentItem").transform;
            self.oneItem = rc.Get<GameObject>("oneItem");
            self.btnSave = rc.Get<GameObject>("btnSave").GetComponent<Button>();
            self.btnDel = rc.Get<GameObject>("btnDel").GetComponent<Button>();
            self.btnPlay = rc.Get<GameObject>("btnPlay").GetComponent<Button>();
            self.btnOffset = rc.Get<GameObject>("btnOffset").GetComponent<Button>();
            self.gameSelf = self.GetParent<UI>().GameObject;
            self.fruitParent = rc.Get<GameObject>("fruitParent").transform;
            self.input = rc.Get<GameObject>("input").GetComponent<InputField>();
            self.excelInput = rc.Get<GameObject>("excelInput").GetComponent<InputField>();
            self.input.text = "1000";
            this.AddListner();

            this.InitScrollItem();
            this.InitScrollLevel();
            List<string> explain = new List<string>();
            explain.Add(null);
            explain.Add(null);
            explain.Add("ID");
            explain.Add("横");
            explain.Add("竖");
            explain.Add("横向偏移");
            explain.Add("竖向偏移");
            explain.Add("道具列表");
            self.excelData.Add(3, explain);
            
            List<string> idNames = new List<string>();
            idNames.Add(null);
            idNames.Add(null);
            idNames.Add("Id");
            idNames.Add("Row");
            idNames.Add("Num");
            idNames.Add("RowOffset");
            idNames.Add("NumOffset");
            idNames.Add("ItemList");
            self.excelData.Add(4, idNames);
            
            List<string> idTypes = new List<string>();
            idTypes.Add(null);
            idTypes.Add(null);
            idTypes.Add("int");
            idTypes.Add("int");
            idTypes.Add("int");
            idTypes.Add("int");
            idTypes.Add("int");
            idTypes.Add("string");
            self.excelData.Add(5, idTypes);
        }

        void InitScrollItem()
        {
            for (int i = 0; i < self.ContentItem.childCount; i++)
            {
                Image img = self.ContentItem.GetChild(i).Find("btnItem").GetComponent<Image>();
                var sp = Resources.Load("MBitem/item" + (i + 1), typeof(Sprite)) as Sprite;
                if (sp != null)
                {
                    img.sprite = sp;
                }

                int itemId = i + 1;
                var btn = self.ContentItem.GetChild(i).Find("btnItem").GetComponent<Button>();
                btn.onClick.AddListener(()=>
                {
                    ClickFruit(btn.transform, itemId);
                });
            }
        }

        void ClickFruit(Transform go, int itemId)
        {
            if (this.self.currentLevel == 0)
            {
                UIHelper.ShowTip(self.ZoneScene(),"请选择层级");
                return;
            }

            if (self.selectItem == null)
            {
                this.self.selectItem = GameObject.Instantiate(self.oneItem, this.self.fruitParent.Find("level" + this.self.currentLevel));
                this.self.selectItem.SetActive(true);
            }

            InstalObj(self.selectItem, itemId);
        }

        void InstalObj(GameObject obj, int itemId, int configId = 0, int level = -1)
        {
            if (level == -1)
                level = this.self.currentLevel;
            Image img = obj.transform.Find("btnItem").GetComponent<Image>();
            MBGridItem item = obj.AddComponent<MBGridItem>();
            item.level = level;
            item.itemId = itemId;
            item.configId = configId;
            
            var btn = item.transform.Find("btnItem").GetComponent<Button>();
            btn.onClick.AddListener(()=>
            {
                if(this.self.selectItem != null)
                    return;
                if (this.self.selectDeleteItem == null)
                {
                    this.self.selectDeleteItem = item;
                    //self.selectDeleteItem.transform.Find("grey").gameObject.SetActive(true);
                }
                else
                {
                    self.selectDeleteItem.transform.Find("grey").gameObject.SetActive(false);
                    this.self.selectDeleteItem = item;
                }
                
                this.self.selectItem = self.selectDeleteItem.gameObject;
            });
            var sp = Resources.Load("MBitem/item" + itemId, typeof(Sprite)) as Sprite;
            if (sp != null)
            {
                img.sprite = sp;
            }
        }

        void InitScrollLevel()
        {
            for (int i = 0; i < self.ContentLevel.childCount; i++)
            {
                Text txt = self.ContentLevel.GetChild(i).Find("levelAll/Label").GetComponent<Text>();
                if (i == 0)
                {
                    txt.text = "全层";
                }
                else
                    txt.text = "第" + i + "层";

                int level = i;
                var tgl = self.ContentLevel.GetChild(i).Find("levelAll").GetComponent<Toggle>();
                tgl.onValueChanged.AddListener((bool isOn) =>
                {
                    this.self.currentLevel = level;

                    if (level != 0)
                    {
                        foreach (var VARIABLE in this.self.levelFruits)
                        {
                            var objs = VARIABLE.Value;
                            if (objs != null)
                            {
                                if (level >= VARIABLE.Key)
                                {
                                    for (int k = 0; k < objs.Count; k++)
                                    {
                                        objs[k].SetActive(true);
                                        objs[k].transform.Find("grey").gameObject.SetActive(level != VARIABLE.Key);
                                    }
                                }
                                else if (level != VARIABLE.Key)
                                {
                                    for (int k = 0; k < objs.Count; k++)
                                    {
                                        objs[k].SetActive(false);
                                    }
                                }
                            }
                            else
                            {
                                Log.Error("objs == null");
                            }
                        }
                    }
                    else
                    {
                        foreach (var VARIABLE in this.self.levelFruits)
                        {
                            var objs = VARIABLE.Value;
                            if (objs != null)
                            {
                                for (int k = 0; k < objs.Count; k++)
                                {
                                    objs[k].SetActive(true);
                                    objs[k].transform.Find("grey").gameObject.SetActive(false);
                                }
                            }
                        }
                    }
                });
            }
        }

        public void AddListner()
        {
            //self.btnClose.onClick.AddListener(() => { OnClickClose(); });
            self.btnSave.onClick.AddListener(() =>
            {
                string excelName = "MBLv1.xlsx";
                if (this.self.excelInput.text != "")
                    excelName = self.excelInput.text + ".xlsx";
                //WriteExcelTools.WriteExcel(excelName, "MBLv1Config", this.self.excelData);
                
                Dictionary<int, MBLv1> dics = new Dictionary<int, MBLv1>();
                foreach (var VARIABLE in this.self.excelData)
                {
                    List<string> rowData = VARIABLE.Value;
                    MBLv1 mbLv1 = new MBLv1();
                    int id = 0;
                    if (int.TryParse(rowData[0], out id))
                    {
                        mbLv1.Id = int.Parse(rowData[2]);
                        mbLv1.Row = int.Parse(rowData[3]);
                        mbLv1.Num = int.Parse(rowData[4]);
                        mbLv1.RowOffset = int.Parse(rowData[5]);
                        mbLv1.NumOffset = int.Parse(rowData[6]);
                        mbLv1.ItemList = rowData[7];
                        dics.Add(mbLv1.Id, mbLv1);
                    }
                }

                GlobalComponent.Instance.mbDic = dics;
            });
            this.self.btnPlay.onClick.AddListener(() =>
            {
//                WriteExcelTools.WriteExcel("MBLv1.xlsx", "MBLv1Config", this.self.excelData);
                AppInfoComponent.Instance.percentMBEdit = int.Parse(this.self.input.text);
                if (AppInfoComponent.Instance.percentMBEdit < 0)
                    AppInfoComponent.Instance.percentMBEdit = 0;
                if (AppInfoComponent.Instance.percentMBEdit > 1000)
                    AppInfoComponent.Instance.percentMBEdit = 1000;
                Dictionary<int, MBLv1> dics = new Dictionary<int, MBLv1>();
                foreach (var VARIABLE in this.self.excelData)
                {
                    List<string> rowData = VARIABLE.Value;
                    MBLv1 mbLv1 = new MBLv1();
                    int id = 0;
                    if (int.TryParse(rowData[2], out id))
                    {
                        mbLv1.Id = int.Parse(rowData[2]);
                        Log.Console("Id:" + mbLv1.Id + "   " + rowData[3]);
                        if (mbLv1.Id != 1)
                        {
                            mbLv1.Row = int.Parse(rowData[3]);
                            mbLv1.Num = int.Parse(rowData[4]);
                            mbLv1.RowOffset = int.Parse(rowData[5]);
                            mbLv1.NumOffset = int.Parse(rowData[6]);
                        }
                        mbLv1.ItemList = rowData[7];
                        dics.Add(mbLv1.Id, mbLv1);
                    }
                }

                GlobalComponent.Instance.mbDic = dics;
                UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIMB, UILayer.Mid).Coroutine();
                UIHelper.Remove(this.self.ZoneScene(), UIType.UIMBEdit).Coroutine();
            });
            self.btnDel.onClick.AddListener(() =>
            {
                string excelName = "MBLv1.xlsx";
                if (this.self.excelInput.text != "")
                    excelName = self.excelInput.text + ".xlsx";
                //Dictionary<int, List<string>> excelRead = WriteExcelTools.ReadExcel(excelName, "MBLv1Config");
//                 this.self.excelData.Clear();
//                 foreach (var VARIABLE in excelRead)
//                 {
//                     int key = VARIABLE.Key;
//                     if (key <= 4)
//                     {
//                         self.excelData.Add(key, VARIABLE.Value);
//                         continue;
//                     }
//                 
//                     List<string> value0 = VARIABLE.Value;
// //                    Log.Console(value0[2]);
//                     if(value0[2] == "")
//                         break;
//                     int keyNew = int.Parse(value0[2]) % 1000000 + 1000000;
//                     string[] itemList = value0[7].Split(',');
//                     int row = int.Parse(value0[3]);
//                     int no = int.Parse(value0[4]);
//                     int offset = int.Parse(value0[6]);
//                     for (int k = 0; k < itemList.Length; k++)
//                     {
//                         int level = k + 1;
//                         int fruitId = int.Parse(itemList[k]);
//                         this.self.CombineItem(keyNew, level, fruitId, row, no, offset);
//                     }
//
//                 }
                //this.self.excelData = excelRead;

                foreach (var listObj in this.self.levelFruits)
                {
                    var list = listObj.Value;
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        GameObject.Destroy(list[i]);
                    }
                }
                this.self.levelFruits.Clear();

                
                foreach (var listObj in this.self.excelData)
                {
                    if (listObj.Key > 4)
                    {
                        List<string> rowData = listObj.Value;
                        string[] itemList = rowData[7].Split(',');
                        int offsetValue = 0;
                        for (int k = 0; k < itemList.Length; k++)
                        {
                            int itemId = 0;
                            if (int.TryParse(itemList[k], out itemId))
                            {
                            }
                            else
                            {
                                Log.Error(k + ":index,道具Id转int失败：" + itemList[k]);
                            }

                            if(itemId == 0)
                                continue;
                            int level = k + 1;
                            GameObject obj = GameObject.Instantiate(self.oneItem, this.self.fruitParent.Find("level" + level));
                            obj.SetActive(true);
                            int row = int.Parse(rowData[3]);
                            int no = int.Parse(rowData[4]);
                            InstalObj(obj, itemId, int.Parse(rowData[2]), level);
                        
                            Vector3 pos = new Vector3(row * 52 , no * 52);
                            int offsetX = int.Parse(rowData[5]);
                            int offsetY = int.Parse(rowData[6]);
                            if (offsetX != 0 )
                            {
                                pos.x += offsetValue * offsetX;
                            }else if (offsetY != 0)
                            {
                                pos.y += offsetValue * offsetY;
                            }
                            obj.transform.localPosition = pos;
                            offsetValue++;
                            
                            if (!self.levelFruits.ContainsKey(level))
                            {
                                self.levelFruits.Add(level, new List<GameObject>());
                            }
                            self.levelFruits[level].Add(obj);
                        }
                    }
                }

                self.ContentLevel.GetChild(0).Find("levelAll").GetComponent<Toggle>().isOn = true;



                // if (this.self.selectDeleteItem == null)
                // {
                //     UIHelper.ShowTip(self.ZoneScene(),"没有选中需要删除的水果");
                // }
                // else
                // {
                //     
                //     var item = this.self.selectDeleteItem.GetComponent<MBGridItem>();
                //
                //     var listOjb = this.self.levelFruits[item.level];
                //     int configId = item.configId;
                //     if (self.excelData.ContainsKey(configId))
                //     {
                //         var list = self.excelData[configId];
                //         string itemIds = list[7];
                //         string[] items = itemIds.Split(',');
                //         items[item.level - 1] = "0";
                //
                //         string itemId = "";
                //         int num = 0;
                //         for (int i = 1; i <= items.Length; i++)
                //         {
                //             if (items[i - 1] != "0")
                //                 num++;
                //             if (i == self.currentLevel)
                //             {
                //                 itemId += items[i - 1];
                //             }
                //             else
                //             {
                //                 itemId += items[i - 1] + ",";
                //             }
                //         }
                //
                //         list[7] = itemId;
                //         if (num == 0)
                //         {
                //             self.excelData[configId] = null;
                //             self.excelData.Remove(configId);
                //         }
                //         else
                //         self.excelData[configId] = list;
                //     }
                //     else
                //     {
                //         Log.Error("需要删除的水果找不到 configId：" + configId + " level:" + item.level);
                //     }
                //
                //     for (int i = 0; i < listOjb.Count; i++)
                //     {
                //         var itemObj = listOjb[i].GetComponent<MBGridItem>();
                //         if (itemObj.itemId == this.self.selectDeleteItem.itemId && itemObj.gameObject == this.self.selectDeleteItem.gameObject)
                //         {
                //             var obj0 = listOjb[i];
                //             listOjb.RemoveAt(i);
                //             GameObject.Destroy(obj0);
                //             break;
                //         }
                //     }
                //
                //     this.self.selectDeleteItem = null;
                //     
                //     
                // }
            });
            
            this.self.btnOffset.onClick.AddListener(() =>
            {
                Text txt = self.btnOffset.transform.Find("Text").GetComponent<Text>();
                int offset = 10;
                if (txt.text == "无偏移")
                {
                    this.self.offset = -10 * offset;
                    txt.text = "下偏移";
                    // this.self.offset = offset;
                    // txt.text = "右偏移";
                }
                // else if (txt.text == "右偏移")
                // {
                //     this.self.offset = -10 * offset;
                //     txt.text = "下偏移";
                // }
                else if (txt.text == "下偏移")
                {
                    this.self.offset = 0;
                    txt.text = "无偏移";
                    // this.self.offset = -1 * offset;
                    // txt.text = "左偏移";
                }
                // else if (txt.text == "左偏移")
                // {
                //     this.self.offset = 10 * offset;
                //     txt.text = "上偏移";
                // }
                // else// if (txt.text == "上偏移")
                // {
                //     this.self.offset = 0;
                //     txt.text = "无偏移";
                // }
                Log.Console(this.self.offset.ToString());
            });
        }

        void ExcelToObj()
        {
            
        }

        private  void  OnClickClose()
        {
            SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            UIHelper.Remove(self.ZoneScene(), UIType.UIPop).Coroutine();
        }
    }
    
   
    public class UIMBEditUpdateSystem: UpdateSystem<UIMBEditCoponent>
    {

        void DeleteItem(UIMBEditCoponent self, bool isLeftObj, int level = -1)
        {
            if (level == -1)
                level = self.currentLevel;
            if (self.selectDeleteItem == null)
            {
                UIHelper.ShowTip(self.ZoneScene(),"没有选中需要删除的水果");
            }
            else
            {
                var item = self.selectDeleteItem.GetComponent<MBGridItem>();
                var listOjb = self.levelFruits[item.level];
                int configId = item.configId;
                if (self.excelData.ContainsKey(configId))
                {
                    var list = self.excelData[configId];
                    string itemIds = list[7];
                    string[] items = itemIds.Split(',');
                    items[item.level - 1] = "0";

                    string itemId = "";
                    int num = 0;
                    for (int i = 1; i <= items.Length; i++)
                    {
                        if (items[i - 1] != "0")
                            num++;
                        if (i == level)
                        {
                            if(i == items.Length)
                                itemId += items[i - 1];
                            else
                                itemId += items[i - 1] + ",";
                        }
                        else
                        {
                            if(i == items.Length)
                                itemId += items[i - 1];
                            else
                                itemId += items[i - 1] + ",";
                        }
                    }

                    list[7] = itemId;
                    if (num == 0)
                    {
                        self.excelData[configId] = null;
                        self.excelData.Remove(configId);
                    }
                    else
                    self.excelData[configId] = list;
                }
                else
                {
                    Log.Error("需要删除的水果找不到 configId：" + configId + " level:" + item.level);
                }

                for (int i = 0; i < listOjb.Count; i++)
                {
                    var itemObj = listOjb[i].GetComponent<MBGridItem>();
                    if (itemObj.itemId == self.selectDeleteItem.itemId && itemObj.gameObject == self.selectDeleteItem.gameObject)
                    {
                        var obj0 = listOjb[i];
                        listOjb.RemoveAt(i);
                        if (!isLeftObj)
                        {
                            GameObject.Destroy(obj0);
                            self.selectItem = null;
                        }

                        break;
                    }
                }
                // if (!isLeftObj)
                // {
                //     for (int i = 0; i < listOjb.Count; i++)
                //     {
                //         var itemObj = listOjb[i].GetComponent<MBGridItem>();
                //         if (itemObj.itemId == self.selectDeleteItem.itemId && itemObj.gameObject == self.selectDeleteItem.gameObject)
                //         {
                //             var obj0 = listOjb[i];
                //             GameObject.Destroy(obj0);
                //             break;
                //         }
                //     }
                //     
                //     self.selectItem = null;
                // }
                // else
                // {
                //     self.selectDeleteItem.transform.Find("grey").gameObject.SetActive(false);
                // }

                self.selectDeleteItem = null;
            }
        }

        public override void Update(UIMBEditCoponent self)
        {
            Vector2 uisize = self.gameSelf.transform.parent.GetComponent<RectTransform>().sizeDelta;//得到画布的尺寸
            Vector2 screenpos = Input.mousePosition;
            Vector2 screenpos2;
            screenpos2.x = screenpos.x - (Screen.width / 2);//转换为以屏幕中心为原点的屏幕坐标
            screenpos2.y = screenpos.y - (Screen.height / 2);
            Vector2 uipos;//UI坐标
            uipos.x = screenpos2.x*  (uisize.x / Screen.width);//转换后的屏幕坐标*画布与屏幕宽高比
            uipos.y = screenpos2.y * ( uisize.y / Screen.height);
            if (self.selectItem != null)
            {
                //Vector2 uipos = Vector3.one;
                //RectTransformUtility.ScreenPointToLocalPointInRectangle(self.gameSelf.transform.parent as RectTransform,
                //    Input.mousePosition, Camera.main, out uipos);
                
                
                // Vector3 worldPos=Camera.main.ScreenToWorldPoint(Input.mousePosition);//屏幕坐标转换世界坐标
                // Vector2 uiPos = self.gameSelf.transform.parent.InverseTransformPoint(worldPos);//世界坐标转换位本地坐标
                self.selectItem.transform.localPosition = new Vector3(uipos.x, uipos.y, 0);
            }

            if ((Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace)) && self.selectDeleteItem != null)
            {
                //Log.Console("删除之前的，不保留Obj");
                DeleteItem(self, false, self.selectDeleteItem.level);
            }

            if (Input.GetMouseButtonUp(1) && self.selectItem != null)
            {
                MBGridItem item = self.selectItem.GetComponent<MBGridItem>();
                int level = item.level;
                if (self.selectDeleteItem != null)
                {
                    //Log.Console("删除之前的，保留Obj");
                    DeleteItem(self, true, level);;
                }
                int row = (int)Mathf.Round(uipos.x / 52.0f);
                int no = (int)Mathf.Round(uipos.y / 52.0f);
                int dic2dId = CommonFuc.SetItemId(row, no, level);
                var listRound = CommonFuc.GetRoundId(dic2dId, 1);
                var pos = new Vector3(row * 52, no * 52, 0);
                
                Log.Console("row:" + row + " no" + no);
                for (int i = 0; i < listRound.Count; i++)
                {
                    int configId0 = listRound[i];
                    if (self.excelData.ContainsKey(configId0))
                    {
                        var config = self.excelData[configId0];
                        string[] itemIds = config[7].Split(',');
                        if (itemIds.Length >= level && itemIds[level - 1] != "0")
                        {
                            Log.Error("configId0:" + configId0 + " level:" + level);
                            UIHelper.ShowTip(self.ZoneScene(), "同层不能重叠，请摆到下一层");
                            return;
                        }
                    }
                }

                

                //Log.Console("松手放置水果");
                int fruitId = item.itemId;
                
                if (!self.levelFruits.ContainsKey(level))
                {
                    self.levelFruits.Add(level, new List<GameObject>());
                }

                self.levelFruits[level].Add(self.selectItem);

                //int configId = row * 1000 + no;

                int configId = CommonFuc.SetItemId(row, no, 1);
                item.configId = configId;
                if (self.excelData.ContainsKey(configId))
                {
                    
                    var list = self.excelData[configId];
                    string itemIds = list[7];
                    string[] items = itemIds.Split(',');
                    
                    string itemId = "";
                    if(items.Length < level)
                    {
                        for (int i = 1; i <= level; i++)
                        {
                            //中间值，直接拿值
                            if(i < items.Length)
                                itemId += items[i - 1] + ",";
                            else if (i == items.Length)
                            {
                                if (i == level)
                                    itemId += fruitId;
                                else
                                {
                                    itemId += items[i - 1] + ",";
                                }
                            }
                            else if(i > items.Length && i < level)//加在后面
                            {
                                itemId += "0,";
                            }
                            else if (i == level)//最后一值没有逗号
                                itemId += fruitId;
                        }
                    }
                    else
                    {
                        items[level - 1] = fruitId.ToString();
                        //items[level] = fruitId.ToString();
                        for (int i = 0; i < items.Length; i++)
                        {
                            if(i < items.Length - 1)
                                itemId += items[i] + ",";
                            else if (i == items.Length - 1)
                                itemId += items[i];
                        }
                    }

                        
                    int offsetValue = 0;
                    for (int i = 0; i < items.Length && i < level; i++)
                    {
                        if (items[i] != "0")
                            offsetValue++;
                    }
                    if (Math.Abs(self.offset) > 0 && Math.Abs(self.offset) < 19)
                    {
                        pos.x += self.offset * (offsetValue - 1);
                    }else if (self.offset != 0)
                    {
                        pos.y += self.offset / 10 * (offsetValue - 1);
                        self.excelData[configId][6] = "-10";//self.offset.ToString();
                    }
                    self.selectItem.transform.localPosition = pos;
                    self.excelData[configId][7] = itemId;
                }
                else
                {
                    self.selectItem.transform.localPosition = pos;
                    List<string> list = new List<string>();
                    list.Add(null);
                    list.Add(null);
                    list.Add(configId.ToString());
                    list.Add(row.ToString());
                    list.Add(no.ToString());
                    if (Math.Abs(self.offset) < 19)
                    {
                        list.Add(self.offset.ToString());
                        list.Add("0");
                    }
                    else
                    {
                        list.Add("0");
                        list.Add((self.offset / 10).ToString());
                    }
                    string itemId = "";
                    for (int i = 1; i <= level; i++)
                    {
                        if (i == level)
                            itemId += fruitId;
                        else
                            itemId += "0,";
                    }
                    list.Add(itemId);
                    self.excelData.Add(configId, list);
                }
                self.selectItem = null;
                
            }
            // else if (Input.GetMouseButtonUp(0) && self.selectItem == null)
            // {
            //     int row = (int)Mathf.Round(uipos.x / 52.0f);
            //     int no = (int)Mathf.Round(uipos.y / 52.0f);
            //     int configId = CommonFuc.SetItemId(row, no, 1);
            //
            //     if (self.excelData.ContainsKey(configId))
            //     {
            //         var list = self.excelData[configId];
            //         if(self.excelData[configId][5] == "0" && self.excelData[configId][6] == "0")
            //             self.excelData[configId][5] = "10";
            //         if(self.excelData[configId][5] == "10" && self.excelData[configId][6] == "0")
            //             self.excelData[configId][6] = "-10";
            //         if(self.excelData[configId][5] == "0" && self.excelData[configId][6] == "-10")
            //             self.excelData[configId][5] = "-10";
            //         if(self.excelData[configId][5] == "-10" && self.excelData[configId][6] == "0")
            //             self.excelData[configId][5] = "0";
            //     }
            // }
        }
    }

    public static class UIMBEditSystem
    {
        public static void CombineItem(this UIMBEditCoponent self, int configId, int level, int fruitId, int row, int no, int offset)
        {
            if (configId == 112004 || configId == 12004)
            {
                Log.Console("error");
            }

            if (self.excelData.ContainsKey(configId))
            {
                var list = self.excelData[configId];
                string itemIds = list[7];
                string[] items = itemIds.Split(',');
                    
                string itemId = "";
                if(items.Length < level)
                {
                    for (int i = 1; i <= level; i++)
                    {
                        if(i <= items.Length)
                            itemId += items[i - 1] + ",";
                        else if(i > items.Length && i < level)
                        {
                            itemId += "0,";
                        }
                        else if (i == level)
                            itemId += fruitId;
                    }
                }
                else
                {
                    items[level] = fruitId.ToString();
                    for (int i = 0; i < items.Length; i++)
                    {
                        if(i < items.Length - 1)
                            itemId += items[i] + ",";
                        else if (i == items.Length - 1)
                            itemId += items[i];
                    }
                }
                self.excelData[configId][7] = itemId;
            }
            
            else
            {
                List<string> list = new List<string>();
                list.Add(null);
                list.Add(null);
                list.Add(configId.ToString());
                list.Add(row.ToString());
                list.Add(no.ToString());
                list.Add("0");
                list.Add(offset.ToString());
                string itemId = "";
                for (int i = 1; i <= level; i++)
                {
                    if (i == level)
                        itemId += fruitId;
                    else
                        itemId += "0,";
                }
                list.Add(itemId);
                self.excelData.Add(configId, list);
            }
        }
    }
}
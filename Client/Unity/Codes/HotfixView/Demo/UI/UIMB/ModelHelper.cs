using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public class ModelHelper
    {
        public List<string> modelAbList;
        public Dictionary<string, List<string>> modelDic;
        public string ABName;
        public string ModelName;
        private static ModelHelper instance;

        public static ModelHelper Instance
        {
            get
            {
                if (instance == null)
                    instance = new ModelHelper();
                return instance;
            }
        }

        public ModelHelper()
        {
            this.modelDic = new Dictionary<string, List<string>>();
            this.modelDic["G03.unity3d"] = new List<string>() { "G03L01_player","G03L02_player"};
            this.modelDic["G05.unity3d"] = new List<string>() { "G05L01_player","G05L02_player","G05L03_player"};
            this.modelDic["L03.unity3d"] = new List<string>() { "L03L01_player","L03L02_player"};
            this.modelDic["L04.unity3d"] = new List<string>() { "L04L01_player","L04L02_player"};
        }
        public Dictionary<string,List<string>> RandomModelList(int count)
        {
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            string abName = "";
            string modelName = "";
            GetModelName(out abName,out modelName);
            while (true)
            {
                int totalCount = 0;
                bool hasSame = false;
                foreach (var value in dic)
                {
                    for (int i = 0; i < value.Value.Count; i++)
                    {
                        if (modelName  == value.Value[i])
                        {
                            hasSame = true;
                        }
                    }
                    totalCount += value.Value.Count;
                }
                if (totalCount >= count)
                    break;
                if (hasSame)
                {
                    ModelHelper.Instance.GetModelName(out abName,out modelName);
                }
                else
                {
                    if (!dic.ContainsKey(abName))
                    {
                        dic[abName] = new List<string>();
                    }
                    dic[abName].Add(modelName);
                }
            }

            return dic;
        }
        public void GetModelName(out string abName,out string modelName)
        {
            List<string> abList = this.modelDic.Keys.ToList();
            int index = RandomHelper.RandomNumber(0,abList.Count);
            abName = abList[index];

            List<string> nameList = this.modelDic[abName].ToList();
            index = RandomHelper.RandomNumber(0, nameList.Count);
            modelName = nameList[index];
        }
    }
}
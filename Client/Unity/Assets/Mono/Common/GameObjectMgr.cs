using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ET
{
    public class GameObjectMgr
    {
        /// <summary>
        /// 查找物体
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject Refer(GameObject obj, string name)
        {
            if (string.Equals(obj.name, name))
            {
                return obj;
            }

            Transform trans = obj.transform;

            int childs = trans.childCount;

            for (int i = 0; i < childs; i++)
            {
                if (string.Equals(trans.GetChild(i).name, name))
                {
                    return trans.GetChild(i).gameObject;
                }
                else
                {
                    if (trans.GetChild(i).childCount > 0)
                    {
                        var c = Refer(trans.GetChild(i).gameObject, name);
                        if (c != null)
                            return c;
                    }
                }
            }
            return null;
        }
    }
}

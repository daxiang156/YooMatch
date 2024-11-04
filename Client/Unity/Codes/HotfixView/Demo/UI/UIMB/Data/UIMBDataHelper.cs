using System.Collections.Generic;
using ILRuntime.Runtime;
using UnityEngine;

namespace ET.Data
{
    public class UIMBDataHelper
    {
        /// <summary>
        /// 单次加钱
        /// </summary>
        public static int AddSMoney = 3;
        public static int ResultMoney = 10;
        private int moneyValue = -1;
        /// <summary>
        /// 基础格子数量
        /// </summary>
        public static int BaseGridNum = 4;
        /// <summary>
        /// 扩展的格子ID
        /// </summary>
        public static int AddGridId = 5;
        private static UIMBDataHelper instance;
        public static UIMBDataHelper Instance
        {
            get
            {
                if (instance == null)
                    instance = new UIMBDataHelper();
                return instance;
            }
        }

        public int MoneyValue
        {
            get
            {
                if (this.moneyValue == -1)
                    this.moneyValue = PlayerPrefs.GetInt("MoneyValue", 0);
                return this.moneyValue;
            }
            set
            {
                this.moneyValue = value;
                PlayerPrefs.SetInt("MoneyValue",this.moneyValue);
            }
        }
        /// <summary>
        /// type == 1 ， 加，   type ==2,减
        /// </summary>
        public void MonyValueCtr(int value,int type)
        {
            if (type == 1)
            {
                this.MoneyValue += value;
            }
            else
            {
                if (this.moneyValue - value > 0)
                {
                    this.MoneyValue -= value;
                }
                else
                {
                    this.MoneyValue = 0;
                }
            }
        }
        
        private List<int> unLockGrids = new List<int>();

        public List<int> UnLockGrids()
        {
            return this.unLockGrids;
        }

        public void SaveGrid(int gridId)
        {
            if (unLockGrids.IndexOf(gridId) == -1)
            {
                this.unLockGrids.Add(gridId);
            }
        }

        public bool GetGridState(int gridId)
        {
            return this.unLockGrids.IndexOf(gridId) != -1;
        }

        public void ResetGridState()
        {
            this.unLockGrids.Clear();
        }
    }
}
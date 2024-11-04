using UnityEngine;

namespace ET.Codes.Hotfix.GameLogic.Common
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        /// <summary>
        /// 挂载点
        /// </summary>
        private static GameObject mountNode = null;
        /// <summary>
        /// 单件
        /// </summary>
        protected static T instance = null;
        /// <summary>
        /// 获得单件
        /// </summary>
        public static T Instance
        {
            get { return instance ?? (instance = GetInstanceInternal()); }
        }
        /// <summary>
        /// 获得单件
        /// </summary>
        public static T GetSingleton()
        {
            return instance ?? (instance = GetInstanceInternal());
        }

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private bool init = false;

        /// <summary>
        /// 内部创建流程
        /// </summary>
        /// <returns></returns>
        private static T GetInstanceInternal()
        {
            T t = null;
            if (mountNode == null)
            {
                mountNode = new GameObject(typeof(T).Name);
            }
            t = mountNode.GetComponent<T>();
            if (t == null)
            {
                t = mountNode.AddComponent<T>();
            }
            return t;
        }

        /// <summary>
        /// 安全删除
        /// </summary>
        public static void SafeRelease()
        {
            if (instance == null)
            {
                return;
            }

            instance.Release();
        }

        /// <summary>
        /// 支持反序列化单件
        /// </summary>
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                enabled = false;
                Log.Debug("MonoSingleton只允许存在一个.");
            }
            else if (!init)
            {
                init = true;
                instance = this as T;
                if (transform.parent == null)
                {
                    Log.Debug("transform.parent == null");
                }
                Initialize();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Initialize()
        {

        }

        /// <summary>
        /// 释放
        /// </summary>
        public virtual void Release()
        {
            instance = null;
            if (mountNode != null)
            {
                DestroyImmediate(mountNode);
            }
        }
    }
}
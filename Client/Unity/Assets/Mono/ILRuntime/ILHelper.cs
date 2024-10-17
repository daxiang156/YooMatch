﻿// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Net;
// using ILRuntime.Runtime.Intepreter;
// using ProtoBuf;
// using UnityEngine;
//
// namespace ET
// {
//     public static class ILHelper
//     {
//         public static List<Type> list = new List<Type>();
//
//         public static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
//         {
//             list.Add(typeof(Dictionary<int, ILTypeInstance>));
//             list.Add(typeof(Dictionary<long, ILTypeInstance>));
//             list.Add(typeof(Dictionary<string, ILTypeInstance>));
//             list.Add(typeof(Dictionary<int, int>));
//             list.Add(typeof(Dictionary<object, object>));
//             list.Add(typeof(Dictionary<int, object>));
//             list.Add(typeof(Dictionary<long, object>));
//             list.Add(typeof(Dictionary<long, int>));
//             list.Add(typeof(Dictionary<int, long>));
//             list.Add(typeof(Dictionary<string, long>));
//             list.Add(typeof(Dictionary<string, int>));
//             list.Add(typeof(Dictionary<string, object>));
//             list.Add(typeof(List<ILTypeInstance>));
//             list.Add(typeof(List<int>));
//             list.Add(typeof(List<long>));
//             list.Add(typeof(List<string>));
//             list.Add(typeof(List<object>));
//             list.Add(typeof(ETTask<int>));
//             list.Add(typeof(ETTask<long>));
//             list.Add(typeof(ETTask<string>));
//             list.Add(typeof(ETTask<object>));
//             list.Add(typeof(ETTask<AssetBundle>));
//             list.Add(typeof(ETTask<UnityEngine.Object[]>));
//             list.Add(typeof(ListComponent<ILTypeInstance>));
//             list.Add(typeof(ListComponent<ETTask>));
//             list.Add(typeof(ListComponent<Vector3>));
//             
//             // 注册重定向函数
//
//             // 注册委托
//             appdomain.DelegateManager.RegisterMethodDelegate<List<object>>();
//             appdomain.DelegateManager.RegisterMethodDelegate<object>();
//             appdomain.DelegateManager.RegisterMethodDelegate<bool>();
//             appdomain.DelegateManager.RegisterMethodDelegate<string>();
//             appdomain.DelegateManager.RegisterMethodDelegate<float>();
//             appdomain.DelegateManager.RegisterMethodDelegate<long, int>();
//             appdomain.DelegateManager.RegisterMethodDelegate<long, MemoryStream>();
//             appdomain.DelegateManager.RegisterMethodDelegate<long, IPEndPoint>();
//             appdomain.DelegateManager.RegisterMethodDelegate<ILTypeInstance>();
//             appdomain.DelegateManager.RegisterMethodDelegate<AsyncOperation>();
//             
//             
//             appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Events.UnityAction>();
//             appdomain.DelegateManager.RegisterFunctionDelegate<System.Object, ET.ETTask>();
//             appdomain.DelegateManager.RegisterFunctionDelegate<ILTypeInstance, bool>();
//             appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.String, System.Int32>, System.String>();
//             appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.Int32>, System.Boolean>();
//             appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.String, System.Int32>, System.Int32>();
//             appdomain.DelegateManager.RegisterFunctionDelegate<List<int>, int>();
//             appdomain.DelegateManager.RegisterFunctionDelegate<List<int>, bool>();
//             appdomain.DelegateManager.RegisterFunctionDelegate<int, bool>();//Linq
//             appdomain.DelegateManager.RegisterFunctionDelegate<int, int, int>();//Linq
//             appdomain.DelegateManager.RegisterFunctionDelegate<KeyValuePair<int, List<int>>, bool>();
//             appdomain.DelegateManager.RegisterFunctionDelegate<KeyValuePair<int, int>, KeyValuePair<int, int>, int>();
//             
//             appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
//             {
//                 return new UnityEngine.Events.UnityAction(() =>
//                 {
//                     ((Action)act)();
//                 });
//             });
//             
//             appdomain.DelegateManager.RegisterDelegateConvertor<Comparison<KeyValuePair<int, int>>>((act) =>
//             {
//                 return new Comparison<KeyValuePair<int, int>>((x, y) =>
//                 {
//                     return ((Func<KeyValuePair<int, int>, KeyValuePair<int, int>, int>)act)(x, y);
//                 });
//             });
//             
//             // 注册适配器
//             RegisterAdaptor(appdomain);
//             
//             //注册Json的CLR
//             LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
//             //注册ProtoBuf的CLR
//             PType.RegisterILRuntimeCLRRedirection(appdomain);
//            
//             
//             ////////////////////////////////////
//             // CLR绑定的注册，一定要记得将CLR绑定的注册写在CLR重定向的注册后面，因为同一个方法只能被重定向一次，只有先注册的那个才能生效
//             ////////////////////////////////////
//             Type t = Type.GetType("ILRuntime.Runtime.Generated.CLRBindings");
//             if (t != null)
//             {
//                 t.GetMethod("Initialize")?.Invoke(null, new object[] { appdomain });
//             }
//             //ILRuntime.Runtime.Generated.CLRBindings.Initialize(appdomain);
//         }
//         
//         public static void RegisterAdaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
//         {
//             //注册自己写的适配器
//             appdomain.RegisterCrossBindingAdaptor(new IAsyncStateMachineClassInheritanceAdaptor());
//         }
//     }
// }

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using ILRuntime.Runtime.Intepreter;
using ProtoBuf;
using UnityEngine;

namespace ET
{
    public static class ILHelper
    {
        public static List<Type> list = new List<Type>();

        public static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
            list.Add(typeof(Dictionary<int, ILTypeInstance>));
            list.Add(typeof(Dictionary<long, ILTypeInstance>));
            list.Add(typeof(Dictionary<string, ILTypeInstance>));
            list.Add(typeof(Dictionary<int, int>));
            list.Add(typeof(Dictionary<object, object>));
            list.Add(typeof(Dictionary<int, object>));
            list.Add(typeof(Dictionary<long, object>));
            list.Add(typeof(Dictionary<long, int>));
            list.Add(typeof(Dictionary<int, long>));
            list.Add(typeof(Dictionary<string, long>));
            list.Add(typeof(Dictionary<string, int>));
            list.Add(typeof(Dictionary<string, object>));
            list.Add(typeof(List<ILTypeInstance>));
            list.Add(typeof(List<int>));
            list.Add(typeof(List<long>));
            list.Add(typeof(List<string>));
            list.Add(typeof(List<object>));
            list.Add(typeof(ETTask<int>));
            list.Add(typeof(ETTask<long>));
            list.Add(typeof(ETTask<string>));
            list.Add(typeof(ETTask<object>));
            list.Add(typeof(ETTask<AssetBundle>));
            list.Add(typeof(ETTask<UnityEngine.Object[]>));
            list.Add(typeof(ListComponent<ILTypeInstance>));
            list.Add(typeof(ListComponent<ETTask>));
            list.Add(typeof(ListComponent<Vector3>));
            
            // 注册重定向函数

            // 注册委托
            appdomain.DelegateManager.RegisterMethodDelegate<List<object>>();
            appdomain.DelegateManager.RegisterMethodDelegate<object>();
            appdomain.DelegateManager.RegisterMethodDelegate<bool>();
            appdomain.DelegateManager.RegisterMethodDelegate<string>();
            appdomain.DelegateManager.RegisterMethodDelegate<float>();
            appdomain.DelegateManager.RegisterMethodDelegate<long, int>();
            appdomain.DelegateManager.RegisterMethodDelegate<long, MemoryStream>();
            appdomain.DelegateManager.RegisterMethodDelegate<long, IPEndPoint>();
            appdomain.DelegateManager.RegisterMethodDelegate<ILTypeInstance>();
            appdomain.DelegateManager.RegisterMethodDelegate<AsyncOperation>();
            
            
            appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Events.UnityAction>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Object, ET.ETTask>();
            appdomain.DelegateManager.RegisterFunctionDelegate<ILTypeInstance, bool>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.String, System.Int32>, System.String>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.Int32>, System.Boolean>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.String, System.Int32>, System.Int32>();
            appdomain.DelegateManager.RegisterFunctionDelegate<List<int>, int>();
            appdomain.DelegateManager.RegisterFunctionDelegate<List<int>, bool>();
            appdomain.DelegateManager.RegisterFunctionDelegate<int, bool>();//Linq
            appdomain.DelegateManager.RegisterFunctionDelegate<int, int, int>();//Linq
            appdomain.DelegateManager.RegisterFunctionDelegate<KeyValuePair<int, List<int>>, bool>();
            appdomain.DelegateManager.RegisterFunctionDelegate<KeyValuePair<int, int>, KeyValuePair<int, int>, int>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Vector2>();
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.Vector2>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<UnityEngine.Vector2>((arg0) =>
                {
                    ((Action<UnityEngine.Vector2>)act)(arg0);
                });
            });


            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
            {
                return new UnityEngine.Events.UnityAction(() =>
                {
                    ((Action)act)();
                });
            });
            
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Object[], System.Boolean>();
            appdomain.DelegateManager.RegisterDelegateConvertor<Comparison<KeyValuePair<int, int>>>((act) =>
            {
                return new Comparison<KeyValuePair<int, int>>((x, y) =>
                {
                    return ((Func<KeyValuePair<int, int>, KeyValuePair<int, int>, int>)act)(x, y);
                });
            });
            
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.String>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.String>((arg0) =>
                {
                    ((Action<System.String>)act)(arg0);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Boolean>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.Boolean>((arg0) =>
                {
                    ((Action<System.Boolean>)act)(arg0);
                });
            });
            
            appdomain.DelegateManager.RegisterDelegateConvertor<ET.EventCallback>((act) =>
            {
                return new ET.EventCallback((userInfo) =>
                {
                    return ((Func<System.Object[], System.Boolean>)act)(userInfo);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Single>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.Single>((arg0) =>
                {
                    ((Action<System.Single>)act)(arg0);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.TweenCallback>((act) =>
            {
                return new DG.Tweening.TweenCallback(() =>
                {
                    ((Action)act)();
                });
            });
            
            
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Int32>();
            appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOSetter<System.Int32>>((act) =>
            {
                return new DG.Tweening.Core.DOSetter<System.Int32>((pNewValue) =>
                {
                    ((Action<System.Int32>)act)(pNewValue);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOGetter<System.Int32>>((act) =>
            {
                return new DG.Tweening.Core.DOGetter<System.Int32>(() =>
                {
                    return ((Func<System.Int32>)act)();
                });
            });
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Single>();
            appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOSetter<System.Single>>((act) =>
            {
                return new DG.Tweening.Core.DOSetter<System.Single>((pNewValue) =>
                {
                    ((Action<System.Single>)act)(pNewValue);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOGetter<System.Single>>((act) =>
            {
                return new DG.Tweening.Core.DOGetter<System.Single>(() =>
                {
                    return ((Func<System.Single>)act)();
                });
            });

            appdomain.DelegateManager.RegisterMethodDelegate<Spine.TrackEntry>();
            appdomain.DelegateManager.RegisterDelegateConvertor<Spine.AnimationState.TrackEntryDelegate>((act) =>
            {
                return new Spine.AnimationState.TrackEntryDelegate((trackEntry) =>
                {
                    ((Action<Spine.TrackEntry>)act)(trackEntry);
                });
            });


            // 注册适配器
            RegisterAdaptor(appdomain);
            
            //注册Json的CLR
            LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
            //注册ProtoBuf的CLR
            PType.RegisterILRuntimeCLRRedirection(appdomain);
           
            
            ////////////////////////////////////
            // CLR绑定的注册，一定要记得将CLR绑定的注册写在CLR重定向的注册后面，因为同一个方法只能被重定向一次，只有先注册的那个才能生效
            ////////////////////////////////////
            Type t = Type.GetType("ILRuntime.Runtime.Generated.CLRBindings");
            if (t != null)
            {
                t.GetMethod("Initialize")?.Invoke(null, new object[] { appdomain });
            }
            //ILRuntime.Runtime.Generated.CLRBindings.Initialize(appdomain);
        }
        
        public static void RegisterAdaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
            //注册自己写的适配器
            appdomain.RegisterCrossBindingAdaptor(new IAsyncStateMachineClassInheritanceAdaptor());
        }
    }
}
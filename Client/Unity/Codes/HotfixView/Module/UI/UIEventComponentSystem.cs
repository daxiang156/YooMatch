using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
	[ObjectSystem]
	public class UIEventComponentAwakeSystem : AwakeSystem<UIEventComponent>
	{
		public override void Awake(UIEventComponent self)
		{
			UIEventComponent.Instance = self;
			
			GameObject uiRoot = GameObject.Find("/Global/UI");
			ReferenceCollector referenceCollector = uiRoot.GetComponent<ReferenceCollector>();
			
			self.UILayers.Add((int)UILayer.Hidden, referenceCollector.Get<GameObject>(UILayer.Hidden.ToString()).transform);
			self.UILayers.Add((int)UILayer.Low, referenceCollector.Get<GameObject>(UILayer.Low.ToString()).transform);

			Transform low3 = uiRoot.transform.Find("Low3");
			if(low3 != null)
				self.UILayers.Add((int)ETUILayer.Low3, low3.transform);
			else
			{
				GameObject Mid = referenceCollector.Get<GameObject>(ETUILayer.Mid.ToString());
				low3 = GameObject.Instantiate(Mid).transform;
				low3.name = "Low3";
				low3.GetComponent<Canvas>().sortingOrder = 8;
				low3.transform.SetParent(Mid.transform.parent.transform);
				low3.transform.SetSiblingIndex(3);
				self.UILayers.Add((int)ETUILayer.Low3, low3.transform);
			}

	//		self.UILayers.Add((int)UILayer.Low2, referenceCollector.Get<GameObject>(UILayer.Low2.ToString()).transform);
			self.UILayers.Add((int)UILayer.Mid, referenceCollector.Get<GameObject>(UILayer.Mid.ToString()).transform);
	//		self.UILayers.Add((int)UILayer.Mid2, referenceCollector.Get<GameObject>(UILayer.Mid2.ToString()).transform);
			self.UILayers.Add((int)UILayer.High, referenceCollector.Get<GameObject>(UILayer.High.ToString()).transform);

			var uiEvents = Game.EventSystem.GetTypes(typeof (UIEventAttribute));
			foreach (Type type in uiEvents)
			{
				object[] attrs = type.GetCustomAttributes(typeof(UIEventAttribute), false);
				if (attrs.Length == 0)
				{
					continue;
				}

				UIEventAttribute uiEventAttribute = attrs[0] as UIEventAttribute;
				AUIEvent aUIEvent = Activator.CreateInstance(type) as AUIEvent;
				self.UIEvents.Add(uiEventAttribute.UIType, aUIEvent);
			}
		}
	}
	
	/// <summary>
	/// 管理所有UI GameObject 以及UI事件
	/// </summary>
	public static class UIEventComponentSystem
	{
		public static async ETTask<UI> OnCreate(this UIEventComponent self, UIComponent uiComponent, string uiType, UILayer uiLayer,
												object data = null,int aniType = 1)
		{
			try
			{
				UI ui = await self.UIEvents[uiType].OnCreate(uiComponent, uiLayer,data);
				if (aniType == 0)
				{
					EventDispatcher.PostEvent(ETEventName.UIAniType,null,0);
				}

				return ui;
			}
			catch (Exception e)
			{
				throw new Exception($"on create ui error: {uiType}", e);
			}
		}
		
		public static void OnRemove(this UIEventComponent self, UIComponent uiComponent, string uiType,int aniType = 0)
		{
			try
			{
				self.UIEvents[uiType].OnRemove(uiComponent);
				if (aniType == 1)
				{
					EventDispatcher.PostEvent(ETEventName.UIAniType,null,1);
				}
			}
			catch (Exception e)
			{
				throw new Exception($"on remove ui error: {uiType}", e);
			}
			
		}
	}
}
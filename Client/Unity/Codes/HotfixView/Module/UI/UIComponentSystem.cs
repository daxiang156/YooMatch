using System.Collections.Generic;

namespace ET
{
	/// <summary>
	/// 管理Scene上的UI
	/// </summary>
	public static class UIComponentSystem
	{
		private static string curOpenType = "";
		/// <summary>
		/// 
		/// <param name="otherUIAni">上下左右UI展开收起</param>
		/// <returns></returns>
		public static async ETTask<UI> Create(this UIComponent self, string uiType, UILayer uiLayer,object data = null,
		bool isRepeat = false, int index = 100,int otherUIAni = 1)
		{
			if (uiType == UIType.UIMainCity && GameDataMgr.Instance.Platflam() == PlatForm.Android)
			{
				Log.Console("maincity is null");
				return null;
			}
			if (curOpenType != uiType)
			{
				if (!isRepeat && self.UIs.ContainsKey(uiType))
				{
					curOpenType = "";
					if(uiType != UIType.UIMainCity && uiType != UIType.UIChatUnder)
						self.UIs[uiType].GameObject.transform.SetAsLastSibling();
					else
					{
						self.UIs[uiType].GameObject.transform.SetAsFirstSibling();
					}
					return self.UIs[uiType];
				}
				curOpenType = uiType;
				UI ui = await UIEventComponent.Instance.OnCreate(self, uiType, uiLayer, data,otherUIAni);
				self.UIs.Add(uiType, ui);
				if(index == 0)
					self.UIs[uiType].GameObject.transform.SetAsFirstSibling();
				curOpenType = "";
				return ui;
			}

			return null;
		}

		public static void Remove(this UIComponent self, string uiType,int otherUIAni = 1)
		{
			if (!self.UIs.TryGetValue(uiType, out UI ui))
			{
				return;
			}
			
			UIEventComponent.Instance.OnRemove(self, uiType,otherUIAni);
			
			self.UIs.Remove(uiType);
			ui.Dispose();
		}

		public static UI Get(this UIComponent self, string name)
		{
			UI ui = null;
			self.UIs.TryGetValue(name, out ui);
			return ui;
		}
	}
}
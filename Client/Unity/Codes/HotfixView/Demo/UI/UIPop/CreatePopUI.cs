using ET.EventType;

namespace ET
{
    public class CreatPopUI : AEvent<EventType.EnterPopUI>
    {
        protected override async ETTask Run(EnterPopUI param)
        {
            await UIHelper.Create(param.ZoneScene, UIType.UIPop, UILayer.Mid);
        }
    }
}
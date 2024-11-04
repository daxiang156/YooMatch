using ET.EventType;

namespace ET
{
    public class CreateMBMoreUI : AEvent<EventType.CreateMBMore>
    {
        protected override async ETTask Run(CreateMBMore param)
        {
            UIHelper.Create(param.ZoneScene,UIType.UIMBMore, UILayer.Mid).Coroutine();
            await ETTask.CompletedTask;
        }
    }
    
}
namespace ET
{
    public abstract class AUIEvent
    {
        public abstract ETTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer,object data = null);
        public abstract void OnRemove(UIComponent uiComponent);


    }
}
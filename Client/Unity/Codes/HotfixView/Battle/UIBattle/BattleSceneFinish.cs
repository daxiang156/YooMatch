namespace ET.Battle.UIBattle
{
    public class BattleSceneFinish: AEvent<EventType.BattleSceneFinish>
    {
        protected override async ETTask Run(EventType.BattleSceneFinish args)
        {
            args.battleScene.AddComponent<BattleComponent>();
            await ETTask.CompletedTask;
        }
    }
}
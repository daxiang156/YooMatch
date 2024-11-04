
namespace ET
{
    public class GameInfoComponent : Entity, IAwake,IDestroy
    {
        public static GameInfoComponent Instance;
        public M2C_GameResult result;
        public int isWin = 0;
        public bool isNext = false;
        public int rank = 1;
        public bool isShowWin = false;
    }
}
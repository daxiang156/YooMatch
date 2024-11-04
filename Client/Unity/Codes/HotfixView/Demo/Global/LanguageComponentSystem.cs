namespace ET
{
    [ObjectSystem]
    public class LanguageComponentSystem: AwakeSystem<LanguageComponent>
    {
        public override void Awake(LanguageComponent self)
        {
            LanguageComponent.Instance = self;
            EventDispatcher.AddObserver(this, EventName.Language, (object[] userInfo) =>
            {
                int languageId = (int) userInfo[0];
                if (LanguageConfigCategory.Instance.Get(languageId) == null)
                {
                    GameDataMgr.Instance.language = languageId.ToString();
                    return false;
                }

                if(GameDataMgr.Instance.languageSelect == LanguageSelect.Indonesia)
                    GameDataMgr.Instance.language = LanguageConfigCategory.Instance.Get(languageId).Indonesian;
                else if(GameDataMgr.Instance.languageSelect == LanguageSelect.English)
                    GameDataMgr.Instance.language = LanguageConfigCategory.Instance.Get(languageId).English;
                else if (GameDataMgr.Instance.languageSelect == LanguageSelect.AutoSelectByIP)
                {
                    if (GameDataMgr.Instance.country == CountryLanguage.Indonesia)
                    {
                        GameDataMgr.Instance.language = LanguageConfigCategory.Instance.Get(languageId).Indonesian;
                    }
                    else
                    {
                        GameDataMgr.Instance.language = LanguageConfigCategory.Instance.Get(languageId).English;
                    }
                }
                else if ((LanguageSelect2)GameDataMgr.Instance.languageSelect == LanguageSelect2.Vietnam)
                {
                    GameDataMgr.Instance.language = LanguageConfigCategory.Instance.Get(languageId).Vietnam;
                }
                else if ((LanguageSelect2)GameDataMgr.Instance.languageSelect == LanguageSelect2.Brazil)
                {
                    GameDataMgr.Instance.language = LanguageConfigCategory.Instance.Get(languageId).Brazil;
                }
                else
                {
                    GameDataMgr.Instance.language = LanguageConfigCategory.Instance.Get(languageId).English;
                }
                if(string.IsNullOrEmpty(GameDataMgr.Instance.language))
                {
                    GameDataMgr.Instance.language = LanguageConfigCategory.Instance.Get(languageId).English;
                }
                return false;
            }, null);
        }
    }
    
    public static class LanguageSystem
    {
        public static string GetLanguage(this LanguageComponent self, int excelId)
        {
            if (LanguageConfigCategory.Instance.Get(excelId) == null)
            {
                return excelId.ToString();
            }

            string str = "";
            if(GameDataMgr.Instance.languageSelect == LanguageSelect.Indonesia)
                 str = LanguageConfigCategory.Instance.Get(excelId).Indonesian;
            else if(GameDataMgr.Instance.languageSelect == LanguageSelect.English)
                 str = LanguageConfigCategory.Instance.Get(excelId).English;
            else if (GameDataMgr.Instance.languageSelect == LanguageSelect.AutoSelectByIP)
            {
                if (GameDataMgr.Instance.country == CountryLanguage.Indonesia)
                {
                    str = LanguageConfigCategory.Instance.Get(excelId).Indonesian;
                }
                else
                {
                    str = LanguageConfigCategory.Instance.Get(excelId).English;
                }
            }else if ((LanguageSelect2) GameDataMgr.Instance.languageSelect == LanguageSelect2.Vietnam)
            {
                str = LanguageConfigCategory.Instance.Get(excelId).Vietnam;
            }else if ((LanguageSelect2) GameDataMgr.Instance.languageSelect == LanguageSelect2.Brazil)
            {
                str = LanguageConfigCategory.Instance.Get(excelId).Brazil;
            }
            else
            {
                str = LanguageConfigCategory.Instance.Get(excelId).English;
            }

            if (string.IsNullOrEmpty(str))
                return LanguageConfigCategory.Instance.Get(excelId).English;
            return str;
        }
    }
    
    [ObjectSystem]
    public class LanguageDestroy: DestroySystem<LanguageComponent>
    {
        public override void Destroy(LanguageComponent self)
        {
            EventDispatcher.RemoveObserver(EventName.Language);
        }
    }
}
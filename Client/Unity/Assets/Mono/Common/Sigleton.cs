public abstract class Singleton<T> where T : class, new()
{
    private static T instance = null;
    public static bool HasInstance { get { return instance != null; } }
    public static T Instance
    {
        get { return instance ?? (instance = new T()); }
    }

    /// <summary>
    /// 获得单件
    /// </summary>
    public static T GetSingleton()
    {
        return instance ?? (instance = new T());
    }

}
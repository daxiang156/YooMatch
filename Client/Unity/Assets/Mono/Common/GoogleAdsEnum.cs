using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 广告平台
/// </summary>
public enum AdPlatform
{
    Google_AdMob = 1,       //google广告
    MaxAppLovin,        //max广告
}
public enum GoogleAdsEnum
{
    /// <summary>
    /// 横幅广告
    /// </summary>
    Google_BannerAd = 1,
    /// <summary>
    /// 插页广告
    /// </summary>
    Google_InterstitialAd = 2,
    /// <summary>
    /// 开屏广告
    /// </summary>
    Google_AppOpenAd = 3,
    /// <summary>
    /// 原生广告
    /// </summary>
    Google_NativeAd = 4,
    /// <summary>
    /// 激励广告
    /// </summary>
    Google_RewardAd = 5,
}
/// <summary>
/// google广告观看途径
/// </summary>
public enum GoogleAdsPath
{
    /// <summary>
    /// 消消乐
    /// </summary>
    GoogleAd_Path1 = 1,
    ResultLose = 2,//结算
    MBMap = 3,//体力
    MBGameAddGrid = 4,//放一边
    MBGameResort = 5,//打乱
    MBGameRelast = 6,//撤回
    MBRevive = 7,//复活
    BigTurnTbale = 8,//大转盘
    GoogleAd_Path_null = 999999,
}
using CommunityToolkit.Mvvm.ComponentModel;

namespace BF1ServerTools.Data;

public partial class RuleTeamModel : ObservableObject
{
    /// <summary>
    /// 最大击杀
    /// </summary>
    [ObservableProperty]
    private int maxKill;

    /// <summary>
    /// 计算KD标志
    /// </summary>
    [ObservableProperty]
    private int flagKD;

    /// <summary>
    /// 最大KD
    /// </summary>
    [ObservableProperty]
    private float maxKD;

    /// <summary>
    /// 计算KPM标志
    /// </summary>
    [ObservableProperty]
    private int flagKPM;

    /// <summary>
    /// 最大KPM
    /// </summary>
    [ObservableProperty]
    private float maxKPM;

    /// <summary>
    /// 最低等级
    /// </summary>
    [ObservableProperty]
    private int minRank;

    /// <summary>
    /// 最低等级
    /// </summary>
    [ObservableProperty]
    private int maxRank;

    /// <summary>
    /// 最大生涯KD
    /// </summary>
    [ObservableProperty]
    private float lifeMaxKD;

    /// <summary>
    /// 最大生涯KPM
    /// </summary>
    [ObservableProperty]
    private float lifeMaxKPM;

    /// <summary>
    /// 最大生涯武器星数
    /// </summary>
    [ObservableProperty]
    private int lifeMaxWeaponStar;

    /// <summary>
    /// 最大生涯载具星数
    /// </summary>
    [ObservableProperty]
    private int lifeMaxVehicleStar;

    /// <summary>
    /// 计算最大生涯命中率等级
    /// </summary>
    [ObservableProperty]
    private int lifeMaxAccuracyRatioLevel;

    /// <summary>
    /// 最大生涯命中率
    /// </summary>
    [ObservableProperty]
    private float lifeMaxAccuracyRatio;

    /// <summary>
    /// 计算最大生涯爆头率等级
    /// </summary>
    [ObservableProperty]
    private int lifeMaxHeadShotRatioLevel;

    /// <summary>
    /// 最大生涯爆头率
    /// </summary>
    [ObservableProperty]
    private float lifeMaxHeadShotRatio;

    /// <summary>
    /// 计算最大生涯胜率等级
    /// </summary>
    [ObservableProperty]
    private int lifeMaxWRLevel;

    /// <summary>
    /// 最大生涯胜率
    /// </summary>
    [ObservableProperty]
    private float lifeMaxWR;
    /// <summary>
    /// 最大生涯胜率
    /// </summary>
    [ObservableProperty]
    private int scoreLimt;
    /// <summary>
    /// 最大生涯胜率
    /// </summary>
    [ObservableProperty]
    private int scoreGap;

}

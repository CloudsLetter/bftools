namespace BF1ServerTools.Data;

public class ServerRule
{
    public int MaxKill { get; set; } = 0;

    public int FlagKD { get; set; } = 0;
    public float MaxKD { get; set; } = 0.00f;

    public int FlagKPM { get; set; } = 0;
    public float MaxKPM { get; set; } = 0.00f;

    public int MinRank { get; set; } = 0;
    public int MaxRank { get; set; } = 0;

    public float LifeMaxKD { get; set; } = 0.00f;
    public float LifeMaxKPM { get; set; } = 0.00f;
    public int LifeMaxWeaponStar { get; set; } = 0;
    public int LifeMaxVehicleStar { get; set; } = 0;

    public int LifeMaxAccuracyRatioLevel { get; set; } = 0;
    public float LifeMaxAccuracyRatio { get; set; } = 0.00f;
    public int LifeMaxHeadShotRatioLevel { get; set; } = 0;
    public float LifeMaxHeadShotRatio { get; set; } = 0;
    public int LifeMaxWRLevel { get; set; } = 0;

    public float LifeMaxWR { get; set; } = 0.00f;
    public int ScoreLimit { get; set; } = 0;
    public int ScoreGap { get; set; } = 0;

    public int MaxScore { get; set; }
    public int FlagKDPro { get; set; }
    public float MaxKDPro { get; set; }
    public int FlagKPMPro { get; set; }
    public float MaxKPMPro { get; set; }
}

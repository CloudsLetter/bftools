using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CloudRule
{
    public bool WhiteLifeKD { get; set; }
    public bool WhiteLifeKPM { get; set; }
    public bool WhiteLifeWeaponStar { get; set; }
    public bool WhiteLifeVehicleStar { get; set; }
    public bool WhiteKill { get; set; }
    public bool WhiteKD { get; set; }
    public bool WhiteKPM { get; set; }
    public bool WhiteRank { get; set; }
    public bool WhiteWeapon { get; set; }
    public bool WhiteToggleTeam { get; set; }
    public bool WhiteWR { get; set; }
    public bool Allow2LowSocre { get; set; }
    public int Team1MaxKill { get; set; }
    public int Team1FlagKD { get; set; }
    public float Team1MaxKD { get; set; }
    public int Team1FlagKPM { get; set; }
    public float Team1MaxKPM { get; set; }
    public int Team1MinRank { get; set; }
    public int Team1MaxRank { get; set; }
    public int Team1ScoreLimt { get; set; }
    public int Team1ScoreGap { get; set; }
    public float Team1LifeMaxKD { get; set; }
    public float Team1LifeMaxKPM { get; set; }
    public int Team1LifeMaxWRLevel { get; set; }
    public float Team1LifeMaxWR { get; set; }
    public int Team1LifeMaxWeaponStar { get; set; }
    public int Team1LifeMaxVehicleStar { get; set; }
    public string Team1WeaponLimit { get; set; }
    public int Team2MaxKill { get; set; }
    public int Team2FlagKD { get; set; }
    public float Team2MaxKD { get; set; }
    public int Team2FlagKPM { get; set; }
    public float Team2MaxKPM { get; set; }
    public int Team2MinRank { get; set; }
    public int Team2MaxRank { get; set; }
    public float Team2LifeMaxKD { get; set; }
    public float Team2LifeMaxKPM { get; set; }
    public int Team2LifeMaxWRLevel { get; set; }
    public float Team2LifeMaxWR { get; set; }
    public int Team2LifeMaxWeaponStar { get; set; }
    public int Team2LifeMaxVehicleStar { get; set; }
    public string Team2WeaponLimit { get; set; }
    public long ServerId { get; set; }
    public long GameId { get; set; }
    public string Guid { get; set; }
    public long OperatorPersonaId { get; set; }
}



﻿namespace BF1ServerTools.Data;

public class LifePlayerData
{
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public long PersonaId { get; set; }

    public float KD { get; set; }
    public float KPM { get; set; }
    public int Time { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int roundsPlayed { get; set; }
    public float LifeMaxAccuracyRatio { get; set; }
    public float LifeMaxHeadShotRatio { get; set; }
    public float LifeMaxWR { get; set; }
    public bool IsWeaponOK { get; set; }
    public bool IsVehicleOK { get; set; }
    public List<WeaponVehicleInfo> WeaponInfos { get; set; }
    public List<WeaponVehicleInfo> VehicleInfos { get; set; }
    public class WeaponVehicleInfo
    {
        public string Name { get; set; }
        public int Kill { get; set; }
        public int Star { get; set; }
    }
}

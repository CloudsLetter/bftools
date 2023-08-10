using BF1ServerTools.Data;

namespace BF1ServerTools;

public static class Globals
{
    /// <summary>
    /// 玩家列表排序规则
    /// </summary>
    public static OrderBy OrderBy = OrderBy.Score;

    /// <summary>
    /// 是否使用模式1
    /// </summary>
    public static bool IsUseMode1 = true;

    ///////////////////////////////////////////////////////

    /// <summary>
    /// 模式1 玩家Avatar
    /// </summary>
    public static string Avatar1 = string.Empty;
    /// <summary>
    /// 模式2 玩家Avatar
    /// </summary>
    public static string Avatar2 = string.Empty;
    /// <summary>
    /// 玩家Avatar
    /// </summary>
    public static string Avatar
    {
        get
        {
            return IsUseMode1 ? Avatar1 : Avatar2;
        }
    }

    /// <summary>
    /// 模式1 玩家DisplayName
    /// </summary>
    public static string DisplayName1 = string.Empty;
    /// <summary>
    /// 模式2 玩家DisplayName
    /// </summary>
    public static string DisplayName2 = string.Empty;
    /// <summary>
    /// 全局玩家DisplayName
    /// </summary>
    public static string DisplayName
    {
        get
        {
            return IsUseMode1 ? DisplayName1 : DisplayName2;
        }
    }

    /// <summary>
    /// 模式1 玩家PersonaId
    /// </summary>
    public static long PersonaId1 = 0;
    /// <summary>
    /// 模式2 玩家PersonaId
    /// </summary>
    public static long PersonaId2 = 0;
    /// <summary>
    /// 全局PersonaId
    /// </summary>
    public static long PersonaId
    {
        get
        {
            return IsUseMode1 ? PersonaId1 : PersonaId2;
        }
    }

    ///////////////////////////////////////////////////////

    /// <summary>
    /// 玩家Remid
    /// </summary>
    public static string Remid = string.Empty;
    /// <summary>
    /// 玩家Sid
    /// </summary>
    public static string Sid = string.Empty;
    /// <summary>
    /// 玩家登录令牌，有效期4小时
    /// </summary>
    public static string AccessToken = string.Empty;

    /// <summary>
    /// 模式1 玩家SessionId
    /// </summary>
    public static string SessionId1 = string.Empty;
    /// <summary>
    /// 模式2 玩家SessionId
    /// </summary>
    public static string SessionId2 = string.Empty;
    /// <summary>
    /// 全局玩家SessionId
    /// </summary>
    public static string SessionId
    {
        get
        {
            return IsUseMode1 ? SessionId1 : SessionId2;
        }
    }

    /// <summary>
    /// 当前服务器游戏Id
    /// </summary>
    public static long GameId = 0;
    /// <summary>
    /// 当前服务器Id
    /// </summary>
    public static int ServerId = 0;
    /// <summary>
    /// 当前服务器游戏Guid
    /// </summary>
    public static string PersistedGameId = string.Empty;

    ///////////////////////////////////////////////////////

    /// <summary>
    /// 判断当前玩家是否为管理员
    /// </summary>
    /// <returns></returns>
    public static bool LoginPlayerIsAdmin
    {
        get
        {
            if (IsUseMode1)
                return ServerAdmins_PID.Contains(PersonaId1);
            else
                return ServerAdmins_PID.Contains(PersonaId2);
        }
    }

    /// <summary>
    /// 服务器管理员，PID
    /// </summary>
    public static List<long> ServerAdmins_PID = new();
    /// <summary>
    /// 服务器VIP
    /// </summary>
    public static List<long> ServerVIPs_PID = new();

    ///////////////////////////////////////////////////////

    /// <summary>
    /// 保存违规玩家列表信息
    /// </summary>
    public static List<BreakRuleInfo> BreakRuleInfo_PlayerList = new();

    /// <summary>
    /// 缓存玩家生涯数据
    /// </summary>
    public static List<LifePlayerData> LifePlayerCacheDatas = new();

    /// <summary>
    /// 踢出玩家CD缓存
    /// </summary>
    public static List<KickCoolDownInfo> KickCoolDownInfos = new();

    ///////////////////////////////////////////////////////

    /// <summary>
    /// 服务器规则 队伍1
    /// </summary>
    public static ServerRule ServerRule_Team1 = new();
    /// <summary>
    /// 服务器规则 队伍2
    /// </summary>
    public static ServerRule ServerRule_Team2 = new();

    /// <summary>
    /// 保存队伍1限制武器名称列表
    /// </summary>
    public static List<string> CustomWeapons_Team1 = new();
    /// <summary>
    /// 保存队伍2限制武器名称列表
    /// </summary>
    public static List<string> CustomWeapons_Team2 = new();

    /// <summary>
    /// 自定义白名单玩家列表
    /// </summary>
    public static List<string> CustomWhites_Name = new();
    /// <summary>
    /// 自定义黑名单玩家列表
    /// </summary>
    public static List<string> CustomBlacks_Name = new();

    ///////////////////////////////////////////////////////

    /// <summary>
    /// 是否设置规则正确
    /// </summary>
    public static bool IsSetRuleOK = false;

    /// <summary>
    /// 是否自动踢出违规玩家
    /// </summary>
    public static bool AutoKickBreakRulePlayer = false;

    /// <summary>
    /// 是否自动踢出观战玩家
    /// </summary>
    public static bool IsAutoKickSpectator = false;

    /// <summary>
    /// 是否启用踢人冷却
    /// </summary>
    public static bool IsEnableKickCoolDown = false;

    /// <summary>
    /// 是否启用踢出非白名单玩家
    /// </summary>
    public static bool IsEnableKickNoWhites = false;
    /// <summary>
    /// 云端服务器是否存活
    /// </summary>
    public static bool IsCloudAlive = false;

    /// <summary>
    /// 是否允许换边
    /// </summary>
    public static bool IsNotAllowToggle = false;

    /// <summary>
    /// 是否换边踢人模式
    /// </summary>
    public static bool ToggleKickMode = false;

    public static bool Allow2LowScoreTeam = false;

    /// <summary>
    /// 是否允许超出胜率限制
    /// </summary>
    public static bool WhiteLifeMaxWR = false;
    /// <summary>
    /// 允许队伍1临时换边玩家列表
    /// </summary>
    public static List<long> AllowTempAloowToggleTeamList1 = new();

    /// <summary>
    /// 允许队伍2临时换边玩家列表
    /// </summary>
    public static List<long> AllowTempAloowToggleTeamList2 = new();

    /// <summary>
    /// 队伍1已经临时换边玩家列表
    /// </summary>
    public static List<long> TempToggleTeamList = new();

    /// <summary>
    /// 队伍2已换边临时玩家列表
    /// </summary>

    /// <summary>
    /// 允许白名单换边
    /// </summary>
    public static bool IsAllowWhlistToggleTeam = false;

    /// <summary>
    /// 倒序输出日志
    /// </summary>
    public static bool ReverseOrder= true;

    /// <summary>
    /// 使用云功能
    /// </summary>
    public static bool IsCloudMode = false;

    /// <summary>
    /// 团队最大分数
    /// </summary>
    public static int TeamMaxScore = 0;
    /// <summary>
    /// 队伍1分数
    /// </summary>
    public static int Team1Score = 0;
    /// <summary>
    /// 队伍2分数
    /// </summary>
    public static int Team2Score = 0;

    /// <summary>
    /// 系统自动平衡
    /// </summary>
    public static bool SystemAutoBalance = false;
    /// <summary>
    /// 队伍1玩家数量
    /// </summary>
    public static int Team1PlayerCount = 0;
    /// <summary>
    /// 队伍2玩家数量
    /// </summary>
    public static int Team2PlayerCount = 0;

    /// <summary>
    /// 当前地图名称
    /// </summary>
    public static string CurrentMapName = string.Empty;

    /// <summary>
    /// 当前地图模式
    /// </summary>
    public static string CurrentMapMode = string.Empty;

    /// <summary>
    /// 服务器地图列表
    /// </summary>
    public static List<string> ServerMapList = new();

    /// <summary>
    /// 玩家是否已经换过边
    /// </summary>
    public static List<long> AlreadyToggleTeamPlayer = new();

    /// <summary>
    /// 是否启用先换后踢
    /// </summary>
    public static bool ToggleTeambeforeKick = false;

    /// <summary>
    /// 规则是否已经设置
    /// </summary>
    public static bool ISetRule = false;

    public static bool CloudModeSet = false;

    public static bool OffileModeSet = false;

    public static long TempGameId = 0;

    public static int previousSelectedIndex = -1;

    public static bool IsBoot = false;

    public static string Version = "3.0.2.4";

    public static string ServerName = string.Empty;

    public static bool NeedUpdate = false;

    public static bool AllowAutoChangeMap = true;

    public static List<string> Team1WaitList = new();

    public static List<string> Team2WaitList = new();

    public static int Team1Count = 0;

    public static int Team2Count = 0;

    public static bool IsRefreshRule = false;
    /*    /// <summary>
        /// 使用云功能
        /// </summary>
        public static bool IsCloudMode = false;*/

    /*    /// <summary>
        /// 使用云功能
        /// </summary>
        public static bool IsCloudMode = false;

        /// <summary>
        /// 使用云功能
        /// </summary>
        public static bool IsCloudMode = false;*/

    ///////////////////////////////////////////////////////

    public static bool WhiteLifeKD = true;
    public static bool WhiteLifeKPM = true;
    public static bool WhiteLifeWeaponStar = true;
    public static bool WhiteLifeVehicleStar = true;
    public static bool WhiteKill = true;
    public static bool WhiteKD = true;
    public static bool WhiteKPM = true;
    public static bool WhiteRank = true;
    public static bool WhiteWeapon = true;
    public static bool WhiteLifeMaxAccuracyRatio  = true;
    public static bool WhiteLifeMaxHeadShotRatio = true;

}

public enum OrderBy
{
    Score,
    Rank,
    Clan,
    Name,
    SquadId,
    Kill,
    Dead,
    KD,
    KPM,
    LKD,
    LKPM,
    LTime,
    WR,
    Kit3,
    Weapon
}

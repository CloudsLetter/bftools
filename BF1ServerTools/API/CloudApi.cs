using BF1ServerTools.API.Resp;
using BF1ServerTools.API;
using RestSharp;
using Newtonsoft.Json.Linq;
using BF1ServerTools.SDK.Data;
using Newtonsoft.Json;
using System.Text;
using static BF1ServerTools.API.Requ.UpdateServer;

public class CloudRespError
{
    public string Id { get; set; }
    public string Error { get; set; }

}


public static class CloudApi
{

    private const string hostpg = "https://bf1.cloudyun.xyz/ping";

    private const string hostaq = "https://bf1.cloudyun.xyz/api/bf1/autotoggleteam/query";
    private const string hostad = "https://bf1.cloudyun.xyz/api/bf1/autotoggleteam/add";
    private const string hostar = "https://bf1.cloudyun.xyz/api/bf1/autotoggleteam/remove";

    private const string hostbq = "https://bf1.cloudyun.xyz/api/bf1/blacklist/query";
    private const string hostba = "https://bf1.cloudyun.xyz/api/bf1/blacklist/add";
    private const string hostbr = "https://bf1.cloudyun.xyz/api/bf1/blacklist/remove";

    private const string kickhistoryad = "https://bf1.cloudyun.xyz/api/bf1/kickhistory/add";

    private const string toggglehistoryad = "https://bf1.cloudyun.xyz/api/bf1/togglehistory/add";

    private const string playgamingdatad = "https://bf1.cloudyun.xyz/api/bf1/playergamingdata/add";

    private const string ruleqr = "https://bf1.cloudyun.xyz/api/bf1/rule/query";
    private const string rulead = "https://bf1.cloudyun.xyz/api/bf1/rule/push";

    private const string hostbkad = "https://bf1.cloudyun.xyz/api/bf1/toggleteambeforekick/add";
    private const string hostbkqr = "https://bf1.cloudyun.xyz/api/bf1/toggleteambeforekick/query";
    private const string hostbkre = "https://bf1.cloudyun.xyz/api/bf1/toggleteambeforekick/remove";
    private const string hostbkra = "https://bf1.cloudyun.xyz/api/bf1/toggleteambeforekick/removeall";

    private const string hostcv = "https://bf1.cloudyun.xyz/api/bf1/checkversion";

    private const string hostcb = "https://ea-api.2788.pro/jsonrpc/pc/api";

    private static readonly RestClient clientpg;

    private static readonly RestClient clientaq;
    private static readonly RestClient clientad;
    private static readonly RestClient clientar;

    private static readonly RestClient clientbq;
    private static readonly RestClient clientba;
    private static readonly RestClient clientbr;

    private static readonly RestClient clientkickhistoryad;

    private static readonly RestClient clienttoggglehistoryad;

    private static readonly RestClient clientplaygamingdatad;

    private static readonly RestClient clientruleqr;
    private static readonly RestClient clientrulead;

    private static readonly RestClient clientbkad;
    private static readonly RestClient clientbkqr;
    private static readonly RestClient clientbkre;
    private static readonly RestClient clientbkra;

    private static readonly RestClient clientcv;
    private static readonly RestClient clientcb;
    static CloudApi()
    {
        if (clientpg == null)
        {
            var options = new RestClientOptions(hostpg)
            {
                MaxTimeout = 5000,
                ThrowOnAnyError = false
            };
            clientpg = new RestClient(options);
        }


        if (clientaq == null)
        {
            var options = new RestClientOptions(hostaq)
            {
                MaxTimeout = 5000,
                ThrowOnAnyError = false
            };
            clientaq = new RestClient(options);
        }

        if (clientad == null)
        {
            var optionss = new RestClientOptions(hostad)
            {
                MaxTimeout = 5000,
                ThrowOnAnyError = false
            };
            clientad = new RestClient(optionss);
        }

        if (clientar == null)
        {
            var optionsss = new RestClientOptions(hostar)
            {
                MaxTimeout = 5000,
                ThrowOnAnyError = false
            };
            clientar = new RestClient(optionsss);
        }


        if (clientbq == null)
        {
            var options = new RestClientOptions(hostbq)
            {
                MaxTimeout = 5000,
                ThrowOnAnyError = true
            };
            clientbq = new RestClient(options);
        }

        if (clientba == null)
        {
            var optionss = new RestClientOptions(hostba)
            {
                MaxTimeout = 5000,
                ThrowOnAnyError = true
            };
            clientba = new RestClient(optionss);
        }

        if (clientbr == null)
        {
            var optionsss = new RestClientOptions(hostbr)
            {
                MaxTimeout = 5000,
                ThrowOnAnyError = true
            };
            clientbr = new RestClient(optionsss);
        }

        if (clientkickhistoryad == null)
        {
            var optionsss = new RestClientOptions(kickhistoryad)
            {
                MaxTimeout = 5000,
                ThrowOnAnyError = true
            };
            clientkickhistoryad = new RestClient(optionsss);
        }
        if (clienttoggglehistoryad == null)
        {
            var optionsss = new RestClientOptions(toggglehistoryad)
            {
                MaxTimeout = 5000,
                ThrowOnAnyError = true
            };
            clienttoggglehistoryad = new RestClient(optionsss);
        }

        if (clientplaygamingdatad == null)
        {
            var optionsss = new RestClientOptions(playgamingdatad)
            {
                MaxTimeout = 5000,
                ThrowOnAnyError = true
            };
            clientplaygamingdatad = new RestClient(optionsss);
        }


        if (clientruleqr == null)
        {
            var optionsss = new RestClientOptions(ruleqr)
            {
                MaxTimeout = 5000,
                ThrowOnAnyError = true
            };
            clientruleqr = new RestClient(optionsss);
        }

        if (clientrulead == null)
        {
            var optionsss = new RestClientOptions(rulead)
            {
                MaxTimeout = 5000,
                ThrowOnAnyError = true
            };
            clientrulead = new RestClient(optionsss);
        }

        if (clientbkad == null)
        {
            var optionsss = new RestClientOptions(hostbkad)
            {
                MaxTimeout = 5000,
                ThrowOnAnyError = true
            };
            clientbkad = new RestClient(optionsss);
        }

        if (clientbkqr == null)
        {
            var optionsss = new RestClientOptions(hostbkqr)
            {
                MaxTimeout = 5000,
                ThrowOnAnyError = true
            };
            clientbkqr = new RestClient(optionsss);
        }


        if (clientbkre == null)
        {
            var optionsss = new RestClientOptions(hostbkre)
            {
                MaxTimeout = 5000,
                ThrowOnAnyError = true
            };
            clientbkre = new RestClient(optionsss);
        }

        if (clientbkra == null)
        {
            var optionsss = new RestClientOptions(hostbkra)
            {
                MaxTimeout = 5000,
                ThrowOnAnyError = true
            };
            clientbkra = new RestClient(optionsss);
        }

        if (clientcv == null)
        {
            var optionsss = new RestClientOptions(hostcv)
            {
                MaxTimeout = 5000,
                ThrowOnAnyError = true
            };
            clientcv = new RestClient(optionsss);
        }

        if (clientcb == null)
        {
            var optionssss = new RestClientOptions(hostcb)
            {
                MaxTimeout = 5000,
                ThrowOnAnyError = true
            };
            clientcv = new RestClient(optionssss);
        }
    }




    public static async Task<RespContent> QueryAutoToggleTeamList(string PersonaId)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {
            var reqBody = new
            {
                Token = "chaoshilisaohuo",
                PersonaId = PersonaId,
            };

            var request = new RestRequest()
                .AddJsonBody(reqBody);

            var response = await clientaq.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                // 处理 HTTP 500 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // 处理 HTTP 400 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else
            {
                // 处理其他状态码
                respContent.IsSuccess = false;
                respContent.Content = response.Content; 
            }
        }
        catch (Exception ex)
        {
            respContent.IsSuccess = false;
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }


    public static async Task<RespContent> AddAutoToggleTeamList(string PersonaId)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {
            var reqBody = new
            {
                Token = "chaoshilisaohuo",
                PersonaId = PersonaId,
            };

            var request = new RestRequest()
                .AddJsonBody(reqBody);

            var response = await clientad.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                // 处理 HTTP 500 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // 处理 HTTP 400 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else
            {
                // 处理其他状态码
                respContent.IsSuccess = false;
                respContent.Content = response.Content; 
            }
        }
        catch (Exception ex)
        {
            respContent.IsSuccess = false;
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }





    public static async Task<RespContent> RemoveAutoToggleTeamList(string PersonaId)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {
            var reqBody = new
            {
                Token = "chaoshilisaohuo",
                PersonaId = PersonaId,
            };

            var request = new RestRequest()
                .AddJsonBody(reqBody);

            var response = await clientar.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                // 处理 HTTP 500 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // 处理 HTTP 400 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else
            {
                // 处理其他状态码
                respContent.IsSuccess = false;
                respContent.Content = response.Content; 
            }
        }
        catch (Exception ex)
        {
            respContent.IsSuccess = false;
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }


    /// <summary>
    /// 联网获取黑名单
    /// </summary>
    /// <param name="serverToken"></param>
    /// <param name="queryUri"></param>
    /// <returns></returns>
    public static async Task<RespContent> RefreshBlackList(string ServerId)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {
            var reqBody = new
            {
                Token = "chaoshilisaohuo",
                ServerId = ServerId
            };

            var request = new RestRequest()
                .AddJsonBody(reqBody);

            var response = await clientbq.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else
            {
                var respError = JsonHelper.JsonDese<RespError>(response.Content);
                respContent.Content = $"{respError.error.code} {respError.error.message}";
            }
        }
        catch (Exception ex)
        {
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }

    /// <summary>
    /// 联网增加黑名单
    /// </summary>
    /// <param name="serverToken"></param>
    /// <returns></returns>
    public static async Task<RespContent> AddBlackList(string ServerId, string Gameid, string Guid, string PlayerName, string ServerName)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {
            var reqBody = new
            {
                Token = "chaoshilisaohuo",
                ServerId = ServerId,
                PlayerName = PlayerName,
                Guid = Guid,
                Gameid = Gameid,
                ServerName = ServerName
            };

            var request = new RestRequest()
                .AddJsonBody(reqBody);

            var response = await clientba.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else
            {
                var respError = JsonHelper.JsonDese<RespError>(response.Content);
                respContent.Content = $"{respError.error.code} {respError.error.message}";
            }
        }
        catch (Exception ex)
        {
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }

    /// <summary>
    /// 联网删除黑名单
    /// </summary>
    /// <param name="serverToken"></param>
    /// <returns></returns>
    public static async Task<RespContent> RemoveBlackList(string ServerId, string PlayerName)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {
            var reqBody = new
            {
                Token = "chaoshilisaohuo",
                ServerId = ServerId,
                PlayerName = PlayerName
            };

            var request = new RestRequest()
                .AddJsonBody(reqBody);

            var response = await clientbr.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else
            {
                var respError = JsonHelper.JsonDese<RespError>(response.Content);
                respContent.Content = $"{respError.error.code} {respError.error.message}";
            }
        }
        catch (Exception ex)
        {
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }




    /// <summary>
    /// 上传踢出记录到服务器
    /// </summary>
    /// <param name="serverToken"></param>
    /// <returns></returns>
    public static async Task<RespContent> KickHIstory(string Operator, string KickedOutPlayerRank, string KickedOutPlayerName, string KickedOutPersonaId, string Reason, string State, string ServerId, string Guid, string GameId, string Type,string ServerName)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {
            var reqBody = new
            {
                Token = "chaoshilisaohuo",
                Operator = Operator,
                KickedOutPlayerRank = KickedOutPlayerRank,
                KickedOutPlayerName = KickedOutPlayerName,
                KickedOutPersonaId = KickedOutPersonaId,
                Reason = Reason,
                State = State,
                ServerId = ServerId,
                Guid = Guid,
                GameId = GameId,
                Type = Type,
                ServerName = ServerName
            };

            var request = new RestRequest()
                .AddJsonBody(reqBody);

            var response = await clientkickhistoryad.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else
            {
                var respError = JsonHelper.JsonDese<RespError>(response.Content);
                respContent.Content = $"{respError.error.code} {respError.error.message}";
            }
        }
        catch (Exception ex)
        {
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }


    /// <summary>
    /// 联网推送换边日志
    /// </summary>
    /// <param name="serverToken"></param>
    /// <returns></returns>
    public static async Task<RespContent> PushToggleHistory(string PlayerRank,string PlayerName, string PersonaId,string GameMode, string MapName, string Team1Name,string Team2Name,string State, string ServerId, string Guid, string GameId, string ServerName)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {
            var reqBody = new
            {
                Token = "chaoshilisaohuo",
                PlayerRank = PlayerRank,
                PlayerName = PlayerName,
                PersonaId = PersonaId,
                GameMode = GameMode,
                MapName = MapName,
                Team1Name = Team1Name,
                Team2Name = Team2Name,
                State = State,
                ServerId = ServerId,
                Guid = Guid,
                GameId = GameId,
                ServerName = ServerName
            };

            var request = new RestRequest()
                .AddJsonBody(reqBody);

            var response = await clienttoggglehistoryad.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else
            {
                var respError = JsonHelper.JsonDese<RespError>(response.Content);
                respContent.Content = $"{respError.error.code} {respError.error.message}";
            }
        }
        catch (Exception ex)
        {
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }

    public static async Task<RespContent> PushPlayGamingData(List<PlayerData> Data)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {

            var reqBody = new
            {
                Data
            };

            var request = new RestRequest()
                .AddJsonBody(reqBody)
                .AddHeader("Token", "chaoshilisaohuo");

            var response = await clientplaygamingdatad.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else
            {
                var respError = JsonHelper.JsonDese<RespError>(response.Content);
                respContent.Content = $"{respError.error.code} {respError.error.message}";
            }
        }
        catch (Exception ex)
        {
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }


    public static async Task<RespContent> PushRule(
        bool whiteLifeKD,
        bool whiteLifeKPM,
        bool whiteLifeWeaponStar,
        bool whiteLifeVehicleStar,
        bool whiteKill,
        bool whiteKD,
        bool whiteKPM,
        bool whiteRank,
        bool whiteWeapon,
        bool whiteToggleTeam,
        bool whiteLifeMaxAccuracyRatio,
        bool whiteLifeMaxHeadShotRatio,
        bool whiteLifeMaxWR,
        bool whiteAllowToggleTeam,
        bool allow2LowScoreTeam,
        bool whiteScore,
        int team1MaxKill,
        int team1FlagKD,
        float team1MaxKD,
        int team1FlagKPM,
        float team1MaxKPM,
        int team1MinRank,
        int team1MaxRank,
        float team1LifeMaxKD,
        float team1LifeMaxKPM,
        int team1LifeMaxAccuracyRatioLevel,
        float team1LifeMaxAccuracyRatio,
        int team1LifeMaxHeadShotRatioLevel,
        float team1LifeMaxHeadShotRatio,
        int team1LifeMaxWRLevel,
        float team1LifeMaxWR,
        int team1LifeMaxWeaponStar,
        int team1LifeMaxVehicleStar,
        int team1MaxScore,
        int team1FlagKDPro,
        float team1MaxKDPro,
        int team1FlagKPMPro,
        float team1MaxKPMPro,
        int team2MaxKill,
        int team2FlagKD,
        float team2MaxKD,
        int team2FlagKPM,
        float team2MaxKPM,
        int team2MinRank,
        int team2MaxRank,
        float team2LifeMaxKD,
        float team2LifeMaxKPM,
        int team2LifeMaxAccuracyRatioLevel,
        float team2LifeMaxAccuracyRatio,
        int team2LifeMaxHeadShotRatioLevel,
        float team2LifeMaxHeadShotRatio,
        int team2LifeMaxWRLevel,
        float team2LifeMaxWR,
        int team2LifeMaxWeaponStar,
        int team2LifeMaxVehicleStar,
        long serverId,
        long gameId,
        string guid,
        long operatorPersonaId,
        string team1Weapon,
        string team2Weapon,
        int team1ScoreLimit,
        int team1ScoreGap,
        string serverName,
        int team2MaxScore,
        int team2FlagKDPro,
        float team2MaxKDPro,
        int team2FlagKPMPro,
        float team2MaxKPMPro
        )
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {

            var reqBody = new
            {
               WhiteLifeKD = whiteLifeKD,
               WhiteLifeKPM = whiteLifeKPM,
               WhiteLifeWeaponStar = whiteLifeWeaponStar,
               WhiteLifeVehicleStar = whiteLifeVehicleStar,
               WhiteKill = whiteKill,
               WhiteKD = whiteKD,
               WhiteKPM = whiteKPM,
               WhiteRank = whiteRank,
               WhiteWeapon = whiteWeapon,
               WhiteToggleTeam = whiteToggleTeam,
               WhiteLifeMaxAccuracyRatio = whiteLifeMaxAccuracyRatio,
               WhiteLifeMaxHeadShotRatio = whiteLifeMaxHeadShotRatio,
               WhiteLifeMaxWR = whiteLifeMaxWR,
               WhiteAllowToggleTeam =  whiteAllowToggleTeam,
               WhiteScore = whiteScore,
               Team1MaxKill = team1MaxKill,
               Team1FlagKD = team1FlagKD,
               Team1MaxKD = team1MaxKD,
               Team1FlagKPM = team1FlagKPM,
               Team1MaxKPM = team1MaxKPM,
               Team1MinRank = team1MinRank,
               Team1MaxRank = team1MaxRank,
               Team1LifeMaxKD = team1LifeMaxKD,
               Team1LifeMaxKPM = team1LifeMaxKPM,
               Team1LifeMaxAccuracyRatioLevel = team1LifeMaxAccuracyRatioLevel,
               Team1LifeMaxAccuracyRatio = team1LifeMaxAccuracyRatio,
               Team1LifeMaxHeadShotRatioLevel = team1LifeMaxHeadShotRatioLevel,
               Team1LifeMaxHeadShotRatio = team1LifeMaxHeadShotRatio,
               Team1LifeMaxWRLevel = team1LifeMaxWRLevel,
               Team1LifeMaxWR = team1LifeMaxWR,
               Team1LifeMaxWeaponStar = team1LifeMaxWeaponStar,
               Team1LifeMaxVehicleStar = team1LifeMaxVehicleStar,
               Team1MaxScore = team1MaxScore,
               Team1FlagKDPro = team1FlagKDPro,
               Team1MaxKDPro = team1MaxKDPro,
               Team1FlagKPMPro = team1FlagKPMPro,
               Team1MaxKPMPro = team1MaxKPMPro,
               Team1WeaponLimit = team1Weapon,
               Team2MaxKill = team2MaxKill,
               Team2FlagKD = team2FlagKD,
               Team2MaxKD = team2MaxKD,
               Team2FlagKPM = team2FlagKPM,
               Team2MaxKPM = team2MaxKPM,
               Team2MinRank = team2MinRank,
               Team2MaxRank = team2MaxRank,
               Team2LifeMaxKD = team2LifeMaxKD,
               Team2LifeMaxKPM = team2LifeMaxKPM,
               Team2LifeMaxAccuracyRatioLevel = team2LifeMaxAccuracyRatioLevel,
               Team2LifeMaxAccuracyRatioat = team2LifeMaxAccuracyRatio,
               Team2LifeMaxHeadShotRatioLevel = team2LifeMaxHeadShotRatioLevel,
               Team2LifeMaxHeadShotRatio = team2LifeMaxHeadShotRatio,
               Team2LifeMaxWRLevel = team2LifeMaxWRLevel,
               Team2LifeMaxWR = team2LifeMaxWR,
               Team2LifeMaxWeaponStar = team2LifeMaxWeaponStar,
               Team2LifeMaxVehicleStar = team2LifeMaxVehicleStar,
               Team2MaxScore = team2MaxScore,
               Team2FlagKDPro = team2FlagKDPro,
               Team2MaxKDPro = team2MaxKDPro,
               Team2FlagKPMPro = team2FlagKPMPro,
               Team2MaxKPMPro = team2MaxKPMPro,
               Team2WeaponLimit = team2Weapon,
               ServerId = serverId,
               GameId = gameId,
               GUID = guid,
               OperatorPersonaId = operatorPersonaId,
               Team1ScoreLimit = team1ScoreLimit,
               Team1ScoreGap = team1ScoreGap,
               Allow2LowScoreTeam  = allow2LowScoreTeam,
               ServerName = serverName
            };

            var request = new RestRequest()
                .AddJsonBody(reqBody)
                .AddHeader("Token", "chaoshilisaohuo");

            var response = await clientrulead.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else
            {
                var respError = JsonHelper.JsonDese<RespError>(response.Content);
                respContent.Content = $"{respError.error.code} {respError.error.message}";
            }
        }
        catch (Exception ex)
        {
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }


    public static async Task<RespContent> QueryRule(long ServerId)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {

            var reqBody = new
            {
                Token = "chaoshilisaohuo",
                ServerId = ServerId
            };

            var request = new RestRequest()
                .AddJsonBody(reqBody)
                .AddHeader("Token", "chaoshilisaohuo");

            var response = await clientruleqr.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else
            {
                var respError = JsonHelper.JsonDese<RespError>(response.Content);
                respContent.Content = $"{respError.error.code} {respError.error.message}";
            }
        }
        catch (Exception ex)
        {
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }



    public static async Task<RespContent> CheckAlive()
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {


            var request = new RestRequest();

            var response = await clientpg.ExecuteGetAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else
            {
                var respError = JsonHelper.JsonDese<RespError>(response.Content);
                respContent.Content = $"{respError.error.code} {respError.error.message}";
            }
        }
        catch (Exception ex)
        {
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }

    public static async Task<RespContent> CheckVersion(string Version)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {

            var reqBody = new
            {
                Token = "chaoshilisaohuo",
                Version = Version
            };

            var request = new RestRequest()
                .AddJsonBody(reqBody)
                .AddHeader("Token", "chaoshilisaohuo");

            var response = await clientcv.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else
            {
                var respError = JsonHelper.JsonDese<RespError>(response.Content);
                respContent.Content = $"{respError.error.code} {respError.error.message}";
            }
        }
        catch (Exception ex)
        {
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }


    public static async Task<RespContent> QueryToggleTeambeForeKick(string gameId,string PersonaId)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {
            var reqBody = new
            {
                Token = "chaoshilisaohuo",
                GameId = gameId,
                PersonaId = PersonaId,
            };

            var request = new RestRequest()
                .AddJsonBody(reqBody);

            var response = await clientbkqr.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                // 处理 HTTP 500 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // 处理 HTTP 400 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else
            {
                // 处理其他状态码
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
        }
        catch (Exception ex)
        {
            respContent.IsSuccess = false;
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }


    public static async Task<RespContent> AddToggleTeambeForeKick(string gameId, string PersonaId)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {
            var reqBody = new
            {
                Token = "chaoshilisaohuo",
                GameId = gameId,
                PersonaId = PersonaId,
            };

            var request = new RestRequest()
                .AddJsonBody(reqBody);

            var response = await clientbkad.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                // 处理 HTTP 500 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // 处理 HTTP 400 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else
            {
                // 处理其他状态码
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
        }
        catch (Exception ex)
        {
            respContent.IsSuccess = false;
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }





    public static async Task<RespContent> RemoveToggleTeambeForeKick(string gameId, string PersonaId)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {
            var reqBody = new
            {
                Token = "chaoshilisaohuo",
                GameId = gameId,
                PersonaId = PersonaId,
            };

            var request = new RestRequest()
                .AddJsonBody(reqBody);

            var response = await clientbkra.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                // 处理 HTTP 500 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // 处理 HTTP 400 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else
            {
                // 处理其他状态码
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
        }
        catch (Exception ex)
        {
            respContent.IsSuccess = false;
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }

    public static async Task<RespContent> RemoveAllToggleTeambeForeKick(string gameId)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {
            var reqBody = new
            {
                Token = "chaoshilisaohuo",
                GameId = gameId,
            };

            var request = new RestRequest()
                .AddJsonBody(reqBody);

            var response = await clientbkra.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                // 处理 HTTP 500 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // 处理 HTTP 400 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else
            {
                // 处理其他状态码
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
        }
        catch (Exception ex)
        {
            respContent.IsSuccess = false;
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }


    public static async Task<RespContent> CloudBanList(string sessionId,int serverId)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {
            var reqBody = new
            {
                jsonrpc=  "2.0",
	            method = "CloudBan.listServerBan",
                @params = new
                {
                    game = "tunguska",
                    serverId = serverId,

                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeader("X-GatewaySession", sessionId)
                .AddJsonBody(reqBody);

            var response = await clientcb.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                // 处理 HTTP 500 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // 处理 HTTP 400 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else
            {
                // 处理其他状态码
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
        }
        catch (Exception ex)
        {
            respContent.IsSuccess = false;
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }

    public static async Task<RespContent> CloudBanAdd(string sessionId, string serverId, long personaName)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {
            var reqBody = new
            {
                jsonrpc = "2.0",
                method = "RSP.addServerBan",
                @params = new
                {
                    game = "tunguska",
                    serverId,
                    personaName
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeader("X-GatewaySession", sessionId)
                .AddJsonBody(reqBody);

            var response = await clientcb.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                // 处理 HTTP 500 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // 处理 HTTP 400 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else
            {
                // 处理其他状态码
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
        }
        catch (Exception ex)
        {
            respContent.IsSuccess = false;
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }


    public static async Task<RespContent> CloudBanRemove(string sessionId, string serverId,long personaId)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {
            var reqBody = new
            {

                jsonrpc = "2.0",
                method = "RSP.removeServerBan",
                @params = new
                {
                    game = "tunguska",
                    serverId,
                    personaId
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeader("X-GatewaySession", sessionId)
                .AddJsonBody(reqBody);

            var response = await clientcb.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                // 处理 HTTP 500 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // 处理 HTTP 400 错误
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
            else
            {
                // 处理其他状态码
                respContent.IsSuccess = false;
                respContent.Content = response.Content;
            }
        }
        catch (Exception ex)
        {
            respContent.IsSuccess = false;
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;

    }
}
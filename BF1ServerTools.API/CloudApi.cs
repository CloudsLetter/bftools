using BF1ServerTools.API.Resp;
using BF1ServerTools.API;
using RestSharp;
using Newtonsoft.Json.Linq;
using BF1ServerTools.SDK.Data;
using Newtonsoft.Json;
using System.Text;

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
    public static async Task<RespContent> AddBlackList(string ServerId, string Gameid, string Guid, string PlayerName)
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
                Gameid = Gameid
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
    public static async Task<RespContent> KickHIstory(string Operator, string KickedOutPlayerRank, string KickedOutPlayerName, string KickedOutPersonaId, string Reason, string State, string ServerId, string Guid, string GameId, string Type)
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
    /// 联网推送换边日志
    /// </summary>
    /// <param name="serverToken"></param>
    /// <returns></returns>
    public static async Task<RespContent> PushToggleHistory(string PlayerRank,string PlayerName, string PersonaId,string GameMode, string MapName, string Team1Name,string Team2Name,string State, string ServerId, string Guid, string GameId)
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
                GameId = GameId
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
        bool whiteLifeMaxWR,
        bool whiteAllowToggleTeam,
        bool allow2LowScoreTeam,
        int team1MaxKill,
        int team1FlagKD,
        double team1MaxKD,
        int team1FlagKPM,
        double team1MaxKPM,
        int team1MinRank,
        int team1MaxRank,
        double team1LifeMaxKD,
        double team1LifeMaxKPM,
        int team1LifeMaxWRLevel,
        double team1LifeMaxWR,
        int team1LifeMaxWeaponStar,
        int team1LifeMaxVehicleStar,
        int team2MaxKill,
        int team2FlagKD,
        double team2MaxKD,
        int team2FlagKPM,
        double team2MaxKPM,
        int team2MinRank,
        int team2MaxRank,
        double team2LifeMaxKD,
        double team2LifeMaxKPM,
        int team2LifeMaxWRLevel,
        double team2LifeMaxWR,
        int team2LifeMaxWeaponStar,
        int team2LifeMaxVehicleStar,
        long serverId,
        long gameId,
        string guid,
        long operatorPersonaId,
        string team1Weapon,
        string team2Weapon,
        int team1ScoreLimit,
        int team1ScoreGap
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
               WhiteLifeMaxWR = whiteLifeMaxWR,
               WhiteAllowToggleTeam =  whiteAllowToggleTeam,
               Team1MaxKill = team1MaxKill,
               Team1FlagKD = team1FlagKD,
               Team1MaxKD = team1MaxKD,
               Team1FlagKPM = team1FlagKPM,
               Team1MaxKPM = team1MaxKPM,
               Team1MinRank = team1MinRank,
               Team1MaxRank = team1MaxRank,
               Team1LifeMaxKD = team1LifeMaxKD,
               Team1LifeMaxKPM = team1LifeMaxKPM,
               Team1LifeMaxWR = team1LifeMaxWR,
               Team1LifeMaxWeaponStar = team1LifeMaxWeaponStar,
               Team1LifeMaxVehicleStar = team1LifeMaxVehicleStar,
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
               Team2LifeMaxWR = team2LifeMaxWR,
               Team2LifeMaxWeaponStar = team2LifeMaxWeaponStar,
               Team2LifeMaxVehicleStar = team2LifeMaxVehicleStar,
               Team2WeaponLimit = team2Weapon,
               ServerId = serverId,
               GameId = gameId,
               GUID = guid,
               OperatorPersonaId = operatorPersonaId,
               Team1ScoreLimit = team1ScoreLimit,
               Team1ScoreGap = team1ScoreGap,
               Team1LifeMaxWRLevel = team1LifeMaxWRLevel,
               Team2LifeMaxWRLevel = team2LifeMaxWRLevel,
               Allow2LowScoreTeam  = allow2LowScoreTeam

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


}
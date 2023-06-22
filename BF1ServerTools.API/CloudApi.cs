using BF1ServerTools.API.Resp;
using BF1ServerTools.API;
using RestSharp;
using Newtonsoft.Json.Linq;


public class CloudRespError
{
    public string Id { get; set; }
    public string Error { get; set; }

}


public static class CloudApi
{
    private const string hostaq = "http://127.0.0.1:8080/api/bf1/autotoggleteam/query";
    private const string hostad = "http://127.0.0.1:8080/api/bf1/autotoggleteam/add";
    private const string hostar = "http://127.0.0.1:8080/api/bf1/autotoggleteam/remove";
    private const string hostbq = "http://127.0.0.1:8080/api/bf1/blacklist/query";
    private const string hostba = "http://127.0.0.1:8080/api/bf1/blacklist/add";
    private const string hostbr = "http://127.0.0.1:8080/api/bf1/blacklist/remove";

    private const string kickhistoryad = "http://127.0.0.1:8080/api/bf1/kickhistory/add";

    private const string toggglehistoryad = "http://127.0.0.1:8080/api/bf1/togglehistory/add";

    private static readonly RestClient clientaq;
    private static readonly RestClient clientad;
    private static readonly RestClient clientar;

    private static readonly RestClient clientbq;
    private static readonly RestClient clientba;
    private static readonly RestClient clientbr;

    private static readonly RestClient clientkickhistoryad;

    private static readonly RestClient clienttoggglehistoryad;

    static CloudApi()
    {
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



}
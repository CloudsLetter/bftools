using BF1ServerTools.API.Resp;
using BF1ServerTools.API;
using RestSharp;


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
    private static readonly RestClient clientaq;
    private static readonly RestClient clientad;
    private static readonly RestClient clientar;

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
                respContent.Content = response.Content; ;
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
                respContent.Content = response.Content; ;
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
                respContent.Content = response.Content; ;
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
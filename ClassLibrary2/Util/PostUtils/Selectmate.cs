using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using JingZeServer.Model;
using Microsoft.SqlServer.Server;
using Kingdee.CDP.WebApi.SDK;
using System.Security.Policy;

namespace JingZeServer.Util.PostUtils
{
    public class Selectmate
    {
        public static async Task<bool> select(string number)
        {

            var client = await GetAuthenticatedClient();
            var datas = new selectjson()
            {
                formid = "BD_MATERIAL",
                data=new RequestData()
                {
                    CreateOrgId = 0,
                    Number = number,
                    IsSortBySeq = "false"
                }

            };
            var jsonContent = JsonConvert.SerializeObject(datas);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://120.79.70.11:8888/k3cloud/Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.View.common.kdsvc"),
                Headers =
                {
                    { "Accept", "*/*" },
                    { "User-Agent", "PostmanRuntime-ApipostRuntime/1.1.0" },
                    { "Connection", "keep-alive" },
                    { "Cookie", "ASP.NET_SessionId=1lmv05nkp4gle43kaqhsw3rn;kdservice-sessionid=9c7c3c7f-7a05-4445-8c6a-08cf1c3cb6a8" },
                },
                Content = new StringContent(jsonContent, Encoding.UTF8)
                {
                    Headers =
                    {

                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };
            //await loginAsync();
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                dynamic responses = JsonConvert.DeserializeObject(body);

                bool isSuccess = responses.Result.ResponseStatus.IsSuccess;
                int bq = responses.Result.Result.F_pinb_Qty_qtr;
                int sz = responses.Result.Result.F_pinb_Qty_qtr_83g;
                int qbq = responses.Result.Result.F_pinb_Qty_qtr_83g_re5;
                int qbh = responses.Result.Result.F_pinb_Qty_qtr_83g_apv;
                int gc = responses.Result.Result.F_pinb_Qty_qtr_83g_tzk;
                
                return isSuccess;

            }

        }
        public async static Task<HttpClient> GetAuthenticatedClient()
        {
            // 基础配置
            string baseUrl = "http://120.79.70.11:8888";
            string loginUrl = $"{baseUrl}/K3Cloud/Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUser.common.kdsvc";

            // 登录参数
            var loginData = new
            {
                username = "test001",
                password = "test.2024",
                acctid = "65857d5a93d202",
                lcid = 2052 // 中文
            };

            var jsonContent = JsonConvert.SerializeObject(loginData);

            // 创建HttpClient
            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(baseUrl)
            };

            // 发送登录请求
            var response = await httpClient.PostAsync(loginUrl,
                new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"登录失败: {response.StatusCode}");
            }

            // 检查登录结果
            var result = await response.Content.ReadAsStringAsync();
            if (result.Contains("\"LoginResultType\":1")) // 1表示成功
            {
                return httpClient; // 返回已认证的客户端
            }
            else
            {
                throw new Exception($"登录失败: {result}");
            }
        }
        public static async Task loginAsync()
        {
            var clientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            };
            var client = new HttpClient(clientHandler);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://120.79.70.11:8888/K3Cloud/Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUser.common.kdsvc"),
                Headers =
    {
        { "Accept", "*/*" },
        { "User-Agent", "PostmanRuntime-ApipostRuntime/1.1.0" },
        { "Connection", "keep-alive" },
        { "Cookie", "ASP.NET_SessionId=1lmv05nkp4gle43kaqhsw3rn;kdservice-sessionid=a52e63a7-6b7a-4d12-93c6-7d4f7fd7fe28" },
    },
                Content = new MultipartFormDataContent
    {
        new StringContent("65857d5a93d202")
        {
            Headers =
            {
                ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "acctID",
                }
            }
        },
        new StringContent("test001")
        {
            Headers =
            {
                ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "username",
                }
            }
        },
        new StringContent("test.2024")
        {
            Headers =
            {
                ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "password",
                }
            }
        },
        new StringContent("2052")
        {
            Headers =
            {
                ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "lcid",
                }
            }
        },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
            }
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;

namespace WebApplication1.Controllers
{
    [RoutePrefix("api/Views")]
    public class ViewsController : ApiController
    {
        //private string connectionString2 = "data source=127.0.0.1;initial catalog=JZZS;persist security info=True;user id=sa;password=telenadmin99;";
        private string connectionString2 = "data source=(localdb)\\MSSQLLocalDB;initial catalog=JZZS;persist security info=True;user id=sa;password=123456;";

        string query2 = @"SELECT * from cp ";
        string pass = @"SELECT * from pass ";
        string capacity = @"SELECT * from capacity ";

        [HttpGet]
        [Route("CP")]
        public HttpResponseMessage Index()
        {
            DataTable dt2 = new DataTable();

            try
            {
                using (SqlConnection connection2 = new SqlConnection(connectionString2))
                {
                    connection2.Open();
                    using (SqlCommand command2 = new SqlCommand(query2, connection2))
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(dt2);
                    }
                }

                // 转换 DataTable 为 JSON 格式
                string jsonResult = JsonConvert.SerializeObject(dt2, Formatting.Indented);

                // 清空 DataTable 数据（保留表结构）
                dt2.Clear();

                // 返回 JSON 响应
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonResult, System.Text.Encoding.UTF8, "application/json")
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"查询出错: {ex.Message}");

                // 返回错误信息
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"服务器错误: {ex.Message}")
                };
            }
        }

        [HttpGet]
        [Route("Pass")]
        public HttpResponseMessage checkpass()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection2 = new SqlConnection(connectionString2))
                {
                    connection2.Open();
                    using (SqlCommand command2 = new SqlCommand(pass, connection2))
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(dt);
                    }
                }

                // 转换 DataTable 为 JSON 格式
                string jsonResult = JsonConvert.SerializeObject(dt, Formatting.Indented);

                // 清空 DataTable 数据（保留表结构）
                dt.Clear();

                // 返回 JSON 响应
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonResult, System.Text.Encoding.UTF8, "application/json")
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"查询出错: {ex.Message}");

                // 返回错误信息
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"服务器错误: {ex.Message}")
                };
            }
        }

        [HttpGet]
        [Route("Capacity")]
        public HttpResponseMessage checkcapacity()
        {
            DataTable dt3 = new DataTable();

            try
            {
                using (SqlConnection connection2 = new SqlConnection(connectionString2))
                {
                    connection2.Open();
                    using (SqlCommand command2 = new SqlCommand(capacity, connection2))
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(dt3);
                    }
                }

                // 转换 DataTable 为 JSON 格式
                string jsonResult = JsonConvert.SerializeObject(dt3, Formatting.Indented);

                // 清空 DataTable 数据（保留表结构）
                dt3.Clear();

                // 返回 JSON 响应
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonResult, System.Text.Encoding.UTF8, "application/json")
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"查询出错: {ex.Message}");

                // 返回错误信息
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"服务器错误: {ex.Message}")
                };
            }
        }

    }
}

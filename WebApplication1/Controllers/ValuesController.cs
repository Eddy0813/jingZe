using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
using JingZeServer;
using JingZeServer.Model;
using JingZeServer.Util;
using JingZeUtil;
using Newtonsoft.Json;


namespace WebApplication1.Controllers
{
    [RoutePrefix("api/Values")]
    public class ValuesController : ApiController
    {
        /// <summary>
        /// 获取标签及重量信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("RFID")]
        public HttpResponseMessage GetRFID()
        {
            //string singleRecord = Class1.GetRFIDIDA();
            string singleRecord = "E2806995000040020154ED7D";

            if (singleRecord != null)
            {
                using (var context = new JZZSEntities1())
                {
                    var proweigth = context.ProWeigth.Where(x => x.RFIDID == singleRecord).OrderByDescending(x => x.DateTime).FirstOrDefault();
                    if (proweigth == null)
                    {
                        proweigth = new ProWeigth()
                        {
                            RFIDID = singleRecord,
                        };
                    }
                    var jsonString = JsonConvert.SerializeObject(proweigth);
                    return JZZSServer.ToJson(true, jsonString);
                }
            }
            else
            {
                return JZZSServer.ToJson(false, "获取标签失败");
            }
        }

        /// <summary>
        /// 皮重接口，查询物料码和标准
        /// </summary>
        /// <param name="pG"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PG")]
        // GET api/values
        public HttpResponseMessage Get([FromBody] PG pG)
        {
            DbBase<GZID> dbgz = new DbBase<GZID>();

            DbBase<material> dbma = new DbBase<material>();


            string rfidid = JZZSServer.PGinsert(pG, dbma, dbgz);
            if (rfidid != "")
            {
                string MateCode = dbgz.FirstOrDefault(x => x.RFIDID == rfidid).MateCode;
                var Mata = dbma.FirstOrDefault(x => x.number == MateCode);
                if (MateCode != null)
                {
                    var jsonString = JsonConvert.SerializeObject(new
                    {
                        qbh = Mata.qbh,
                        MateCode = MateCode
                    });

                    var test = JZZSServer.ToJson(true, jsonString);
                    return test;
                }
                else
                {
                    var test = JZZSServer.ToJson(false, "该材料编码不存在");
                    return test;
                }
            }
            else
            {
                var test = JZZSServer.ToJson(false, "获取标签失败");
                return test;
            }

        }

        /// <summary>
        /// 毛重，返回公差
        /// </summary>
        /// <param name="mG"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("MG")]
        // GET api/values/5
        public HttpResponseMessage GetMG([FromBody] PG mG)
        {
            DbBase<GZID> dbgz = new DbBase<GZID>();
            DbBase<material> dbma = new DbBase<material>();
            DbBase<ProWeigth> dbpro = new DbBase<ProWeigth>();
            var (glValue, rfid) = JZZSServer.PGUpdate(mG, dbma, dbgz);
            if (rfid != null)
            {
                string MateCode = dbgz.FirstOrDefault(x => x.RFIDID == rfid).MateCode;
                using (var context = new JZZSEntities1())
                {
                    var proweigth = context.ProWeigth.Where(x => x.RFIDID == rfid).OrderByDescending(x => x.DateTime).FirstOrDefault();
                    var Mata = dbma.FirstOrDefault(x => x.number == MateCode);
                    if (MateCode != null)
                    {
                        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                        double? mpg = Mata.qbq;
                        double? tol = Mata.gcRange;
                        if (mpg.HasValue && tol.HasValue)
                        {
                            double? lowerBound = mpg - tol;
                            double? upperBound = mpg + tol;

                            if (glValue >= lowerBound && glValue <= upperBound)
                            {
                                proweigth.Reserver1 = "合格";
                                context.SaveChanges();
                                var jsonString = JsonConvert.SerializeObject(new
                                {
                                    OrderNo = proweigth.OrderNo,
                                    OrderNum = proweigth.OrderNum,
                                    ProID = (timestamp % 100000000).ToString("D8"),
                                    ProName = proweigth.ProName,
                                    result = "glValue 在 mpg 的上下 tol 范围内"
                                });
                                var test = JZZSServer.ToJson(true, jsonString);
                                return test;
                            }
                            else if (glValue <= lowerBound)
                            {
                                proweigth.Reserver1 = "偏轻";
                                context.SaveChanges();
                                var test = JZZSServer.ToJson(true, "glValue 低于下限");
                                return test;
                                // 在这里可以处理 glValue 不在范围内的逻辑
                            }
                            else if (glValue >= upperBound)
                            {
                                proweigth.Reserver1 = "偏重";
                                context.SaveChanges();
                                var test = JZZSServer.ToJson(true, "glValue 高于上限");
                                return test;
                                // 在这里可以处理 glValue 不在范围内的逻辑
                            }
                            else
                            {
                                var test = JZZSServer.ToJson(false, "该材料编码不存在");
                                return test;
                            }
                        }
                        else
                        {

                            var test = JZZSServer.ToJson(false, "不存在该记录");
                            return test;
                            // 在这里可以处理 glValue 不在范围内的逻辑

                        }
                    }
                    else
                    {
                        var test = JZZSServer.ToJson(false, "该材料编码不存在");
                        return test;
                    }
                }

            }
            else
            {
                var test = JZZSServer.ToJson(false, "获取标签失败");
                return test;
            }

        }

        /// <summary>
        /// 生产详情更新
        /// </summary>
        /// <param name="mG"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ID")]
        // POST api/values
        public HttpResponseMessage Post([FromBody] PG mG)
        {
            DbBase<GZID> dbgz = new DbBase<GZID>();
            DbBase<material> dbma = new DbBase<material>();

            var rfid = JZZSServer.MataUpdate(mG, dbma, dbgz);
            string MateCode = dbgz.FirstOrDefault(x => x.RFIDID == rfid).MateCode;

            var Mata = dbma.FirstOrDefault(x => x.number == MateCode);
            if (MateCode != null)
            {
                var jsonString = JsonConvert.SerializeObject(new
                {
                    qbh = Mata.qbh,
                    MateCode = MateCode
                });

                var test = JZZSServer.ToJson(true, jsonString);
                return test;
            }
            else
            {
                var test = JZZSServer.ToJson(false, "该材料编码不存在");
                return test;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="insert"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("insertRFID")]
        public HttpResponseMessage insertrfid([FromBody] Insert insert)
        {
            try
            {
                using (var db = new JZZSEntities1())
                {
                    // 查找是否已存在相同RFIDID的记录
                    var existingRecord = db.GZID.FirstOrDefault(q => q.RFIDID == insert.RFIDID);

                    if (existingRecord == null)
                    {
                        // 插入新记录（不设置ID，让数据库自动生成）
                        GZID newRecord = new GZID()
                        {
                            RFIDID = insert.RFIDID,
                            MateCode = insert.MateCode,
                            DateTime = DateTime.Now
                        };

                        db.GZID.Add(newRecord);
                        db.SaveChanges();
                        return JZZSServer.ToJson(true, "RFID绑定完成");
                    }
                    else
                    {
                        // 更新现有记录
                        existingRecord.MateCode = insert.MateCode;
                        existingRecord.DateTime = DateTime.Now;

                        db.GZID.AddOrUpdate(existingRecord);
                        db.SaveChanges();
                        return JZZSServer.ToJson(true, "RFID信息更新完成");
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                // 处理数据库更新异常
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627) // 主键冲突错误代码
                {
                    return JZZSServer.ToJson(false, "数据冲突，请重试或联系管理员");
                }
                return JZZSServer.ToJson(false, $"数据库操作失败: {ex.Message}");
            }
            catch (Exception ex)
            {
                // 处理其他异常
                return JZZSServer.ToJson(false, $"操作失败: {ex.Message}");
            }

        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Mate")]
        public HttpResponseMessage selectmate()
        {
            DbBase<material> db = new DbBase<material>();

            var mater = db.GetAll();

            var test = JZZSServer.ToJson(true, mater);
            return test;
        }

    }
}

using JingZeServer;
using JingZeServer.Model;
using JingZeUtil;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Web.Script.Serialization;
using Z.EntityFramework.Plus;

namespace JingZeServer.Util
{
    public class JZZSServer
    {
        public static HttpResponseMessage ToJson(bool isSuccess, object content)
        {
            string jsonString;

            if (isSuccess)
            {
                // 如果是 true，构造相应的 JSON 格式
                jsonString = JsonSerializer.Serialize(new
                {
                    Result = isSuccess,
                    Message = "",
                    Data = content // 将输入的内容放入 Data 字段
                });
            }
            else
            {
                // 如果是 false，构造相应的 JSON 格式
                jsonString = JsonSerializer.Serialize(new
                {
                    Result = isSuccess,
                    Message = content.ToString(), // 将输入的错误信息放入 Message 字段
                    Data = ""
                });
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
            };
        }
        public static string PGinsert(PG pG, DbBase<material> dbBase, DbBase<GZID> dbgz)
        {
            DbBase<RERFID> dbrfid = new DbBase<RERFID>();
            DbBase<ProWeigth> dbpro = new DbBase<ProWeigth>();
            string singleRecord = Class1.GetRFIDIDA();
            //string singleRecord = "E2806995000040020154ED7D";

            if (singleRecord != null)
            {
                string number = dbgz.FirstOrDefault(x => x.RFIDID == singleRecord).MateCode;
                var mata = dbBase.FirstOrDefault(x => x.number == number.Trim());
                ProWeigth proWeigth = new ProWeigth()
                {
                    ProNo = mata.number,
                    ProName = mata.name,
                    spec = mata.specification,
                    PGWeigth = pG.PGWeigth,
                    MPWeigth = mata.qbh,
                    RFIDID = singleRecord,
                    DateTime = DateTime.Now
                };
                using (var db = new JZZSEntities1())
                {
                    db.ProWeigth.Add(proWeigth);
                    db.SaveChanges();
                }
                return singleRecord;
            }
            else
            {
                return "";
            }

        }
        public static (double? gl, string singleRecord) PGUpdate(PG mG, DbBase<material> dbBase, DbBase<GZID> dbgz)
        {
            using (var db = new JZZSEntities1())
            {
                DbBase<RERFID> dbrfid = new DbBase<RERFID>();
                string singleRecord = Class1.GetRFIDIDA();
                //string singleRecord = "E2806995000040020154ED7D";
                if (singleRecord != null)
                {
                    string number = dbgz.FirstOrDefault(x => x.RFIDID == singleRecord)?.MateCode;
                    var mata = dbBase.FirstOrDefault(x => x.number == number.Trim());
                    var ProWeigth = db.ProWeigth
                                      .Where(x => x.RFIDID == singleRecord)
                                      .OrderByDescending(a => a.DateTime)
                                      .FirstOrDefault();
                    double? gl = mG.MGWeigth - ProWeigth.PGWeigth;
                    if (ProWeigth != null)
                    {
                        ProWeigth.MGWeigth = mG.MGWeigth;
                        ProWeigth.JGWeigth = gl;
                        ProWeigth.OrderProNo = "完成";
                        ProWeigth.DateTime = DateTime.Now;

                        db.SaveChanges();
                    }
                    return (gl, singleRecord);
                }
                else
                {
                    return (0, null);
                }
            }
        }
        public static string MataUpdate(PG mG, DbBase<material> dbBase, DbBase<GZID> dbgz)
        {
            using (var db = new JZZSEntities1())
            {
                DbBase<RERFID> dbrfid = new DbBase<RERFID>();
                string singleRecord = Class1.GetRFIDIDA();
                //string singleRecord = "E2806995000040020154ED7D";
                var ProWeigth = db.ProWeigth
                                  .Where(x => x.RFIDID == singleRecord)
                                  .OrderByDescending(a => a.DateTime)
                                  .FirstOrDefault();

                long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                if (ProWeigth != null)
                {

                    ProWeigth.OrderProNo = "未完成";
                    ProWeigth.ProID = mG.ProID;

                    ProWeigth.DateTime = DateTime.Now;


                    db.SaveChanges();
                }
                return singleRecord;
            }
        }

    }
}

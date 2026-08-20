using GDotnet.Reader.Api.DAL;
using GDotnet.Reader.Api.Protocol.Gx;
using JingZeUtil;
using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace JingZeServer
{
    public class Class1
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static string epcResult;  // 用于保存 EPC 信息
        private static ManualResetEvent epcReceivedEvent = new ManualResetEvent(false);  // 用于同步等待 EPC 的获取

        public static string GetRFIDIDA()
        {
            epcResult = null;
            GClient clientConn = new GClient();
            eConnectionAttemptEventStatusType status;

            if (clientConn.OpenTcp("192.168.1.168:8170", 3000, out status))
            {
                Logger.Info("192.168.1.168:8170 RFID读写器（顶升）连接成功！");

                // 订阅事件
                clientConn.OnEncapedTagEpcLog += new delegateEncapedTagEpcLog(OnEncapedTagEpcLog);
                clientConn.OnEncapedTagEpcOver += new delegateEncapedTagEpcOver(OnEncapedTagEpcOver);

                // 停止之前的操作
                MsgBaseStop msgBaseStop = new MsgBaseStop();
                clientConn.SendSynMsg(msgBaseStop);
                if (0 == msgBaseStop.RtCode)
                {
                    Logger.Info("192.168.1.168:8170 RFID读写器（顶升）");
                    //Console.WriteLine("Stop successful.");
                }
                else
                {
                    Logger.Info("");
                    //Console.WriteLine("Stop1 error.");
                }

                // 开始新操作
                MsgBaseInventoryEpc msgBaseInventoryEpc = new MsgBaseInventoryEpc();
                msgBaseInventoryEpc.AntennaEnable = (uint)(eAntennaNo._1 | eAntennaNo._2 | eAntennaNo._3 | eAntennaNo._4);
                msgBaseInventoryEpc.InventoryMode = (byte)eInventoryMode.Inventory;

                clientConn.SendSynMsg(msgBaseInventoryEpc);
                if (0 == msgBaseInventoryEpc.RtCode)
                {
                    Console.WriteLine("Inventory epc successful.");
                }
                else { Console.WriteLine("Inventory epc error."); }
                Thread.Sleep(1000);
                // 等待 EPC 事件完成
                if (epcReceivedEvent.WaitOne(5000)) // 等待最多 5 秒
                {
                    clientConn.Close();
                    // 如果成功收到 EPC，返回 EPC 信息
                    return epcResult;
                }
                else
                {
                    Console.WriteLine("Timeout waiting for EPC.");
                    return null;  // 超时未收到 EPC
                }

            }
            else
            {
                //Console.WriteLine("Connect failure.");
                Logger.Info("192.168.1.168:8170 RFID读写器（顶升）连接失败！");
                return null;
            }
        }

        public static string GetRFIDIDB(string IP, int port)
        {
            GClient clientConn = new GClient();
            eConnectionAttemptEventStatusType status;
            string ipport = IP + ":" + port;
            if (clientConn.OpenTcp(ipport, 3000, out status))
            {
                // 订阅事件
                clientConn.OnEncapedTagEpcLog += new delegateEncapedTagEpcLog(OnEncapedTagEpcLogc);
                clientConn.OnEncapedTagEpcOver += new delegateEncapedTagEpcOver(OnEncapedTagEpcOver);

                // 停止之前的操作
                MsgBaseStop msgBaseStop = new MsgBaseStop();
                clientConn.SendSynMsg(msgBaseStop);
                if (0 == msgBaseStop.RtCode)
                {
                    Console.WriteLine("Stop successful.");
                }
                else { Console.WriteLine("Stop1 error."); }

                // 开始新操作
                MsgBaseInventoryEpc msgBaseInventoryEpc = new MsgBaseInventoryEpc();
                msgBaseInventoryEpc.AntennaEnable = (uint)(eAntennaNo._1 | eAntennaNo._2 | eAntennaNo._3 | eAntennaNo._4);
                msgBaseInventoryEpc.InventoryMode = (byte)eInventoryMode.Inventory;

                clientConn.SendSynMsg(msgBaseInventoryEpc);
                if (0 == msgBaseInventoryEpc.RtCode)
                {
                    Console.WriteLine("Inventory epc successful.");
                }
                else { Console.WriteLine("Inventory epc error."); }

                // 等待 EPC 事件完成
                if (epcReceivedEvent.WaitOne(5000)) // 等待最多 5 秒
                {
                    // 如果成功收到 EPC，返回 EPC 信息
                    return epcResult;
                }
                else
                {
                    Console.WriteLine("Timeout waiting for EPC.");
                    return null;  // 超时未收到 EPC
                }

            }
            else
            {
                Console.WriteLine("Connect failure.");
                return null;
            }
        }
        public static string GetRFIDIDC(string IP, int port)
        {
            GClient clientConn = new GClient();
            eConnectionAttemptEventStatusType status;
            string ipport = IP + ":" + port;
            if (clientConn.OpenTcp(ipport, 3000, out status))
            {
                // 订阅事件
                clientConn.OnEncapedTagEpcLog += new delegateEncapedTagEpcLog(OnEncapedTagEpcLogc);
                clientConn.OnEncapedTagEpcOver += new delegateEncapedTagEpcOver(OnEncapedTagEpcOver);

                // 停止之前的操作
                MsgBaseStop msgBaseStop = new MsgBaseStop();
                clientConn.SendSynMsg(msgBaseStop);
                if (0 == msgBaseStop.RtCode)
                {
                    Console.WriteLine("Stop successful.");
                }
                else { Console.WriteLine("Stop1 error."); }

                // 开始新操作
                MsgBaseInventoryEpc msgBaseInventoryEpc = new MsgBaseInventoryEpc();
                msgBaseInventoryEpc.AntennaEnable = (uint)(eAntennaNo._1 | eAntennaNo._2 | eAntennaNo._3 | eAntennaNo._4);
                msgBaseInventoryEpc.InventoryMode = (byte)eInventoryMode.Inventory;

                clientConn.SendSynMsg(msgBaseInventoryEpc);
                if (0 == msgBaseInventoryEpc.RtCode)
                {
                    Console.WriteLine("Inventory epc successful.");
                }
                else { Console.WriteLine("Inventory epc error."); }

                // 等待 EPC 事件完成
                if (epcReceivedEvent.WaitOne(5000)) // 等待最多 5 秒
                {
                    // 如果成功收到 EPC，返回 EPC 信息
                    return epcResult;
                }
                else
                {
                    Console.WriteLine("Timeout waiting for EPC.");
                    return null;  // 超时未收到 EPC
                }

            }
            else
            {
                Console.WriteLine("Connect failure.");
                return null;
            }
        }

        #region API事件
        public static event delegateEncapedTagEpcLog OnEncapedTagEpc;
        public static string OnEncapedTagEpcLoga(EncapedLogBaseEpcInfo msg)
        {
            // 确保 msg 不是 null，且 Result 为 0 表示成功
            if (msg != null && msg.logBaseEpcInfo.Result == 0)
            {
                Console.WriteLine(msg.reader + ":ant[" + msg.logBaseEpcInfo.AntId + "] EPC: " + msg.logBaseEpcInfo.Epc + " | TID: " + msg.logBaseEpcInfo.Tid);

                // 返回 EPC 信息
                return msg.logBaseEpcInfo.Epc;
            }

            // 如果不满足条件，返回 null 或其他标记
            return null;
        }
        public static void OnEncapedTagEpcLog(EncapedLogBaseEpcInfo msg)
        {
            DbBase<RERFID> db = new DbBase<RERFID>();
            if (msg != null && msg.logBaseEpcInfo.Result == 0)
            {
                Console.WriteLine(msg.reader + ":ant[" + msg.logBaseEpcInfo.AntId + "]" + msg.logBaseEpcInfo.Epc + "|" + msg.logBaseEpcInfo.Tid);

                // 存储 EPC 信息
                epcResult = msg.logBaseEpcInfo.Epc;
                Task.Run(() => InsertA(msg.logBaseEpcInfo.Epc, db));
                // 通知主线程已收到 EPC
                epcReceivedEvent.Set();
            }
        }
        public static void OnEncapedTagEpcLogb(EncapedLogBaseEpcInfo msg)
        {
            if (msg != null && msg.logBaseEpcInfo.Result == 0)
            {
                Console.WriteLine(msg.reader + ":ant[" + msg.logBaseEpcInfo.AntId + "]" + msg.logBaseEpcInfo.Epc + "|" + msg.logBaseEpcInfo.Tid);

                // 存储 EPC 信息
                epcResult = msg.logBaseEpcInfo.Epc;
                Task.Run(() => InsertB(msg.logBaseEpcInfo.Epc));
                // 通知主线程已收到 EPC
                epcReceivedEvent.Set();
            }


        }
        public static void OnEncapedTagEpcLogc(EncapedLogBaseEpcInfo msg)
        {
            if (msg != null && msg.logBaseEpcInfo.Result == 0)
            {
                Console.WriteLine(msg.reader + ":ant[" + msg.logBaseEpcInfo.AntId + "]" + msg.logBaseEpcInfo.Epc + "|" + msg.logBaseEpcInfo.Tid);

                // 存储 EPC 信息
                epcResult = msg.logBaseEpcInfo.Epc;
                Task.Run(() => InsertC(msg.logBaseEpcInfo.Epc));
                // 通知主线程已收到 EPC
                epcReceivedEvent.Set();
            }


        }

        public static void InsertA(string msg, DbBase<RERFID> db)
        {

            RERFID rERFID = new RERFID()
            {
                RFIDID = msg,
                RFIDName = "192.168.0.200",
                DateTime = DateTime.Now,
            };
            db.Insert(rERFID);
            db.SaveChanges();

        }
        public static void InsertB(string msg)
        {
            DbBase<RERFID> db = new DbBase<RERFID>();
            RERFID rERFID = new RERFID()
            {
                RFIDID = msg,
                RFIDName = "192.168.0.168",
                DateTime = DateTime.Now,
            };
            db.Insert(rERFID);
            db.SaveChanges();

        }
        public static void InsertC(string msg)
        {
            DbBase<RERFID> db = new DbBase<RERFID>();
            RERFID rERFID = new RERFID()
            {
                RFIDID = msg,
                RFIDName = "192.168.0.201",
                DateTime = DateTime.Now,
            };
            db.Insert(rERFID);
            db.SaveChanges();

        }

        public static void OnEncapedTagEpcOver(EncapedLogBaseEpcOver msg)
        {
            if (null != msg)
            {
                Console.WriteLine("Epc log over.");
            }
        }

        #endregion

    }
}

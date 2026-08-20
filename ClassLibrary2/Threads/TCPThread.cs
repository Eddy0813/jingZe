using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using JingZeUtil;
using NLog;

namespace JingZeServer.Threads
{
    public class TCPThread
    {
        private static TcpClient client;
        string serverIP = "192.168.0.10";
        int port = 7771;
        private bool isExiting = false;
        private bool isConnected = false;
        private static NetworkStream stream;
        static int prevStartFlag = 0; // 初始化上一次的开始标志位
        static int prevLocalFlag = 1;
        static SerialPort serialPort = new SerialPort("COM5", 9600, Parity.None, 8, StopBits.One);//树脂
        static SerialPort serialPort1 = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);//玻钎
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        public async void Start() //60秒执行一次
        {
            if (!serialPort.IsOpen)
            {
                serialPort.Open();
                serialPort1.Open();
            }
            await ConnectToServerAsync();
        }

        public async Task ConnectToServerAsync()
        {
            if (isConnected && client?.Connected == true)
            {
                Logger.Info("已连接，无需重复连接。");
                return;
            }

            try
            {
                client?.Dispose(); // 清理旧连接
                client = new TcpClient();
                await client.ConnectAsync(serverIP, port);
                stream = client.GetStream();
                isConnected = true;
                _ = ListenForMessagesAsync(); // 启动监听任务
            }
            catch (Exception ex)
            {
                Logger.Error($"连接失败: {ex.Message}");
                await RetryConnectionAsync();
            }
        }

        private async Task ListenForMessagesAsync()
        {
            byte[] buffer = new byte[1024]; // 使用更大缓冲区
            try
            {
                while (isConnected && !isExiting)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) // 服务器断开
                    {
                        Logger.Error("服务器已断开连接");
                        break;
                    }

                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    OnTimedEvent(response);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"监听异常: {ex.Message}");
            }
            finally
            {
                Disconnect(); // 确保资源释放
                await RetryConnectionAsync();
            }
        }

        private void Disconnect()
        {
            try
            {
                isConnected = false;
                stream?.Dispose();
                client?.Dispose();
            }
            catch { /* 忽略释放异常 */ }
        }

        private async Task RetryConnectionAsync()
        {
            if (!isExiting)
            {
                await Task.Delay(5000);
                await ConnectToServerAsync();
            }
        }

        static (int startFlag, int station) GetMessage(string input)
        {
            // 假设输入格式："CLROBOT,开始标志位,当前工位"
            string[] parts = input.Split(',');
            if (parts.Length == 3 && parts[0] == "CLROBOT")
            {
                int startFlag = int.Parse(parts[1]);
                int station = int.Parse(parts[2]);
                return (startFlag, station);
            }
            else
            {
                throw new FormatException("输入的字符串格式不正确");
            }
        }

        private static void OnTimedEvent(string input)
        {
            DbBase<ProWeigth> dbpro = new DbBase<ProWeigth>();
            DbBase<MaterialLoss> dbmate = new DbBase<MaterialLoss>();
            try
            {
                var (startFlag, station) = GetMessage(input);
                var (bq, sq) = getweigth();//获取树脂玻纤的重量

                if (prevStartFlag == 0 && startFlag == 1)
                {
                    var config = GetStationConfig(station);
                    string rfidId = station == 1
                        ? Class1.GetRFIDIDB(config.Ip, config.Port)
                        : Class1.GetRFIDIDC(config.Ip, config.Port);
                    var proWeigth = dbpro.GetList(x => x.RFIDID == rfidId).Last();
                    SaveMaterialLoss(dbmate, rfidId, station.ToString(), "开始",
                        proWeigth.OrderNo, proWeigth.ProID, bq, sq, 0, config.Desc);
                }
                else if (prevStartFlag == 1 && startFlag == 0)
                {
                    var config = GetStationConfig(station);
                    string rfidId = station == 1
                        ? Class1.GetRFIDIDB(config.Ip, config.Port)
                        : Class1.GetRFIDIDC(config.Ip, config.Port);
                    var proWeigth = dbpro.GetList(x => x.RFIDID == rfidId).Last();
                    SaveMaterialLoss(dbmate, rfidId, station.ToString(), "结束",
                        proWeigth.OrderNo, proWeigth.ProID, bq, sq, 0, config.Desc);
                }
                // 其他状态处理...

                prevLocalFlag = station;
                prevStartFlag = startFlag;
            }
            catch (Exception ex)
            {
                Logger.Error($"处理异常: {ex.Message}");
                // 记录完整异常信息

            }
        }

        private static (string Ip, int Port, string Desc) GetStationConfig(int station)
        {
            switch (station)
            {
                case 1:
                    return ("192.168.0.168", 8160, "B");
                case 2:
                    return ("192.168.0.201", 8170, "C");
                default:
                    throw new ArgumentException("无效的工位编号");
            }
        }

        private static void SaveMaterialLoss(DbBase<MaterialLoss> dbmate, string rfidId, string line,
    string status, string orderNo, string proId, double gfWeight,
    double resinWeight, int cs, string stationDesc)
        {
            var existing = dbmate.FirstOrDefault(x => x.RFIDID == rfidId);
            int currentCs = existing == null ? 1 : (existing.CS == 2 ? 1 : 2);

            var materialLoss = new MaterialLoss()
            {
                RFIDID = rfidId,
                Line = line,
                Status = status,
                OrderNo = orderNo,
                Reserver1 = proId,
                GFweight = gfWeight,
                Resinweight = resinWeight,
                CS = currentCs,
                DateTime = DateTime.Now,
            };

            dbmate.Insert(materialLoss);
            dbmate.SaveChanges();
            Logger.Info($"{stationDesc}工位{status}操作，RFID: {rfidId}");
        }

        static (float bq, float sq) getweigth()
        {
            string messageToSend = "R";
            serialPort.WriteLine(messageToSend);
            string incomingMessage = serialPort.ReadLine();
            serialPort1.WriteLine(messageToSend);
            string incomingMessage1 = serialPort1.ReadLine();
            string result = incomingMessage.Replace("wn", "").Replace("kg", "").Trim();
            float bq = float.Parse(result);
            string result1 = incomingMessage1.Replace("wn", "").Replace("kg", "").Trim();
            float sq = float.Parse(result1);
            return (bq, sq);
        }

        public static string test()
        {
            string datetest = DateTime.Now.ToString();
            Logger.Info(datetest);
            return datetest;
        }
    }
}

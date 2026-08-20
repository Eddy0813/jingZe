using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using JingZeServer;
using JingZeServer.Threads;
using JingZeServer.Util.PostUtils;
using JingZeUtil;
using Newtonsoft.Json;
using NLog;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace jztest
{
    public partial class Form1 : Form
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        string connectionString2 = "data source=127.0.0.1;initial catalog=JZZS;persist security info=True;user id=sa;password=telenadmin99;password=telenadmin99;";
        public Form1()
        {
            InitializeComponent(); 
            SetupNLog();
            //serialPort.Open();
            //serialPort1.Open();
            Logger.Info($"程序已启动");
        }
        private void SetupNLog()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // 日志文件（按日期分割）
            var logfile = new NLog.Targets.FileTarget("logfile")
            {
                FileName = "logs/${shortdate}.log",
                Layout = "${longdate}|${level}|${message} ${exception:format=tostring}"
            };

            // 控制台日志（调试用）
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // 日志规则
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);

            NLog.LogManager.Configuration = config;
        }

        TCPThread TCPThread =new TCPThread();
        static SerialPort serialPort = new SerialPort("COM5", 9600, Parity.None, 8, StopBits.One);//树脂
        static SerialPort serialPort1 = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);//玻钎
        //static SerialPort serialPort2 = new SerialPort("COM7", 9600, Parity.None, 8, StopBits.One);//玻钎
        private void button1_Click(object sender, EventArgs e)
        {
            TCPThread.Start();
            
            textBox1.Text = "服务已启动";
            //Class1.test();
        }
        Timer timer;
        public void Start(int RepeatTime = 5000) //60秒执行一次
        {
            
            //定时执行
            timer = new Timer();
            timer.Enabled = true;
            timer.Interval = RepeatTime;//执行间隔时间,单位为毫秒
            timer.Tick += new EventHandler(test);
            timer.Start();
        }
        public void test(object sender,EventArgs eventArgs)
        {
         string test=Class1.GetRFIDIDA();
            textBox1.Text = test; 
        }
        string serviceFilePath = $"{Application.StartupPath}\\JZService1.exe";
        string serviceName = "jzzsService1";

        #region 服务方法
        #endregion 服务方法

        private void button2_Click(object sender, EventArgs e)
        {
            //TCPThread.test();
            string messageToSend = "R";
            serialPort1.WriteLine(messageToSend);
            string incomingMessage = serialPort1.ReadLine();
            string result = incomingMessage.Replace("wn", "").Replace("kg", "").Trim();

            /*  DbBase<MaterialLoss> dbmate = new DbBase<MaterialLoss>();
              DbBase<ProWeigth> dbpro = new DbBase<ProWeigth>();

              ProWeigth proWeigths = dbpro.GetList(x => x.RFIDID == "E2806995000050175E616630").Last();
              string proid = proWeigths.ProID;
              MaterialLoss materialLoss = new MaterialLoss()
              {
                  RFIDID = "E2806995000050175E616630",
                  Line = "1",
                  Status = "结束",
                  OrderNo = proWeigths.OrderNo,
                  Reserver1 = proWeigths.ProID,
                  GFweight = 391.2,
                  Resinweight = 291.3,
                  CS = 1,
                  DateTime = DateTime.Now,
              };
              dbmate.Insert(materialLoss);
              dbmate.SaveChanges();
              */
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            List<string> codes = new List<string>
{
    "18.03.01.001",
    "18.03.01.002",
    "18.03.01.003",
    "18.03.01.004",
    "18.03.01.005",
    "18.03.01.006",
    "18.03.01.007",
    "18.03.01.007",
    "18.03.01.008",
    "18.03.01.008",
    "18.03.01.009",
    "18.03.01.010",
    "18.03.01.011",
    "18.03.01.012",
    "18.03.01.013",
    "18.03.01.014",
    "18.03.01.015",
    "18.03.01.016",
    "18.03.01.017",
    "18.03.01.018",
    "18.03.01.021",
    "18.03.01.021",
    "18.03.01.022",
    "18.03.01.023",
    "18.03.01.024",
    "18.03.01.025",
    "18.03.01.026",
    "18.03.01.028",
    "18.03.01.029",
    "18.03.01.038",
    "18.03.01.040",
    "18.03.01.041",
    "18.03.01.042",
    "18.03.01.045",
    "18.03.01.047",
    "18.03.01.051",
    "18.03.01.053",
    "18.03.01.054",
    "18.03.01.055",
    "18.03.01.056",
    "18.03.01.057",
    "18.03.01.058",
    "18.03.01.059",
    "18.03.01.060",
    "18.03.01.061",
    "18.03.01.062",
    "18.03.01.064",
    "18.03.01.065",
    "18.03.01.066",
    "18.03.01.067",
    "18.03.01.072",
    "18.03.01.073",
    "18.03.01.074",
    "18.03.03.002"
};
            //await Selectmate.loginAsync();
          var result=  await Selectmate.select(codes[5]);
           button5.Text= result.ToString();

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

        DataTable dt2 = new DataTable();
        string query2 = @"
        SELECT * from cp 
        ";
        private void button4_Click(object sender, EventArgs e)
        {

            using (SqlConnection connection2 = new SqlConnection(connectionString2))
            {
                try
                {
                    connection2.Open();
                    using (SqlCommand command2 = new SqlCommand(query2, connection2))
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(dt2);
                    }

                    // 转换 DataTable 为 JSON 格式
                    string jsonResult = JsonConvert.SerializeObject(dt2, Newtonsoft.Json.Formatting.Indented);
                    Console.WriteLine(jsonResult);
                    dt2.Clear();

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"查询出错: {ex.Message}");
                }
            }
        }
    }
}

using JingZeServer;
using JingZeServer.Threads;
using JingZeServer.Util.PostUtils;
using System.ServiceProcess;
namespace JZ_BackServer
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        TCPThread TCPThread = new TCPThread();

        protected override void OnStart(string[] args)
        {
            TCPThread.Start();
        }

        protected override void OnStop()
        {
        }
    }
}

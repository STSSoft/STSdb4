using STSdb4.Database;
using STSdb4.General.Communication;
using STSdb4.Remote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STSdb4.Server
{
    public partial class STSdb4Service : ServiceBase
    {
        internal static STSdb4Service Service { get; private set; }
        private MainForm Form;
        private IStorageEngine StorageEngine;
        private TcpServer TcpServer;

        public STSdb4Service()
        {
            InitializeComponent();
        }

        public void Start()
        {
            OnStart(new string[] { });
        }

        protected override void OnStart(string[] args)
        {
            string FileName = ConfigurationSettings.AppSettings["FileName"];
            int port = int.Parse(ConfigurationSettings.AppSettings["Port"]);
            int boundedCapacity = int.Parse(ConfigurationSettings.AppSettings["BoundedCapacity"]);
            Service = this;

            StorageEngine = STSdb.FromFile(FileName);
            TcpServer = new TcpServer(port);
            Program.StorageEngineServer = new StorageEngineServer(StorageEngine, TcpServer);
            Program.StorageEngineServer.Start();

            Form = new MainForm();
            Application.Run(Form);
            Program.StorageEngineServer.Stop();
            StorageEngine.Close();
        }

        protected override void OnStop()
        {
            if (Form != null)
                Form.Close();
            if (StorageEngine != null)
                StorageEngine.Close();
            if (Program.StorageEngineServer != null)
                Program.StorageEngineServer.Stop();
        }
    }
}

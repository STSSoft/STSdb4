using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using STSdb4.Database;
using STSdb4.Data;
using STSdb4.Database.Operations;
using STSdb4.WaterfallTree;
using System.IO;
using System.Configuration;
using STSdb4.Remote;

namespace STSdb4.Server
{
    static class Program
    {
        internal static StorageEngineServer StorageEngineServer;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        static void Main()
        {
            string serviceMode = ConfigurationSettings.AppSettings["ServiceMode"];
            bool isService = bool.Parse(serviceMode);

            if (!isService)
                new STSdb4Service().Start();
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                { 
                    new STSdb4Service() 
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}

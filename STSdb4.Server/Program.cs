using System.ServiceProcess;
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

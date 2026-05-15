using NetworkHandler;
using System.Diagnostics;
using System.Net;
using FilePacket;

namespace MainForm
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string appDirectory = Path.GetDirectoryName(exePath);
            Directory.SetCurrentDirectory(appDirectory);

            // BODY --->
            
            ConfigManager.Initialize(appDirectory);

            NetworkResponser.Initialize(
                ConfigManager.GetValue("appKey"),
                ConfigManager.GetValue("nickName"),
                ConfigManager.GetValue("dataPath"));

            // :: Configuration starter ::
            // 

            string method = ConfigManager.GetValue("strategy"); // Getting strategy method

            if (method == "default") // default = UDP standart
            {
                Task.Run(() => NetworkReceiver.Listener());
                Task.Run(() => NetworkController.Is_Online());
            }

            if (method == "cloud") { } // cloud = Yandex Disk (Cloud) — заглушка

            Application.Run(new MainForm_Welcome());
        }
    }
}
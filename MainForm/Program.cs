using NetworkHandler;

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

            Task.Run(() => NetworkReciever.Listener());
            Task.Run(() => NetworkController.Is_Online());

            Application.Run(new MainForm_Welcome());
        }
    }
}
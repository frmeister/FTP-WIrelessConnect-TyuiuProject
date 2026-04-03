using UDP_BroadcastHandler;

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
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Чтобы выключить слушалку
            // UDP_Reciever.IsRunning = false;

            Task.Run(() => UDP_Reciever.Listener());
            Task.Run(() => UDP_Controller.Is_Online());

            Application.Run(new MainForm_Welcome());
        }
    }
}
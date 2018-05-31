using Ninject;
using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using WalletMonitorApp.Log;
using WalletMonitorApp.Properties;

namespace WalletMonitorApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IKernel Kernel { get; set; }
        private Logger logger = new Logger(typeof(AppBootstrapper));

        public static String VersionNumber
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Assembly.GetExecutingAssembly().GetName().Version.Major);
                sb.Append(".");
                sb.Append(Assembly.GetExecutingAssembly().GetName().Version.Minor);
                if (Assembly.GetExecutingAssembly().GetName().Version.Build != 0)
                {
                    sb.Append(".");
                    sb.Append(Assembly.GetExecutingAssembly().GetName().Version.Build);
                }
                return sb.ToString();
            }
        }

        public App()
        {
            logger.LogInfoMessage("App init");
            logger.LogInfoMessage("Version " + App.VersionNumber);

            AppDomain currentDomain = AppDomain.CurrentDomain;
            //currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnmanagedException);

            CultureInfo customCulture = new CultureInfo("en-EN");
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            Thread.CurrentThread.CurrentUICulture = customCulture;
            Thread.CurrentThread.CurrentCulture = customCulture;
            InitializeComponent();
            ShutdownMode = ShutdownMode.OnLastWindowClose;
        }

        private void UnmanagedException(object sender, UnhandledExceptionEventArgs args)
        {
            logger.LogError("Unmanaged exception occurs. Application terminated");
            Exception e = (Exception)args.ExceptionObject;
            logger.LogException(e);
            MessageBox.Show("Application terminated due internal error. See log file for more information");
            Environment.Exit(0);
        }
    }
}

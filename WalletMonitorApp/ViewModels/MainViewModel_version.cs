using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using WalletMonitorApp.Models;
using WalletMonitorServices;
using WalletMonitorApp.Properties;

namespace WalletMonitorApp.ViewModels
{
    public partial class MainViewModel : Caliburn.Micro.PropertyChangedBase
    {
        public string Version { get; set; }
        public string WindowTitle
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(Resources.WindowTitleMain);
                sb.Append(" ");
                sb.Append(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
                return sb.ToString();
            }
        }
        
        public void GetVersion(System.Action action)
        {
            Task.Run(() => _loginService.GetVersion().ContinueWith((result) =>
            {

                if (result.Exception != null)
                {
                    MessageBox.Show("Could not get version information.\nApplication will exit");
                    Environment.Exit(0);
                }
                if (result.Result.Version != "0")
                {
                    if (result.Result.Version != System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())
                    {
                        NewVersion = "New version available: " + result.Result.Version.ToString();
                    }
                    NotifyOfPropertyChange(() => WindowTitle);
                    action();
                }
            }));
        }
    }
}

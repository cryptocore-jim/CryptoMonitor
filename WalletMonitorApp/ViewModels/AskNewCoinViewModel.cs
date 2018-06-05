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
using WalletMonitorApp.Views;
using WalletMonitorServices.DTO;
using Ninject;

namespace WalletMonitorApp.ViewModels
{
    public partial class AskNewCoinViewModel : Caliburn.Micro.Screen
    {
        public AskNewCoinViewModel()
        {
        }
    }
}

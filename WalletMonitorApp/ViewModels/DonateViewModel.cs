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
    public partial class DonateViewModel : Caliburn.Micro.Screen
    {
        private object locker = new object();
        private List<DonationAddressDTO> _addresses = new List<DonationAddressDTO>();
        private WalletService _walletService;
        public DonateViewModel(WalletService ws)
        {
            _walletService = ws;
            Task.Run(() => _walletService.GetDonationAddresses().ContinueWith((result) =>
            {
                if (result.Exception != null)
                {
                    MessageBox.Show(result.Exception.Message);
                    Environment.Exit(0);
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var ticker in result.Result)
                    {
                        lock (locker)
                        {
                            _addresses.Add(ticker);
                        }
                    }
                });
            }));
        }

        public string DonationAddresses
        {
            get
            {
                var sb = new StringBuilder();
                lock (locker)
                {
                    foreach(var addr in _addresses)
                    {
                        sb.Append(addr.CoinSymbol);
                        sb.Append(": ");
                        sb.AppendLine(addr.Address);
                    }
                }
                return sb.ToString();
            }
        }
    }
}

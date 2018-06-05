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
using WalletMonitorApp.Helpers;
using System.ComponentModel;
using System.Diagnostics;

namespace WalletMonitorApp.ViewModels
{
    public partial class MainViewModel : Caliburn.Micro.PropertyChangedBase
    {
        private LoginService _loginService;
        private WalletService _walletService;
        private PoolingService _poolingService;
        private AddAddressViewModel _addAddressViewModel;
        private DonateViewModel _donateViewModel;
        private bool firstLogin = false;
        public MainViewModel(PoolingService ps, LoginService ls, WalletService ws, AddAddressViewModel aavm, DonateViewModel dvm)
        {
            _poolingService = ps;
            _addAddressViewModel = aavm;
            _donateViewModel = dvm;
            _loginService = ls;
            _walletService = ws;
            firstLogin = true;
            GetVersion(() => { LoginSeed(); });
            
        }

        private String _newVersion = "";
        public String NewVersion
        {
            get
            {
                return _newVersion;
            }
            set
            {
                _newVersion = value;
                NotifyOfPropertyChange(() => NewVersion);
            }
        }

        private String _currentMainView = "WalletView";
        public String CurrentMainView
        {
            get
            {
                return _currentMainView;
            }
            set
            {
                _currentMainView = value;
                NotifyOfPropertyChange(() => CurrentMainView);
            }
        }

        public Decimal TotalBTC
        {
            get
            {
                var totalBTC = _walletList.Sum(s => s.AmountBTC.GetValueOrDefault());
                return totalBTC;
            }
        }

        public Decimal TotalUSD
        {
            get
            {
                return _walletList.Sum(s => s.AmountUSD.GetValueOrDefault());
            }
        }

        public Decimal TotalEUR
        {
            get
            {
                return _walletList.Sum(s => s.AmountEUR.GetValueOrDefault());
            }
        }

        private ObservableCollection<Wallet> _walletList = new ObservableCollection<Wallet>();
        public ObservableCollection<Wallet> WalletList
        {
            get
            {
                return _walletList;
            }
            set
            {
                _walletList = value;
                NotifyOfPropertyChange(() => WalletList);
            }
        }

        public void DiffSeedLogin()
        {
            _poolingService.BalanceUpdated -= _balanceUpdated;
            WalletList.Clear();
            CurrentMainView = "LoginView";
        }

        public void AppExit()
        {
            Environment.Exit(0);
        }

        public void AddNewAddress()
        {
            IWindowManager manager = new WindowManager();
            manager.ShowDialog(_addAddressViewModel, null, null);
        }

        public void Donate()
        {
            IWindowManager manager = new WindowManager();
            manager.ShowDialog(_donateViewModel, null, null);
        }

        public void Discord()
        {
            Process.Start("https://discord.gg/cQNZNxN");
        }

        public async void RemoveAddress()
        {
            var addresses = WalletList.Where(w => w.Selected).ToList();
            List<Task<BoolJsonDTO>> taksList = new List<Task<BoolJsonDTO>>();
            foreach (var address in addresses)
            {
                taksList.Add(_walletService.RemoveAddress(Settings.Default.Seed, address.Address, address.CoinSymbol));
            }
            await Task.WhenAll(taksList);
            foreach (var address in addresses)
            {
                _poolingService.RemoveAddress(address.Address);
                WalletList.Remove(address);
            }
        }

        private object _sync = new object();
        private ObservableCollection<String> _tickerList;
        public ObservableCollection<String> TickerList
        {
            get
            {
                ObservableCollection<String> result;
                lock (_sync)
                {
                    result = _tickerList;
                }
                return result;
            }
            set
            {
                lock (_sync)
                {
                    _tickerList = value;
                    NotifyOfPropertyChange(() => TickerList);
                }
            }
        }

        public void _balanceUpdated(BalanceWalletDTO balance, string address, string seed)
        {
            var vlt = _walletList.FirstOrDefault(w => w.Address == address);
            if (vlt != null)
            {
                vlt.Amount = balance.Amount;
                vlt.AmountBTC = CurrencyHelper.BTCHelperDecimal(balance.PriceBTC * balance.Amount);
                vlt.AmountEUR = CurrencyHelper.FiatHelperDecimal(balance.PriceEUR * balance.Amount);
                vlt.AmountUSD = CurrencyHelper.FiatHelperDecimal(balance.PriceUSD * balance.Amount);
                if (!string.IsNullOrEmpty(balance.MasternodeStatus))
                {
                    vlt.IsMasternode = true;
                }
                vlt.MasternodeStatus = balance.MasternodeStatus;
                vlt.LastUpdated = balance.LastUpdated;
                vlt.Degraded = balance.Degraded;
                vlt.Error = balance.Error;
                vlt.PriceBTC = balance.PriceBTC;
                vlt.Trend1 = balance.Trend1;
                vlt.Trend7 = balance.Trend7;
                vlt.Trend14 = balance.Trend14;
                NotifyOfPropertyChange(() => TotalBTC);
                NotifyOfPropertyChange(() => TotalEUR);
                NotifyOfPropertyChange(() => TotalUSD);
            }
        }

        public String SortBy { get; set; }
        public ListSortDirection SortOrder { get; set; }

    }
}

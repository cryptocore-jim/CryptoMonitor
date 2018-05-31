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
    public partial class AddAddressViewModel : Caliburn.Micro.Screen
    {
        private WalletService _walletService;
        private PoolingService _poolingService;
        public AddAddressViewModel(WalletService ws, PoolingService ps)
        {
            _poolingService = ps;
            _walletService = ws;
            Task.Run(() => _walletService.GetAvailabeTickers().ContinueWith((result) =>
            {
                if (result.Exception != null)
                {
                    MessageBox.Show(result.Exception.Message);
                    Environment.Exit(0);
                }
                var tickerSelected = new TickerDTO()
                {
                    CoinSymbol = "Auto Detect",
                    Degraded = false
                };
                Application.Current.Dispatcher.Invoke(() =>
                {
                    TickerList.Add(tickerSelected);
                    foreach (var ticker in result.Result.OrderBy(o => o.CoinSymbol).ToList())
                    {
                        TickerList.Add(ticker);
                    }
                    NotifyOfPropertyChange(() => TickerList);
                    SelectedTicker = tickerSelected;
                });
            }));
        }

        private string _address;
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                NotifyOfPropertyChange(() => Address);
            }
        }

        private TickerDTO _selectedTicker;
        public TickerDTO SelectedTicker
        {
            get
            {
                return _selectedTicker;
            }
            set
            {
                _selectedTicker = value;
                NotifyOfPropertyChange(() => SelectedTicker);
            }
        }

        private object _sync = new object();
        private ObservableCollection<TickerDTO> _tickerList = new ObservableCollection<TickerDTO>();
        public ObservableCollection<TickerDTO> TickerList
        {
            get
            {
                ObservableCollection<TickerDTO> result;
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

        private bool _addIsEnabled = true;
        public bool AddIsEnabled
        {
            get
            {
                return _addIsEnabled;
            }
            set
            {
                _addIsEnabled = value;
                NotifyOfPropertyChange(() => AddIsEnabled);
            }
        }

        public async void AddNewAddressToSeed()
        {
            var seed = Settings.Default.Seed;
            try
            {
                AddIsEnabled = false;
                var ticker = SelectedTicker;
                if (SelectedTicker.CoinSymbol == "Auto Detect")
                {
                    try
                    {
                        var resultAuto = await _walletService.DetectAddress(Address);
                        if (string.IsNullOrEmpty(resultAuto.CoinSymbol))
                        {
                            throw new Exception();
                        }
                        ticker.CoinSymbol = resultAuto.CoinSymbol;
                    }
                    catch
                    {
                        MessageBox.Show("Could not detect wallet ticker.");
                        AddIsEnabled = true;
                        return;
                    }

                }
                var result = await _walletService.AddNewAddress(seed, Address, ticker.CoinSymbol);
                if (string.IsNullOrEmpty(result.Address))
                {
                    MessageBox.Show("Could not add wallet");
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        App.Kernel.Get<MainViewModel>().WalletList.Add(new Wallet()
                        {
                            Address = result.Address,
                            CoinName = result.CoinName,
                            ExplorerURL = result.ExplorerURL,
                            CoinSymbol = result.CoinSymbol
                        });
                    });
                    _poolingService.AddAddress(new WalletDTO()
                    {
                        Seed = Settings.Default.Seed,
                        Address = result.Address
                    });
                    _poolingService.ForceUpdate();
                    TryClose();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            AddIsEnabled = true;
        }

    }
}

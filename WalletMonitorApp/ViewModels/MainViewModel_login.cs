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
using WalletMonitorServices.DTO;

namespace WalletMonitorApp.ViewModels
{
    public partial class MainViewModel : Caliburn.Micro.PropertyChangedBase
    {

        private String _defaultSeed = Settings.Default.Seed;
        public String DefaultSeed
        {
            get
            {
                return _defaultSeed;
            }
            set
            {
                _defaultSeed = value;
                NotifyOfPropertyChange(() => DefaultSeed);
                Settings.Default.Seed = DefaultSeed;
                Settings.Default.Save();
            }
        }

        public void LoginSeed()
        {
            Task.Run(() => _loginService.GetSeed(Settings.Default.Seed)).ContinueWith(async (result) =>
            {
                if (result.Exception != null)
                {
                    MessageBox.Show("Could not get seed information.\nApplication will exit");
                    Environment.Exit(0);
                }
                if (result.Result.Result == false)
                {
                    if (!firstLogin)
                    {
                        MessageBox.Show("Unknown seed");
                    }
                    firstLogin = false;
                    CurrentMainView = "LoginView";
                }
                else
                {
                    _poolingService.ClearWallets();
                    try
                    {
                        var wallets = await _walletService.GetWallets(Settings.Default.Seed);
                        foreach (var wallet in wallets)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                WalletList.Add(new Wallet()
                                {
                                    Address = wallet.Address,
                                    CoinName = wallet.CoinName,
                                    ExplorerURL = wallet.ExplorerURL,
                                    CoinSymbol = wallet.CoinSymbol
                                });
                            });
                            _poolingService.AddAddress(new WalletDTO()
                            {
                                Seed = Settings.Default.Seed,
                                Address = wallet.Address
                            });
                        }
                        _poolingService.BalanceUpdated += _balanceUpdated;
                        _poolingService.ForceUpdate();
                        CurrentMainView = "WalletView";
                        firstLogin = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\nApplication will exit");
                        Environment.Exit(0);
                    }
                }

            });
        }

        public async void CreateNewSeed()
        {
            try
            {
                var seedDTO = await _loginService.CreateNewSeed();
                Settings.Default.Seed = seedDTO.Seed;
                Settings.Default.Save();
                DefaultSeed = seedDTO.Seed;
            }
            catch
            {
                MessageBox.Show("Could not generate new seed");
            }
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using WalletMonitorServices.DTO;

namespace WalletMonitorServices
{
    public delegate void BalanceUpdated(BalanceWalletDTO balance, string address, string seed);
    public class PoolingService
    {
        private HttpClient _httpClient;
        private object locker = new object();
        private List<WalletDTO> _wallets = new List<WalletDTO>();

        public PoolingService(HttpClient hc)
        {
            _httpClient = hc;
            Thread thread = new Thread(_worker);
            thread.Start();
            System.Timers.Timer timer = new System.Timers.Timer(120000);
            timer.Elapsed += (sender, e) =>
            {
                ForceUpdate();
            };
            timer.Start();
        }

        /// <summary>
        /// add addresses to worker
        /// </summary>
        /// <param name="address"></param>
        public void RemoveAddress(string address)
        {
            lock (locker)
            {
                var wallet = _wallets.FirstOrDefault(w => w.Address == address);
                if (wallet != null)
                {
                    _wallets.Remove(wallet);
                }
            }
        }

        /// <summary>
        /// clear all worker addreses
        /// </summary>
        public void ClearWallets()
        {
            lock (locker)
            {
                _wallets.Clear();
            }
        }
        /// <summary>
        /// Add address to worker
        /// </summary>
        /// <param name="wallet"></param>
        public void AddAddress(WalletDTO wallet)
        {
            lock(locker)
            {
                _wallets.Add(wallet);
            }
        }

        /// <summary>
        /// Force update wallet status
        /// </summary>
        public void ForceUpdate()
        {
            lock (locker)
            {
                Monitor.Pulse(locker);
            }
        }
        /// <summary>
        /// event for worker updates
        /// </summary>
        public event BalanceUpdated BalanceUpdated;

        private bool _sleep;
        /// <summary>
        /// Update worker
        /// </summary>
        /// <param name="data"></param>
        private void _worker(object data)
        {
            while (true)
            {
                List<WalletDTO> processWallets;
                lock (locker)
                {
                    processWallets = new List<WalletDTO>(_wallets);
                }
                foreach(var wallet in processWallets)
                {
                    try
                    {
                        throw new Exception();
                        var json = _httpClient.GetStringAsync(string.Format("http://monitorapi.ccore.online/api/getbalance?seed={0}&address={1}", wallet.Seed, wallet.Address)).Result;
                        var balance = JsonConvert.DeserializeObject<BalanceWalletDTO>(json);
                        BalanceUpdated?.Invoke(balance, wallet.Address, wallet.Seed);
                    }
                    catch
                    {
                        var balance = new BalanceWalletDTO()
                        {
                            Error = true
                        };
                        BalanceUpdated?.Invoke(balance, wallet.Address, wallet.Seed);
                        //log exception
                    }
                }
                lock (locker)
                {
                    _sleep = true;
                    while (_sleep)
                    {
                        Monitor.Wait(locker);
                        _sleep = false;
                    }
                }
            }
        }
    }
}

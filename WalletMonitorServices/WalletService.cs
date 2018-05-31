using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WalletMonitorServices.DTO;

namespace WalletMonitorServices
{
    public class WalletService
    {
        private HttpClient _httpClient;
        public WalletService(HttpClient hc)
        {
            _httpClient = hc;
        }

        /// <summary>
        /// get all wallet addresses by seed
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public async Task<List<WalletDTO>> GetWallets(string seed)
        {
            var json = await _httpClient.GetStringAsync(string.Format("http://monitorapi.ccore.online/api/getaddresses?seed={0}", seed));
            var wallets = JsonConvert.DeserializeObject<List<WalletDTO>>(json);
            return wallets;

        }

        /// <summary>
        /// get donation addresses
        /// </summary>
        /// <returns>donation list</returns>
        public async Task<List<DonationAddressDTO>> GetDonationAddresses()
        {
            var json = await _httpClient.GetStringAsync("http://monitorapi.ccore.online/api/getdonationaddresses");
            var tickers = JsonConvert.DeserializeObject<List<DonationAddressDTO>>(json);
            return tickers;
        }

        /// <summary>
        /// get available coin tickers
        /// </summary>
        /// <returns>tickers list</returns>
        public async Task<List<TickerDTO>> GetAvailabeTickers()
        {
            var json = await _httpClient.GetStringAsync("http://monitorapi.ccore.online/api/gettickers");
            var tickers = JsonConvert.DeserializeObject<List<TickerDTO>>(json);
            return tickers;
        }

        /// <summary>
        /// add new address
        /// </summary>
        /// <returns>tickers list</returns>
        public async Task<WalletDTO> AddNewAddress(string seed, string address, string coinsymbol)
        {
            var json = await _httpClient.GetStringAsync(string.Format("http://monitorapi.ccore.online/api/addaddress?seed={0}&address={1}&coinsymbol={2}", seed, address, coinsymbol));
            var tickers = JsonConvert.DeserializeObject<WalletDTO>(json);
            return tickers;
        }


        /// <summary>
        /// Remove address from seed
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="address"></param>
        /// <param name="coinsymbol"></param>
        /// <returns></returns>
        public async Task<BoolJsonDTO> RemoveAddress(string seed, string address, string coinsymbol)
        {
            var json = await _httpClient.GetStringAsync(string.Format("http://monitorapi.ccore.online/api/deleteaddress?seed={0}&address={1}&coinsymbol={2}", seed, address, coinsymbol));
            var tickers = JsonConvert.DeserializeObject<BoolJsonDTO>(json);
            return tickers;
        }

        /// <summary>
        /// add new address
        /// </summary>
        /// <returns>tickers list</returns>
        public async Task<CurrencyWalletDTO> DetectAddress(string address)
        {
            var json = await _httpClient.GetStringAsync(string.Format("http://monitorapi.ccore.online/api/getsymbol?address={0}", address));
            var tickers = JsonConvert.DeserializeObject<CurrencyWalletDTO>(json);
            return tickers;
        }
    }
}

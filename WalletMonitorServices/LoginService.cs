using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WalletMonitorServices.DTO;

namespace WalletMonitorServices
{
    public class LoginService
    {
        private HttpClient _httpClient;
        public LoginService(HttpClient hc)
        {
            _httpClient = hc;
        }

        /// <summary>
        /// check if seed exists
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public async Task<BoolJsonDTO> GetSeed(string seed)
        {
            var json = await _httpClient.GetStringAsync(string.Format("http://monitorapi.ccore.online/api/getseed?seed={0}", seed));
            var seedResponse = JsonConvert.DeserializeObject<BoolJsonDTO>(json);
            return seedResponse;
        }

        /// <summary>
        /// Get api version
        /// </summary>
        /// <returns>version in string</returns>
        public async Task<VersionDTO> GetVersion()
        {
            var json = await _httpClient.GetStringAsync("http://monitorapi.ccore.online/api/getversion");
            var seed = JsonConvert.DeserializeObject<VersionDTO>(json);
            return seed;
        }

        /// <summary>
        /// Creates new seed
        /// </summary>
        /// <returns>seed in string</returns>
        public async Task<ApiSeedDTO> CreateNewSeed()
        {
            var json = await _httpClient.GetStringAsync("http://monitorapi.ccore.online/api/createnewseed");
            var seed = JsonConvert.DeserializeObject<ApiSeedDTO>(json);
            return seed;
        }
    }
}

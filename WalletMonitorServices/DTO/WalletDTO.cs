using System;
using System.Collections.Generic;
using System.Text;

namespace WalletMonitorServices.DTO
{
    public class WalletDTO
    {
        public string Seed { get; set; }
        public string Address { get; set; }
        public string CoinSymbol { get; set; }
        public string CoinName { get; set; }
        public string ExplorerURL { get; set; }
    }
}

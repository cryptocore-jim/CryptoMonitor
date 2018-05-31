using System;
using System.Collections.Generic;
using System.Text;

namespace WalletMonitorServices.DTO
{
    public class BalanceWalletDTO
    {
        public long LastUpdated { get; set; }
        public string MasternodeStatus { get; set; }
        public decimal Amount { get; set; }
        public decimal PriceBTC { get; set; }
        public decimal PriceUSD { get; set; }
        public decimal PriceEUR { get; set; }
        public decimal Trend1 { get; set; }
        public decimal Trend7 { get; set; }
        public decimal Trend14 { get; set; }
        public bool Degraded { get; set; }
    }
}

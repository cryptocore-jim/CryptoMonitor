using System;
using System.Collections.Generic;
using System.Text;

namespace WalletMonitorServices.DTO
{
    public class TickerDTO
    {
        public string CoinSymbol { get; set; }
        public bool Degraded { get; set; }
        public string CoinSymbolPreview
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(CoinSymbol);
                if (Degraded)
                {
                    sb.Append(" (Degraded)");
                }
                return sb.ToString();
            }
        }
    }
}

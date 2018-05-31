using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletMonitorApp.Helpers
{
    public class CurrencyHelper
    {
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return Convert.ToInt64((TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                     new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds);
        }

        public static Decimal BTCHelperDecimal(Decimal amount)
        {
            return Math.Round(amount, 9);
        }

        public static Decimal FiatHelperDecimal(Decimal amount)
        {
            var dec = Math.Round(amount, 4);
            if (dec < 0)
            {
                return -dec;
            }
            else
            {
                return dec;
            }
        }
    }
}

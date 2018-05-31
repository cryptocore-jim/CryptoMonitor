using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletMonitorApp.Helpers
{
    public class TimeHelper
    {
        public static string ToAge(long timestamp)
        {
            DateTime BlockTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            BlockTime = BlockTime.AddSeconds(timestamp).ToUniversalTime();

            StringBuilder sb = new StringBuilder();
            var days = (DateTime.UtcNow - BlockTime).Days;
            if (days > 0)
            {
                sb.Append(days);
                sb.Append(" d");
            }
            var hours = (DateTime.UtcNow - BlockTime).Hours;
            if (hours > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" ");
                }
                sb.Append(hours);
                sb.Append(" h");
            }
            var min = (DateTime.UtcNow - BlockTime).Minutes;
            if (min > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" ");
                }
                sb.Append(min);
                sb.Append(" min");
            }
            var sec = (DateTime.UtcNow - BlockTime).Seconds;
            if (sec > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" ");
                }
                sb.Append(sec);
                sb.Append(" sec");
            }
            return sb.ToString();

        }
    }
}

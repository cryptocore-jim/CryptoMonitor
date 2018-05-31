using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WalletMonitorApp.Helpers
{
    public class PositiveNegativeConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }
            Decimal val = System.Convert.ToDecimal(value);
            var sb = new StringBuilder();
            if (val == 0)
            {
                return "0.00";
            }
            else if (val > 0)
            {
                sb.Append(string.Format("+{0:0.00}", val));
            }
            else
            {
                sb.Append(string.Format("{0:0.00}", val));
            }
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("StringMetersConverter, ConvertBack not implemented");
        }
    }
}

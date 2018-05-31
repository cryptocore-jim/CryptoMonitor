using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WalletMonitorApp.Helpers
{
    public class BTC8Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }
            Decimal val = System.Convert.ToDecimal(value);
            return string.Format("{0:0.00000000}", val);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("StringMetersConverter, ConvertBack not implemented");
        }
    }
}

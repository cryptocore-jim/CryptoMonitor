using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WalletMonitorApp.Helpers
{
    public class GTEConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 0;
            }
            Decimal val = System.Convert.ToDecimal(value);
            if (val == 0)
            {
                return 0;
            }
            else if (val > 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("StringMetersConverter, ConvertBack not implemented");
        }
    }
}

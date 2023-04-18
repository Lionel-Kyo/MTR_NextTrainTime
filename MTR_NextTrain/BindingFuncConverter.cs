using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTR_NextTrain
{
    public class BindingFuncConverter : IValueConverter
    {
        private Func<object, object> convert;
        private Func<object, object> convertBack;
        public BindingFuncConverter(Func<object, object> convert, Func<object, object> convertBack)
        {
            this.convert = convert;
            this.convertBack = convertBack;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try { return convert(value); }
            catch { return value; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try { return convertBack(value); }
            catch { return value; }
        }
    }
}

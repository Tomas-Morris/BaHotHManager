using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BaHotHManager.Helpers
{
    class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return "";
            if (parameter == null) return value.ToString();
            if (value is DateTime && parameter is string)
            {
                var valueDate = value as DateTime?;
                var parameterString = parameter as string;
                return valueDate.Value.ToString(parameterString);
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var valueString = value as string;
            if (string.IsNullOrWhiteSpace(valueString)) return null;
            if (parameter == null) return DateTime.Parse(valueString);
            if (value is string && parameter is string)
            {
                var parameterString = parameter as string;
                return DateTime.ParseExact(valueString, parameterString, null);
            }
            return DateTime.Parse(value.ToString());
        }
    }
}

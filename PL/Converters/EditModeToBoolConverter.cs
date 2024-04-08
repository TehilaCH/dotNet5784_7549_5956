
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PL.Converters;

internal class EditModeToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int id = (int)value;
        return id == 0 ? true : false;
    }


    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
       
    }

}



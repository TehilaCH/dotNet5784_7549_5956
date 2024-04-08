using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PL.Converters;

class BoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int id = (int)value;
        return id == 0 ? false : true;
    }


    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
    //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //{
    //    if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
    //    {
    //        bool boolValue = (bool)value;
    //        return boolValue ? 1 : 0;
    //    }
    //    else
    //    {
    //        // במקרה שהערך הוא מחרוזת ריקה, נחזיר ערך ברירת המחדל, בדומה למה שהוחזר בהמרתה של bool ל-int.
    //        return 0;
    //    }
    //}

}

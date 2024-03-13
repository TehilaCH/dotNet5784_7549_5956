using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace PL.Converters;

internal class ConvertTimeSpan: IValueConverter
{
    //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //{
    //    return (int)((TimeSpan)value).Days;
    //    //if (value != null && value is TimeSpan)
    //    //{
    //    //    return ((TimeSpan)value).Days;
    //    //}
    //    //return 0; 

    //}
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null && value is TimeSpan)
        {
            return ((int)((TimeSpan)value).TotalDays).ToString(); // המרת הימים למספר שלם והחזרתו כמחרוזת
        }
        return "0"; // ערך ברירת המחדל אם הערך אינו תקין או ריק
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}



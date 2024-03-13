using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PL.Converters;

public class StringToIntConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // ממיר ערך מספרי למחרוזת
        return value?.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // בדיקה האם הערך ריק
        if (string.IsNullOrEmpty((string)value))
        {
            // אם הערך ריק, החזרת ערך מספרי נוסף או ערך מספרי אחר לפי הצורך
            return null;
        }

        // אחרת, המרה לערך מספרי
        return int.Parse((string)value);
    }
}
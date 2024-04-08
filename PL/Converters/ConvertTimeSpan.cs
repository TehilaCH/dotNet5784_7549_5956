using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace PL.Converters;

internal class ConvertTimeSpan : IValueConverter
{
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return 0;
        return (int)((TimeSpan)value).Days;
        
    }
  
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int days;
        if (int.TryParse((string)value, out days))
        {
            return TimeSpan.FromDays(days);
        }
        return DependencyProperty.UnsetValue;
    }
 

}


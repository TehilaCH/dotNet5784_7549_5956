using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL;

/// <summary>
/// Interaction logic for GanttChartWindow.xaml
/// </summary>
public partial class GanttChartWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get;

    public GanttChartWindow()
    {
        InitializeComponent();

        foreach (var task in s_bl.Task.ReadAll())
        {
           // AddTaskToGrid(task);
        }
    }
    //private void AddTaskToGrid(Task task)
    //{
    //    // יצירת מלבן המייצג את המשימה
    //    Rectangle rect = new Rectangle
    //    {
    //        Width = task.Duration.TotalDays * 20, // קביעת הרוחב של המלבן בהתאם למשך המשימה (20 פיקסל ליום)
    //        Height = 50, // קביעת הגובה של המלבן
    //        Fill = task.IsDelayed ? Brushes.Red : Brushes.Blue, // קביעת הצבע של המלבן בהתאם לסטטוס המשימה
    //        Margin = new Thickness((task.StartDate - new DateTime(2024, 3, 1)).TotalDays * 20, 0, 0, 0) // קביעת המרווח של המלבן בהתאם לתאריך התחלת המשימה
    //    };

    //    // יצירת תווית המכילה את שם המשימה וה-ID שלה
    //    Label label = new Label
    //    {
    //        Content = $"{task.Name} (ID: {task.ID})",
    //        HorizontalAlignment = HorizontalAlignment.Center,
    //        VerticalAlignment = VerticalAlignment.Center,
    //        Foreground = Brushes.White // קביעת צבע הטקסט ללבן
    //    };

    //    // הוספת המלבן והתווית לגריד
    //    Grid.Children.Add(rect);
    //    Grid.Children.Add(label);

    //    // הגדרת מיקום התווית בתוך המלבן
    //    Grid.SetRow(label, 1);
    //    Grid.SetColumn(label, Grid.GetColumn(rect));
    //}
}


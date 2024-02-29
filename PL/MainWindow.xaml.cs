using BlApi;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get;

    public MainWindow()
    {
        InitializeComponent();
    }

    
   

    private void btnDirector_Click(object sender, RoutedEventArgs e)
    {
        new DirectorMainWindow().Show();

    }

    private void btnEngineer_Click(object sender, RoutedEventArgs e)
    {
       // new EngineerMainWindow().show();
    }

    private void InitializeTimeButton_Click(object sender, RoutedEventArgs e)
    {
        s_bl.InitializeTime();
    }

    private void AdvanceDayButton_Click(object sender, RoutedEventArgs e)
    {
        s_bl.AdvanceDay();
    }

    private void AdvanceHourButton_Click(object sender, RoutedEventArgs e)
    {
        s_bl.AdvanceHour();
    }

    private void AdvanceYearButton_Click(object sender, RoutedEventArgs e)
    {
        s_bl.AdvanceYear();
    }
}
//private void btnEngineer_Click(object sender, RoutedEventArgs e)
//{
//    new Engineer.EngineerListWindow().Show();
//}

//private void btnInitializeDB_Click(object sender, RoutedEventArgs e)
//{
//    MessageBoxResult result = MessageBox.Show("Are you sure you want to initialize the database?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
//    if (result == MessageBoxResult.Yes)
//    {
//        // Call the initialization method
//        BlApi.IBl.ResetDB();
//        BlApi.IBl.InitializeDB();


//    }
//}

//private void btnResetDB_Click(object sender, RoutedEventArgs e)
//{
//    // Call the reset method

//    MessageBoxResult result = MessageBox.Show("Are you sure you want to reset the database?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
//    if (result == MessageBoxResult.Yes)
//    {
//        BlApi.IBl.ResetDB();
//    }

//}

//private void btnTask_Click(object sender, RoutedEventArgs e)
//{
//    new Task.TaskListWindow().Show();

//}

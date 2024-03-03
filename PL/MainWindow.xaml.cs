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

    public static int Id { get; set; }
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
      //  new EngineerMainWindow(Id).Show();
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

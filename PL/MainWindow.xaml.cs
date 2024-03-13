using BlApi;
using BO;
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
        //CurrentTime = s_bl.Clock;
        UpdateClock();
    }

    public int IdEngineer
    {
        get { return (int)GetValue(DependenciesProperty); }
        set { SetValue(DependenciesProperty, value); }
    }

    public static readonly DependencyProperty DependenciesProperty =
    DependencyProperty.Register("IdEngineer", typeof(int), typeof(MainWindow), new PropertyMetadata(null));



    public DateTime CurrentTime
    {
        get { return (DateTime)GetValue(CurrentTimeProperty); }
        set { SetValue(CurrentTimeProperty, value); }
    }

    public static readonly DependencyProperty CurrentTimeProperty =
    DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));

    private void btnDirector_Click(object sender, RoutedEventArgs e)
    {
        new DirectorMainWindow().Show();

    }

  
    private void btnEngineer_Click(object sender, RoutedEventArgs e)
    {
        int id;
        if (int.TryParse(IdEngineer.ToString(), out id)) // מנסה להמיר את הטקסט למספר שלם
        {
            try
            {
                s_bl.Engineer.Read(id);
                new EngineerMainWindow(id).Show();
            }
            catch (BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        else
        {
            MessageBox.Show("Please enter a valid integer for Engineer ID.");
        }
    }
    private void AdvanceDayButton_Click(object sender, RoutedEventArgs e)
    {
        s_bl.AdvanceDay();
        UpdateClock();
    }

    private void AdvanceHourButton_Click(object sender, RoutedEventArgs e)
    {
        s_bl.AdvanceHour();
        UpdateClock();
    }

    private void AdvanceYearButton_Click(object sender, RoutedEventArgs e)
    {
        s_bl.AdvanceYear();
        UpdateClock();
    }

    private void UpdateClock()
    {
        CurrentTime = s_bl.Clock;
    }


    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        foreach (char c in e.Text)
        {
            if (!char.IsDigit(c))
            {
                e.Handled = true; // מונע את הקלט אם לא מספרי
                return;
            }
        }
    }

    private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Space)
        {
            e.Handled = true; // מונע את הקלט אם המקש הוא רווח
        }
    }

}

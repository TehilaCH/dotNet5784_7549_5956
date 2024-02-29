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
/// Interaction logic for DirectorMainWindow.xaml
/// </summary>
public partial class DirectorMainWindow : Window
{
    public DirectorMainWindow()
    {
        InitializeComponent();
    }


    private void btnEngineer_Click(object sender, RoutedEventArgs e)
    {
        new Engineer.EngineerListWindow().Show();
    }

    private void btnInitializeDB_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("Are you sure you want to initialize the database?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            // Call the initialization method
            BlApi.IBl.ResetDB();
            BlApi.IBl.InitializeDB();


        }
    }

    private void btnResetDB_Click(object sender, RoutedEventArgs e)
    {
        // Call the reset method

        MessageBoxResult result = MessageBox.Show("Are you sure you want to reset the database?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            BlApi.IBl.ResetDB();
        }

    }

    private void btnTask_Click(object sender, RoutedEventArgs e)
    {
        new Task.TaskListWindow().Show();

    }
}
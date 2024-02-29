using BO;
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
/// Interaction logic for DependentWindow.xaml
/// </summary>
public partial class DependentWindow : Window
{
    //public DependentWindow()
    //{
    //    InitializeComponent();
    //}
    public List<TaskInList> Dependencies { get; set; }

    public DependentWindow(List<TaskInList> dependencies)
    {
        InitializeComponent();
        Dependencies = dependencies;
        DataContext = this;
    }

    private void btnAddDependency_Click(object sender, RoutedEventArgs e)
    {

    }
}

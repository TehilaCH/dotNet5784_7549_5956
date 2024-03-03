using BO;
using DO;
using PL.Task;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

    private static readonly BlApi.IBl s_bl = BlApi.Factory.Get;

    public DependentWindow(BO.Task task)
    {
        InitializeComponent();
        Dep = new Dependence();
        Dependencies = task.Dependencies;
        IdT = task.Id;
         DataContext = this;
    }

    public List<BO.TaskInList> Dependencies
    {
        get { return (List<BO.TaskInList>)GetValue(DependenciesProperty); }
        set { SetValue(DependenciesProperty, value); }
    }

    public static readonly DependencyProperty DependenciesProperty =
    DependencyProperty.Register("Dependencies", typeof(List<BO.TaskInList>), typeof(DependentWindow), new PropertyMetadata(null));

    public DO.Dependence Dep
    {
        get { return (DO.Dependence)GetValue(DepProperty); }
        set { SetValue(DepProperty, value); }
    }

    public static readonly DependencyProperty DepProperty =
    DependencyProperty.Register("Dep", typeof(DO.Dependence), typeof(DependentWindow), new PropertyMetadata(null));

    public int IdT { get; set; }
    private void btnAddDependency_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            s_bl.depAdd((int)Dep.IdPreviousTask!, (int)Dep.IdPendingTask!);
            var taskNew = s_bl.Task.Read(IdT);
            Dependencies = taskNew.Dependencies;
            DataContext = this;
        }
         catch (BlInvalidValueException ex)
        {

            MessageBox.Show(ex.Message);
        }
        catch (Exception ex)
        {

            MessageBox.Show("An error occurred: " + ex.Message);
        }


    }

    private void btnDeleteDependency_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            s_bl.Deletedep((int)Dep.IdPreviousTask!, (int)Dep.IdPendingTask!);
            var taskNew = s_bl.Task.Read(IdT);
            Dependencies = taskNew.Dependencies;
            DataContext = this;
        }
        catch (BlInvalidValueException ex)
        {

            MessageBox.Show(ex.Message);
        }
        catch (Exception ex)
        {

            MessageBox.Show("An error occurred: " + ex.Message);
        }
    }
}


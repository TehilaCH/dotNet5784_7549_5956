using BO;
using PL.Task;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
/// Interaction logic for EngineerMainWindow.xaml
/// </summary>
public partial class EngineerMainWindow : Window
{

    private static readonly BlApi.IBl s_bl = BlApi.Factory.Get;

    public EngineerMainWindow(int idE)
    {
        InitializeComponent();
        // Load suggested tasks for the engineer
        SuggestedTaskList = new ObservableCollection<BO.Task>(s_bl.listTaskForEngineer(idE));

        // Set the DataContext
        DataContext = this;
        BO.Engineer engineer = s_bl.Engineer.Read(idE);

        if(engineer.Task != null)
        {
            idTask = engineer.Task.Id;

        }
        else { idTask = null; }

    }
    public int? idTask {get; set;}
    public ObservableCollection<BO.Task> SuggestedTaskList
    {
        get { return (ObservableCollection<BO.Task>)GetValue(DependenciesProperty); }
        set { SetValue(DependenciesProperty, value); }
    }

    public static readonly DependencyProperty DependenciesProperty =
    DependencyProperty.Register("SuggestedTaskList", typeof(ObservableCollection<BO.Task>), typeof(EngineerMainWindow), new PropertyMetadata(null));

   
    private void addOrUpdateNewItem(int id, bool isUpdate)//Refreshment
    {
        var task = s_bl.Task.Read(id);

        if (isUpdate)
        {
            SuggestedTaskList = new ObservableCollection<BO.Task>(SuggestedTaskList.Where(e => e.Id != id));
        }
        SuggestedTaskList.Add(task);
    }

    private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        //Get the selected item in the DataGrid
        var selectedRow = sender as DataGridRow;
        if (selectedRow != null && selectedRow.Item is BO.Task)
        {
            BO.Task selectedTask = selectedRow.Item as BO.Task;

            // Create a single item view window in update mode
            TaskWindow taskWindow = new TaskWindow(addOrUpdateNewItem, selectedTask.Id); // Update mode parameter
            taskWindow.ShowDialog(); // Opening the window in dialog mode
        }
    }

    private void btncurrentTask_Click(object sender, RoutedEventArgs e)
    {
        if (idTask != null)
        {
            TaskWindow taskWindow = new TaskWindow(addOrUpdateNewItem, (int)idTask ,false);
            taskWindow.ShowDialog();
        }
        else
        {
            MessageBox.Show("Engineer does not have a task that belongs to him");
        }

    }
    public DateTime? StartDateTask
    {
        get { return (DateTime)GetValue(StartDateTaskProperty); }
        set { SetValue(StartDateTaskProperty, value); }
    }

    public static readonly DependencyProperty StartDateTaskProperty =
    DependencyProperty.Register("StartDateTask", typeof(DateTime), typeof(EngineerMainWindow), new PropertyMetadata(null));


    public DateTime? EndDateTask
    {
        get { return (DateTime)GetValue(EndDateTaskProperty); }
        set { SetValue(EndDateTaskProperty, value); }
    }
    public static readonly DependencyProperty EndDateTaskProperty =
    DependencyProperty.Register("EndDateTask", typeof(DateTime), typeof(EngineerMainWindow), new PropertyMetadata(null));



    private void updatTaskDates_Click(object sender, RoutedEventArgs e)
    {
        try 
        {
            if (idTask != null)
            {
                s_bl.Task.UpdateStartAndEndDate(StartDateTask,EndDateTask,(int)idTask);
                MessageBox.Show("Date updated successfully!");
            }
            else
            {
              MessageBox.Show("Unable to update dates There is no current task");
            }
        }
        catch (BlDoesNotExistException ex)
        {
            MessageBox.Show(ex.Message);
            StartDateTask = DateTime.MinValue;
            EndDateTask= DateTime.MinValue;
        }
        catch (Exception ex)
        {
          MessageBox.Show("An error occurred: " + ex.Message);
        }

    }
}

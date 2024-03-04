using BO;
using PL.Engineer;
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

namespace PL.Task;

/// <summary>
/// Interaction logic for TaskListWindow.xaml
/// </summary>
public partial class TaskListWindow : Window
{
    private static readonly BlApi.IBl s_bl = BlApi.Factory.Get;
    public TaskListWindow()//A window displays a list of tasks
    {
        InitializeComponent();
        TaskList = new ObservableCollection<BO.Task>(s_bl?.Task.ReadAll()!);
        
    }

    public ObservableCollection<BO.Task> TaskList//An object that will contain the list
    {
        get { return (ObservableCollection<BO.Task>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }

    public static readonly DependencyProperty TaskListProperty =
    DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.Task>), typeof(TaskListWindow), new PropertyMetadata(null));

    public BO.EngineerLevel level { get; set; } = BO.EngineerLevel.All;

    private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)//An event to filter engineers by engineer level
    {
            TaskList =
            new ObservableCollection<BO.Task>((level == BO.EngineerLevel.All) ? s_bl?.Task.ReadAll()! :
            s_bl?.Task.ReadAll(item => item.TaskLave == level)!);
    }
    
    private void btnAdd_Click(object sender, RoutedEventArgs e)//Add click event
    {
        TaskWindow taskWindow = new TaskWindow(addOrUpdateNewItem);
        taskWindow.ShowDialog();
    }
    private void addOrUpdateNewItem(int id, bool isUpdate)//Refreshment
    {
        try
        {
            var task = s_bl.Task.Read(id);

            if (isUpdate)
            {
                TaskList = new ObservableCollection<BO.Task>(TaskList.Where(e => e.Id != id));
            }
            TaskList.Add(task);
        }
        catch (BlDoesNotExistException)
        {
            TaskList = new ObservableCollection<BO.Task>(s_bl?.Task.ReadAll()!);
        }
        catch(Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}");
        }

    }


    private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)//Double click event to update
    {
        //Get the selected item in the list
        BO.Task? SelectedTask = (sender as ListView)?.SelectedItem as BO.Task;

        if (SelectedTask != null)
        {
            // Create a single item view window in update mode
            TaskWindow taskWindow = new TaskWindow(addOrUpdateNewItem, SelectedTask.Id); // Update mode parameter
            taskWindow.ShowDialog(); // Opening the window in dialog mode
        }
    }


}

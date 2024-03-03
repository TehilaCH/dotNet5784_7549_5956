using BO;
using PL.Engineer;
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

namespace PL.Task;

/// <summary>
/// Interaction logic for TaskWindow.xaml
/// </summary>
public partial class TaskWindow : Window
{
   
    private static readonly BlApi.IBl s_bl = BlApi.Factory.Get;


    public static readonly DependencyProperty TaskProperty = DependencyProperty.Register(
    nameof(Task),
     typeof(BO.Task),
     typeof(TaskWindow));

    private event Action<int, bool> _addOrUpdateNewItem;

    public BO.Task Task
    {
        get { return (BO.Task)GetValue(TaskProperty); }
        set { SetValue(TaskProperty, value); }
    }

    public TaskWindow(Action<int, bool> addOrUpdateNewItem, int Id = 0)//A constructor with a parameter
    {
        InitializeComponent();
        _addOrUpdateNewItem = addOrUpdateNewItem;
       
        if (Id == 0)
        {
            Task = new BO.Task(); //Creation to add
        }
        else
        {
            try
            {
                Task = s_bl.Task.Read(Id);//Call for update
                Task.Engineer = new EngineerInTask();
               
            }
            catch (BlDoesNotExistException ex)
            {

                MessageBox.Show("The entity does not exist.");
            }
            catch (Exception ex)
            {

                MessageBox.Show("An error occurred: " + ex.Message);
            }

        }
    }



    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)//Add/Update click event
    {
        try
        {
            var button = sender as Button;
            if (button?.Content is "Add")
            {
                // Adding a new entity
               int id= s_bl.Task.Creat(Task);
                _addOrUpdateNewItem(id, true);
                MessageBox.Show("Task added successfully!");
            }
            else
            {
                // Update an existing entity
                s_bl.Task.Update(Task);
                _addOrUpdateNewItem(Task.Id, true);

                MessageBox.Show("Task updated successfully!");
            }

            // Close the window
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}");
        }
    }

    public List<BO.TaskInList> TaskDependencies { get; set; }

    private void btnDependent_Click(object sender, RoutedEventArgs e)
    {
        DependentWindow dependenciesWindow = new DependentWindow(Task);
        dependenciesWindow.ShowDialog();
    }
}

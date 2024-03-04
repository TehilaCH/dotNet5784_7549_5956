using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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

namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerWindow.xaml
/// </summary>
public partial class EngineerWindow : Window
{
    private static readonly BlApi.IBl s_bl = BlApi.Factory.Get;
   

    public static readonly DependencyProperty EngineerProperty = DependencyProperty.Register(
    nameof(Engineer),
     typeof(BO.Engineer),
     typeof(EngineerWindow));

    private event Action<int, bool> _addOrUpdateNewItem;

    public BO.Engineer Engineer
    {
        get { return (BO.Engineer)GetValue(EngineerProperty); }
        set { SetValue(EngineerProperty, value); }
    }

    public EngineerWindow(Action<int, bool> addOrUpdateNewItem, int Id = 0)//A constructor with a parameter
    {
        InitializeComponent();
        _addOrUpdateNewItem = addOrUpdateNewItem;
        if (Id == 0)
        {
            Engineer = new BO.Engineer(); //Creation to add
            
        }
        else
        {
            try
            {
                Engineer = s_bl.Engineer.Read(Id);//Call for update
                if(Engineer.Task==null)
                {
                    Engineer.Task = new TaskInEngineer();

                }

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
                s_bl.Engineer.Creat(Engineer);
                _addOrUpdateNewItem(Engineer.Id, false);
                MessageBox.Show("Engineer added successfully!");
            }
            else
            {
                // Update an existing entity
               
                s_bl.Engineer.Update(Engineer);
                _addOrUpdateNewItem(Engineer.Id, true);

                MessageBox.Show("Engineer updated successfully!");
            }

            // Close the window
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}");
        }
    }


    private void btnDeleteEngineer_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            s_bl.Engineer.Delete(Engineer.Id);
            _addOrUpdateNewItem(Engineer.Id, false);
            MessageBox.Show("Engineer delete successfully!");
            Close();
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
}

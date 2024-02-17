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

    public EngineerWindow(Action<int, bool> addOrUpdateNewItem, int Id = 0)
    {
        InitializeComponent();
        _addOrUpdateNewItem = addOrUpdateNewItem;
        if (Id == 0)
        {
            Engineer = new BO.Engineer(); 
        }
        else
        {
            try
            {
                Engineer = s_bl.Engineer.Read(Id);
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

   

    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var button = sender as Button;
            if (button?.Content is "Add")
            {
                // הוספת ישות חדשה
                s_bl.Engineer.Creat(Engineer);
                _addOrUpdateNewItem(Engineer.Id, false);
                MessageBox.Show("Engineer added successfully!");
            }
            else
            {
                // עדכון ישות קיימת
                s_bl.Engineer.Update(Engineer);
                _addOrUpdateNewItem(Engineer.Id, true);

                MessageBox.Show("Engineer updated successfully!");
            }

            // סגירת החלון
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}");
        }
    }
   
}

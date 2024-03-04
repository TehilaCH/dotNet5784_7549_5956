using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
//using BlApi;

namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>

public partial class EngineerListWindow : Window
{
    private static readonly BlApi.IBl s_bl = BlApi.Factory.Get;
    public EngineerListWindow()//A window displays a list of engineers
    {
        InitializeComponent();
        EngineerList = new ObservableCollection<BO.Engineer>( s_bl?.Engineer.ReadAll()!);
    }

    public ObservableCollection<BO.Engineer> EngineerList//An object that will contain the list
    {
        get { return (ObservableCollection<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }

    public static readonly DependencyProperty EngineerListProperty =
    DependencyProperty.Register("EngineerList", typeof(ObservableCollection<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

    public BO.EngineerLevel level { get; set; } = BO.EngineerLevel.All;

    private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)//An event to filter engineers by engineer level
    {
        EngineerList =
            new ObservableCollection<BO.Engineer>((level == BO.EngineerLevel.All) ?  s_bl?.Engineer.ReadAll()! : 
            s_bl?.Engineer.ReadAll(item => item.Level== level)!);
    }

    private void btnAdd_Click(object sender, RoutedEventArgs e)//Add click event
    {
        EngineerWindow engineerWindow = new EngineerWindow(addOrUpdateNewItem);
        engineerWindow.ShowDialog();
    }
    private void addOrUpdateNewItem(int id, bool isUpdate)//Refreshment
    {
        try
        {
            var engineer = s_bl.Engineer.Read(id);
            if (isUpdate)
            {
                EngineerList = new ObservableCollection<BO.Engineer>(EngineerList.Where(e => e.Id != id));
            }
            EngineerList.Add(engineer);
        }
        catch (BlDoesNotExistException) 
        {
            EngineerList = new ObservableCollection<BO.Engineer>(s_bl?.Engineer.ReadAll()!);
        }
        catch(Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}");
        }
       
    }


    private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)//Double click event to update
    {
        //Get the selected item in the list
        BO.Engineer? SelectedEngineer = (sender as ListView)?.SelectedItem as BO.Engineer;

        if (SelectedEngineer != null)
        {
            // Create a single item view window in update mode
            EngineerWindow engineerWindow = new EngineerWindow(addOrUpdateNewItem, SelectedEngineer.Id); // Update mode parameter
            engineerWindow.ShowDialog(); // Opening the window in dialog mode
        }
    }
}

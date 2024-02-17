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
//using BlApi;

namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>

public partial class EngineerListWindow : Window
{
    private static readonly BlApi.IBl s_bl = BlApi.Factory.Get;
    public EngineerListWindow()
    {
        InitializeComponent();
        EngineerList = new ObservableCollection<BO.Engineer>( s_bl?.Engineer.ReadAll()!);
    }

    public ObservableCollection<BO.Engineer> EngineerList
    {
        get { return (ObservableCollection<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }

    public static readonly DependencyProperty EngineerListProperty =
    DependencyProperty.Register("EngineerList", typeof(ObservableCollection<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

    public BO.EngineerLevel level { get; set; } = BO.EngineerLevel.All;

    private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        EngineerList =
            new ObservableCollection<BO.Engineer>((level == BO.EngineerLevel.All) ?  s_bl?.Engineer.ReadAll()! : 
            s_bl?.Engineer.ReadAll(item => item.Level== level)!);
    }

    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
        EngineerWindow engineerWindow = new EngineerWindow(addOrUpdateNewItem);
        engineerWindow.ShowDialog();
    }
    private void addOrUpdateNewItem(int id, bool isUpdate)
    {
        var engineer = s_bl.Engineer.Read(id);

        if (isUpdate)
        {
            EngineerList = new ObservableCollection<BO.Engineer>(EngineerList.Where(e => e.Id != id));
        }
        EngineerList.Add(engineer);
    }


    private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        // קח את הפריט הנבחר ברשימה
        BO.Engineer? SelectedEngineer = (sender as ListView)?.SelectedItem as BO.Engineer;

        if (SelectedEngineer != null)
        {
            // יצירת חלון תצוגת פריט בודד במצב עדכון
            EngineerWindow engineerWindow = new EngineerWindow(addOrUpdateNewItem, SelectedEngineer.Id); // פרמטר האידישן במצב עדכון
            engineerWindow.ShowDialog(); // פתיחת החלון במצב דיאלוגי
        }
    }
}

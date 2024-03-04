using BO;
using PL.Engineer;
using PL.Task;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for ScheduleWindow.xaml
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get;

        public ScheduleWindow()
        {
            InitializeComponent();
        }

        private void btnEngineers_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().ShowDialog();

            //EngineerListWindow engineerListWindow = new EngineerListWindow();
            //engineerListWindow.ShowDialog();
        }

        private void btnDependent_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnTasks_Click(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().ShowDialog();

            //TaskListWindow taskListWindow = new TaskListWindow();
            //taskListWindow.ShowDialog();
        }

        public int ID
        {
            get { return (int)GetValue(IDProperty); }
            set { SetValue(IDProperty, value); }
        }

        public static readonly DependencyProperty IDProperty =
        DependencyProperty.Register("ID", typeof(int), typeof(ScheduleWindow), new PropertyMetadata(null));

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly DependencyProperty DateProperty =
        DependencyProperty.Register("Date", typeof(DateTime), typeof(ScheduleWindow), new PropertyMetadata(null));


        public DateTime StartDateProject
        {
            get { return (DateTime)GetValue(StartDateProjectProperty); }
            set { SetValue(StartDateProjectProperty, value); }
        }

        public static readonly DependencyProperty StartDateProjectProperty =
        DependencyProperty.Register("StartDateProject", typeof(DateTime), typeof(ScheduleWindow), new PropertyMetadata(null));


        public DateTime EndDateProject
        {
            get { return (DateTime)GetValue(EndDateProjectProjectProperty); }
            set { SetValue(EndDateProjectProjectProperty, value); }
        }

        public static readonly DependencyProperty EndDateProjectProjectProperty =
        DependencyProperty.Register("EndDateProject", typeof(DateTime), typeof(ScheduleWindow), new PropertyMetadata(null));

        private void btnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Task.Delete(ID);
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

        private void btnDeleteEngineer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Engineer.Delete(ID);
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

        private void btnProjectStartDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime? d = s_bl.Schedule.SetStartProjectDate(StartDateProject);
                if (d == null)
                {
                    MessageBox.Show("A project start date already exists");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnProjectEndDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime? d = s_bl.Schedule.SetEndProjectDate(EndDateProject);
                if (d == null)
                {
                    MessageBox.Show("A project end date already exists");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnUpdateDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Task.UpdateStartDate(ID, Date);

            }
            catch (BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
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
}

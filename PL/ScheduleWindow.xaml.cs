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
            if (s_bl.Schedule.getStartProjectDate() != null)
            {
                StartDateProject = (DateTime)s_bl.Schedule.getStartProjectDate()!;
            }

            if (s_bl.Schedule.getEndProjectDate() != null)
            {
                EndDateProject = (DateTime)s_bl.Schedule.getEndProjectDate()!;
            }



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

      

        private void btnProjectStartDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (s_bl.Schedule.getStartProjectDate() != null)
                {
                    MessageBox.Show("A project start date already exists");

                    StartDateProject = (DateTime)s_bl.Schedule.getStartProjectDate()!;
                    return; 
                }
                s_bl.Schedule.SetStartProjectDate((DateTime)StartDateProject!);
                MessageBox.Show("Project start date updated successfully");
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
                if(s_bl.Schedule.getEndProjectDate() != null)
                {
                    MessageBox.Show("A project end date already exists");
                    return;
                }
                s_bl.Schedule.SetEndProjectDate((DateTime)EndDateProject!);
                MessageBox.Show("Project End date updated successfully");
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
                MessageBox.Show($"updated date for Task {ID} successfully");
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

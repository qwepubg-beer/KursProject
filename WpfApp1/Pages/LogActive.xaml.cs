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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.res;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для LogActive.xaml
    /// </summary>
    public partial class LogActive : Page
    {
        ObservableCollection<Activity> ActivityList { get; set; }
        Activity activity = null;
        public LogActive()
        {
            InitializeComponent();
            LoadData();
        }

        void LoadData()
        {
            ActivityList = new ObservableCollection<Activity>(Core.kursEntities.Activity.ToList());
            ActivityListBox.ItemsSource = ActivityList;
            ChoosingActivty.Text = "Выбрано: ";
        }
        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ActivityListBox.ItemsSource = Core.kursEntities.Foods.Where(a => a.Name.Contains(TextSearch.Text)).ToList();
        }
        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            TextSearch.Text = "";
            SortComboBox.SelectedItem = null;
            LoadData();
        }

        private void ChooseFoodBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ActivityListBox.SelectedItem != null)
            {
                activity = ActivityListBox.SelectedItem as Activity;
                ChoosingActivty.Text = "Выбрано: " + activity.Name;
            }
        }

        private void RecordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(Quantity.Text, out int Q))
            {
                UsersActivity newUsActv = new UsersActivity()
                {
                    UserID = Core.user.UserId,
                    ActivityID = activity.Id,
                    TimeMin = Q,
                    Date = DateTime.Now
                };
                Core.kursEntities.UsersActivity.Add(newUsActv);
                Core.kursEntities.SaveChanges();
                MessageBox.Show("Данные сохранены!");
            }
            else
            {
                MessageBox.Show("Неверный формат колличества минут!");
            }
        }
        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (SortComboBox.SelectedIndex)
            {
                case 0:
                    ActivityListBox.ItemsSource = ActivityList.OrderBy(b => b.MET).ToList();
                    break;
                case 1:
                    ActivityListBox.ItemsSource = ActivityList.OrderByDescending(b => b.MET).ToList();
                    break;
                default:
                    break;
            }
        }
    }
}

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
    /// Логика взаимодействия для LogMealPage.xaml
    /// </summary>
    public partial class LogMealPage : Page
    {
        ObservableCollection<Foods> FoodList { get; set; }
        Foods food=null;
        public LogMealPage()
        {
            InitializeComponent();
            LoadData();
            TypeProductComboBox.ItemsSource = Core.kursEntities.FoodCategories.ToList();
        }
        void LoadData()
        {
            FoodList = new ObservableCollection<Foods>(Core.kursEntities.Foods.ToList());
            ProductList.ItemsSource = FoodList;
            ChoosingProduct.Text = "Выбрано: ";
        }
        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ProductList.ItemsSource = Core.kursEntities.Foods.Where(a => a.Name.Contains(TextSearch.Text)).ToList();
        }
        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            TextSearch.Text = "";
            TypeProductComboBox.SelectedItem = null;
            SortComboBox.SelectedItem = null;
            LoadData();
        }

        private void ChooseFoodBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ProductList.SelectedItem != null)
            { 
                food = (Foods)ProductList.SelectedItem;
                ChoosingProduct.Text = "Выбрано: "+food.Name;
            }
        }

        private void RecordBtn_Click(object sender, RoutedEventArgs e)
        {
            //записать в таблицу
        }
        private void TypeProductComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TypeProductComboBox.SelectedItem != null)
            {
                FoodCategories g = TypeProductComboBox.SelectedItem as FoodCategories;
                ProductList.ItemsSource = FoodList.Where(b => b.CategoryId == g.CategoryId).ToList();
            }
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (SortComboBox.SelectedIndex)
            {
                case 0:
                    ProductList.ItemsSource = FoodList.OrderBy(b => b.Calories).ToList();
                    break;
                case 1:
                    ProductList.ItemsSource = FoodList.OrderByDescending(b => b.Calories).ToList();
                    break;
                case 2:
                    ProductList.ItemsSource = FoodList.OrderBy(b => b.Name).ToList();
                    break;
                default:
                    break;
            }
        }
    }
}

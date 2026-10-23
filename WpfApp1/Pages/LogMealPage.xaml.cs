using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
        Meal newMeal = null;
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
        void CreateMeal()
        {
            Meal meal = new Meal()
            {
                ClientID = Core.user.UserId,
                Date =DateTime.Now
            };
            Core.kursEntities.Meal.Add(meal);
            Core.kursEntities.SaveChanges();
            newMeal=Core.kursEntities.Meal.First(a=> a.ClientID == meal.ClientID);
        }
        void CreateMealFood()
        {
            if (int.TryParse(Quantity.Text, out int Q))
            {
                Meal_Foods mealfood = new Meal_Foods()
                {
                    MealID = newMeal.MealId,
                    FoodID = food.FoodId,
                    WeightInG = Q
                };
                Core.kursEntities.Meal_Foods.Add(mealfood);
                Core.kursEntities.SaveChanges();
            }
            else
            {
                MessageBox.Show("Неверный формат колличества грамм!");
            }
        }
        private void RecordBtn_Click(object sender, RoutedEventArgs e)
        {
            if(newMeal==null)
            {
                CreateMeal();
                CreateMealFood();
            }
            else
            {
                CreateMealFood();
            }

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

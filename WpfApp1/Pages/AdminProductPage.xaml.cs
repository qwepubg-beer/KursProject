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
using WpfApp1.Functions;
using WpfApp1.res;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminProductPage.xaml
    /// </summary>
    public partial class AdminProductPage : Page,ITrackChanges
    {
        public ObservableCollection<Foods> FoodList { get; set; }
        public List<FoodCategories> TypeFoodList { get; set; }
        private bool _isDirty = false;
        public AdminProductPage()
        {
            InitializeComponent();
            LoadData();
            DataContext = this;
            ProductDataGrid.CellEditEnding += ProductDataGrid_CellEditEnding;
        }
        private void LoadData()
        {
            TypeFoodList = Core.kursEntities.FoodCategories.ToList();
            FoodList = new ObservableCollection<Foods>(Core.kursEntities.Foods.ToList());
            ProductDataGrid.ItemsSource = FoodList;
            _isDirty = false;
        }
        public bool HasUnsavedChanges()
        {
            return _isDirty;
        }
        private void ProductDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                _isDirty = true;
            }
        }
        public int ReturnCategoryID(int id)
        {
            FoodCategories foodCategory = TypeFoodList.FirstOrDefault(r => r.CategoryId == id);
            return foodCategory != null ? foodCategory.CategoryId : 1;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ProductDataGrid.CommitEdit();
            var result = MessageBox.Show("Сохранить все изменения?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (var food in FoodList)
                    {
                        if (food.FoodId == 0)
                        {
                            var newFood = new Foods
                            {
                                Name = food.Name,
                                Calories = food.Calories,
                                ProteinG = food.ProteinG,
                                FatG= food.FatG,
                                CarbsG=food.CarbsG,
                                FiberG=food.FiberG,
                                IsActive = food.IsActive,
                                CategoryId = ReturnCategoryID(food.CategoryId)
                            };
                            Core.kursEntities.Foods.Add(newFood);
                        }
                        else
                        {
                            var existingFood = Core.kursEntities.Foods.Find(food.FoodId);
                            if (existingFood != null)
                            {
                                existingFood.Name = food.Name;
                                existingFood.Calories = food.Calories;
                                existingFood.ProteinG = food.ProteinG;
                                existingFood.FatG = food.FatG;
                                existingFood.CarbsG = food.CarbsG;
                                existingFood.FiberG = food.FiberG;
                                existingFood.IsActive = food.IsActive;
                                existingFood.CategoryId = ReturnCategoryID(food.CategoryId);
                            }
                        }
                    }
                    Core.kursEntities.SaveChanges();
                    LoadData();
                    MessageBox.Show("Сохранено успешно!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var newFood = new Foods
            {
                Name = "",
                Calories = 0,
                ProteinG = 0,
                FatG = 0,
                CarbsG = 0,
                FiberG = null,
                IsActive = true,
               CategoryId = TypeFoodList.FirstOrDefault()?.CategoryId ?? 0

            };
            FoodList.Add(newFood);
            ProductDataGrid.SelectedItem = newFood;
            ProductDataGrid.ScrollIntoView(newFood);
        }
    }
}

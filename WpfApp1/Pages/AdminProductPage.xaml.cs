using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
    public partial class AdminProductPage : Page, ITrackChanges
    {
        public ObservableCollection<Foods> FoodList { get; set; }
        public List<FoodCategories> TypeFoodList { get; set; }
        private bool _isDirty = false;

        public AdminProductPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            TypeFoodList = Core.kursEntities.FoodCategories.ToList();
            var dbFoods = Core.kursEntities.Foods.ToList();
            foreach (var food in dbFoods)
            {
                food.CategoryId = ConvertDbIdToComboIndex(food.CategoryId);
                if (food is INotifyPropertyChanged npc)
                {
                    npc.PropertyChanged -= Food_PropertyChanged;
                    npc.PropertyChanged += Food_PropertyChanged;
                }
            }
            FoodList = new ObservableCollection<Foods>(dbFoods);
            FoodList.CollectionChanged += FoodList_CollectionChanged;
            this.DataContext = this;
            ProductDataGrid.ItemsSource = FoodList;
            _isDirty = false;
        }
        public bool HasUnsavedChanges()
        {
            return _isDirty;
        }

        private void FoodList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _isDirty = true;
            if (e.NewItems != null)
            {
                foreach (Foods newFood in e.NewItems)
                {
                    if (newFood is INotifyPropertyChanged npc)
                    {
                        npc.PropertyChanged += Food_PropertyChanged;
                    }
                }
            }
        }
       private void Food_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _isDirty = true;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Сохранить все изменения?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (var food in FoodList)
                    {
                        int dbCategoryId = ReturnCategoryID(food.CategoryId);

                        if (food.FoodId == 0)
                        {
                            var newFood = new Foods
                            {
                                Name = food.Name,
                                Calories = food.Calories,
                                ProteinG = food.ProteinG,
                                FatG = food.FatG,
                                CarbsG = food.CarbsG,
                                FiberG = food.FiberG,
                                IsActive = food.IsActive,
                                CategoryId = dbCategoryId
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
                                existingFood.CategoryId = dbCategoryId;
                            }
                        }
                    }
                    Core.kursEntities.SaveChanges();
                    LoadData();
                    MessageBox.Show("Сохранено успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var newFood = new Foods
            {
                Name = "Новый продукт",
                Calories = 0,
                ProteinG = 0,
                FatG = 0,
                CarbsG = 0,
                FiberG = 0,
                IsActive = true,
                CategoryId = 0
            };
            FoodList.Add(newFood);
            ProductDataGrid.SelectedItem = newFood;
            ProductDataGrid.ScrollIntoView(newFood);
        }
        private int ReturnCategoryID(int comboIndex)
        {
            switch (comboIndex)
            {
                case 0: return 1;
                case 1: return 2;
                case 2: return 3;
                case 3: return 4;
                default: return 1;
            }
        }
        private int ConvertDbIdToComboIndex(int dbCategoryId)
        {
            switch (dbCategoryId)
            {
                case 1: return 0;
                case 2: return 1;
                case 3: return 2;
                case 4: return 3;
                default: return 0;
            }
        }
    }
}

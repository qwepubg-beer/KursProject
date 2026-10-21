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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.res;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для Measurement.xaml
    /// </summary>
    public partial class Measurement : Page
    {
        public ObservableCollection<ProgressMeasurements> Measurements{ get; set; }
        public Measurement()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            UserDataGrid.ItemsSource = Measurements;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var newMeasurement = new ProgressMeasurements
            {
                   MeasurementId = 0,
                   MeasuredAt =DateTime.Now,
                   WeightKg = Core.profile.WeightKg!=null? Core.profile.WeightKg.Value:0,
                   HeightCm = Core.profile.HeightCm != null ? Core.profile.HeightCm.Value : 0,
                   ChestCm =null,
                   WaistCm=null,
                   HipsCm = null,
                   BicepsCm = null,
                   BodyFatPct = null,
            };
            Measurements.Add(newMeasurement);
            UserDataGrid.SelectedItem = newMeasurement;
            UserDataGrid.ScrollIntoView(newMeasurement);
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            UserDataGrid.CommitEdit();
            var result = System.Windows.MessageBox.Show("Сохранить все изменения?", "Подтверждение",MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (var Measurement in Measurements)
                    {
                        if (Measurement.MeasurementId == 0)
                        {
                            var newMeasurement = new ProgressMeasurements
                            {
                                MeasuredAt = Measurement.MeasuredAt,
                                WeightKg = Measurement.WeightKg,
                                ChestCm = Measurement.ChestCm,
                                WaistCm = Measurement.WaistCm,
                                HipsCm = Measurement.HipsCm,
                                BicepsCm = Measurement.BicepsCm,
                                BodyFatPct = Measurement.BodyFatPct,
                                HeightCm = Measurement.HeightCm,
                            };
                            Core.kursEntities.ProgressMeasurements.Add(newMeasurement);
                        }
                    }
                    Core.kursEntities.SaveChanges();
                    Core.editProfile();
                    LoadData();
                    System.Windows.MessageBox.Show("Сохранено успешно!", "Успех",MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

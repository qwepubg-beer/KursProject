using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Functions;
using WpfApp1.res;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для Measurement.xaml
    /// </summary>
    public partial class Measurement : Page, ITrackChanges
    {
        private bool _isDirty = false;
        public ObservableCollection<ProgressMeasurements> Measurements{ get; set; }
        public Measurement()
        {
            InitializeComponent();
            LoadData();
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
        private void LoadData()
        {  
            Measurements = new ObservableCollection<ProgressMeasurements>(Core.kursEntities.ProgressMeasurements.Where(y => y.UserID == Core.user.UserId).ToList());
            MeasurementDataGrid.ItemsSource = Measurements;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var newMeasurement = new ProgressMeasurements
            {
                   MeasurementId = 0,
                   MeasuredAt =DateTime.Now,
                   UserID=Core.user.UserId,
                   WeightKg = Core.profile.WeightKg!=null? Core.profile.WeightKg.Value:0,
                   HeightCm = Core.profile.HeightCm != null ? Core.profile.HeightCm.Value : 0,
                   ChestCm =null,
                   WaistCm=null,
                   HipsCm = null,
                   BicepsCm = null,
                   BodyFatPct = null,
            };
            Measurements.Add(newMeasurement);
            MeasurementDataGrid.SelectedItem = newMeasurement;
            MeasurementDataGrid.ScrollIntoView(newMeasurement);
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MeasurementDataGrid.CommitEdit();
            var result = MessageBox.Show("Сохранить все изменения?", "Подтверждение",MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes && Measurements!=null)
            {
                try
                {
                    foreach (var Measurement in Measurements)
                    {
                        if (Measurement.MeasurementId == 0)
                        {
                            var newMeasurement = new ProgressMeasurements
                            {
                                UserID = Core.user.UserId,
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
                    MessageBox.Show("Сохранено успешно!", "Успех",MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }
    }
}

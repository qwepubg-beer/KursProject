using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Functions;
using WpfApp1.res;
using WpfApp1.Services;

namespace WpfApp1.Pages
{

    public partial class ProfilePage : Page, ITrackChanges
    {
        private bool _isDirty = false;
        private readonly MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
        public ProfilePage()
        {
            InitializeComponent();
            СomboBxGoal.ItemsSource = Core.kursEntities.Goals.ToList();
            StyleComboBox.ItemsSource = Core.kursEntities.Styles.ToList();
            LoadData();
            SubscribeToChanges();
        }

        public bool HasUnsavedChanges()
        {
            return _isDirty;
        }

        private void SubscribeToChanges()
        {
            TextBoxFIO.TextChanged += (s, e) => _isDirty = true;
            TextBoxMN.TextChanged += (s, e) => _isDirty = true;
            СomboBxGender.SelectionChanged += (s, e) => _isDirty = true;
            СomboBxGoal.SelectionChanged += (s, e) => _isDirty = true;
            birthday.SelectedDateChanged += (s, e) => _isDirty = true;
        }
        private void LoadData()
        {
            if (Core.profile == null) return;
            TextBoxFIO.Text = $"{Core.profile.LastName} {Core.profile.FirstName} {Core.profile.MiddleName}".Trim();
            birthday.SelectedDate = Core.profile.BirthDate ?? DateTime.Today;
            if (Core.profile.Gender != null)
            {
                СomboBxGender.SelectedIndex = Core.profile.Gender.ToLower() == "м" ? 0 : 1;
            }
            TextBoxMN.Text = Core.profile.MedicalNotes;
            if (Core.profile.GoalId != null)
            {
                СomboBxGoal.SelectedValue = Core.profile.GoalId;
            }
        }
        private bool Save()
        {
            UserProfiles userProfiles = Core.kursEntities.UserProfiles.FirstOrDefault(u => u.UserID == Core.profile.UserID);
            if (userProfiles == null) return false;

            // Валидация ФИО
            if (!string.IsNullOrWhiteSpace(TextBoxFIO.Text))
            {
                var list = TextBoxFIO.Text
                    .Trim()
                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                if (list.Count < 2 || list.Count > 3)
                {
                    MessageBox.Show("Введите ФИО в формате: Фамилия Имя [Отчество] (через пробел)",
                        "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                userProfiles.LastName = list[0];
                userProfiles.FirstName = list[1];
                userProfiles.MiddleName = list.Count == 3 ? list[2] : string.Empty;
            }
            else
            {
                MessageBox.Show("Поле ФИО не может быть пустым",
                    "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (СomboBxGender.SelectedItem != null)
            {
                userProfiles.Gender = СomboBxGender.SelectedIndex == 0 ? "м" : "ж";
            }
            if (СomboBxGoal.SelectedItem is Goals selectedGoal)
            {
                userProfiles.GoalId = selectedGoal.GoalId;
            }
            if (birthday.SelectedDate.HasValue && birthday.SelectedDate != DateTime.Today)
            {
                userProfiles.BirthDate = birthday.SelectedDate.Value;
            }
            Core.kursEntities.SaveChanges();
            _isDirty = false;
            return true;
        }
        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Сохранить изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (Save())
                {
                    MessageBox.Show("Изменения успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private void ButtonChangePass_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Сохранить изменения перед переходом?", "Подтверждение", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (Save())
                {
                    mainWindow?.MainFrame.NavigationService.Navigate(new ChangePassPage());
                }
            }
            else if (result == MessageBoxResult.No)
            {
                _isDirty = false;
                mainWindow?.MainFrame.NavigationService.Navigate(new ChangePassPage());
            }
        }
        private void StyleButton_Click(object sender, RoutedEventArgs e)
        {
            if (StyleComboBox.SelectedItem is Styles selectedStyle)
            {
                var themeService = new ThemeService();
                SettingsRepository.SaveTheme(Core.user.UserId, selectedStyle.id);
                string savedTheme = SettingsRepository.GetTheme(Core.user.UserId);
                themeService.Apply(savedTheme);

                MessageBox.Show("Тема успешно применена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Выберите тему!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

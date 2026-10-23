using System;
using System.Collections.Generic;
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
using WpfApp1.Functions;
using WpfApp1.res;
using WpfApp1.Services;

namespace WpfApp1.Pages
{

    public partial class ProfilePage : Page
    {
        MainWindow mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
        public ProfilePage()
        {
            InitializeComponent();
            СomboBxGoal.ItemsSource = Core.kursEntities.Goals.ToList();
            StyleComboBox.ItemsSource = Core.kursEntities.Styles.ToList();
        }
        void LoadData()
        {
            TextBoxFIO.Text = Core.profile.LastName + " " + Core.profile.FirstName + " " + Core.profile.MiddleName;
            birthday.DisplayDate = Core.profile.BirthDate.Value;
            СomboBxGender.SelectedIndex = Core.profile.Gender == "м" ? 0 : 1;
            TextBoxMN.Text = Core.profile.MedicalNotes;
            СomboBxGoal.SelectedValue = Core.profile.Goals;
            Height.Text=Core.profile.HeightCm.ToString();
        }
        void save()
        {
            UserProfiles userProfiles = Core.kursEntities.UserProfiles.FirstOrDefault(u => u.UserID == Core.profile.UserID);
            if (userProfiles != null)
            {
                if (!string.IsNullOrWhiteSpace(TextBoxFIO.Text))
                {
                    var list = TextBoxFIO.Text
                        .Trim()
                        .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList();

                    if (list.Count < 2 || list.Count > 3)
                    {
                        System.Windows.Forms.MessageBox.Show("Введите ФИО в формате: Фамилия Имя [Отчество] (через пробел)",
                            "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    userProfiles.LastName = list[0];
                    userProfiles.FirstName = list[1];
                    userProfiles.MiddleName = list.Count == 3 ? list[2] : string.Empty;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Поле ФИО не может быть пустым",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                userProfiles.Gender = СomboBxGender.SelectedIndex == 0 ? "м" : "ж";
                Goals g = СomboBxGoal.SelectedItem as Goals;
                userProfiles.GoalId = g.GoalId;
                userProfiles.BirthDate = birthday.DisplayDate;
                Core.kursEntities.SaveChanges();
            }
        }
        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            var result = System.Windows.MessageBox.Show("Сохранить изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                save();
            }
            else {}
        }

        private void ButtonChangePass_Click(object sender, RoutedEventArgs e)
        {
            var result = System.Windows.MessageBox.Show("Сохранить изменения?","Подтверждение", MessageBoxButton.YesNo,MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) { 
                save(); 
                mainWindow.MainFrame.NavigationService.Navigate(new ChangePassPage());
            }
            else { mainWindow.MainFrame.NavigationService.Navigate(new ChangePassPage());}
            
        }

        private void StyleButton_Click(object sender, RoutedEventArgs e)
        {
            if(StyleComboBox.SelectedItem!=null)
            {
                Styles g = StyleComboBox.SelectedItem as Styles;
                var themeService = new ThemeService();
                SettingsRepository.SaveTheme(Core.user.UserId, g.id);
                string savedTheme = SettingsRepository.GetTheme(Core.user.UserId);
                themeService.Apply(savedTheme);
            }
            else { System.Windows.MessageBox.Show("Выбирите тему!");}
        }
    }
}

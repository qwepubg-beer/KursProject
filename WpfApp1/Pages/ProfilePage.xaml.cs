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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.res;

namespace WpfApp1.Pages
{

    public partial class ProfilePage : Page
    {
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
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
            UserProfiles userProfiles = Core.kursEntities.UserProfiles.FirstOrDefault(u => u.ProfileID == Core.profile.ProfileID);
            if (userProfiles != null)
            {
                List<string> list = TextBoxFIO.Text.Split(' ').ToList();
                userProfiles.FirstName = list[1];
                userProfiles.LastName = list[0];
                userProfiles.MiddleName = list[2];
                userProfiles.Gender = СomboBxGender.SelectedIndex == 0 ? "м" : "ж";
                userProfiles.GoalId = СomboBxGoal.SelectedIndex;
                userProfiles.BirthDate = birthday.DisplayDate;
                Core.kursEntities.SaveChanges();
            }
        }
        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Сохранить изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                save();
            }
            else {}
        }

        private void ButtonChangePass_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Сохранить изменения?","Подтверждение", MessageBoxButton.YesNo,MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) { 
                save(); 
                mainWindow.MainFrame.NavigationService.Navigate(new ChangePassPage());
            }
            else { mainWindow.MainFrame.NavigationService.Navigate(new ChangePassPage());}
            
        }

        private void StyleButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

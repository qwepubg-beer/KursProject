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
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
            СomboBxGoal.ItemsSource = Core.kursEntities.Goals.ToList();
        }
        void LoadData()
        {
            TextBoxFIO.Text = Core.profile.LastName + " " + Core.profile.FirstName + " " + Core.profile.MiddleName;
            birthday.DisplayDate = Core.profile.BirthDate.Value;
            СomboBxGender.SelectedIndex = Core.profile.Gender == "м" ? 0 : 1;
            TextBoxMN.Text = Core.profile.MedicalNotes;
            СomboBxGoal.SelectedValue = Core.profile.Goals;
            Height.Text=Core.profile.HeightCm.ToString();
            Weight.Text = Core.profile.WeightKg.ToString();
        }
        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            UserProfiles userProfiles = Core.kursEntities.UserProfiles.FirstOrDefault(u=>u.ProfileId==Core.profile.ProfileId);
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
            else
            {
                return;
            }
        }
    }
}

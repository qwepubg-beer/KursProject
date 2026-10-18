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
        }
        void LoadData()
        {
            TextBoxFIO = Core.profile.LastName + " " + Core.profile.FirstName + " " + Core.profile.MiddleName;
            birthday.DisplayDate = Core.profile.BirthDate;
            СomboBxGender.SelectedIndex = Core.profile.Gender == 'м' ? 0 : 1;
        }
        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

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
using static WpfApp1.Function;
namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для ChangePassPage.xaml
    /// </summary>
    public partial class ChangePassPage : Page
    {
        public ChangePassPage()
        {
            InitializeComponent();
        }

        private void Conf_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrEmpty(PasswordBox.Password) || string.IsNullOrEmpty(PasswordBox2.Password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            else
            {
                var user = Core.kursEntities.Users.AsNoTracking().FirstOrDefault(u => u.Email == TextBoxLogin.Text && u.Password == GetHash(PasswordBox.Password));
                user.Password = GetHash(PasswordBox2.Password);
                Core.kursEntities.SaveChanges();
                MessageBox.Show("Пароль был изменён!");
            }
        }
    }
}

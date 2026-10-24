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
using WpfApp1.Functions;
using WpfApp1.res;
using static WpfApp1.Function;
namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для ChangePassPage.xaml
    /// </summary>
    public partial class ChangePassPage : Page, ITrackChanges
    {
        private bool _isDirty = false;
        public ChangePassPage()
        {
            InitializeComponent();
            SubscribeToChanges();
        }
        private void SubscribeToChanges()
        {
            TextBoxEmail.TextChanged += (s, e) => _isDirty = true;
            PasswordBox1.PasswordChanged += (s, e) => _isDirty = true;
            PasswordBox2.PasswordChanged += (s, e) => _isDirty = true;
        }
        public bool HasUnsavedChanges()
        {
            return _isDirty;
        }
        private void Conf_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxEmail.Text) || string.IsNullOrEmpty(PasswordBox1.Password) || string.IsNullOrEmpty(PasswordBox2.Password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            else
            {
                if(PasswordBox1.Password == PasswordBox2.Password)
                {
                    PasswordBox2.Password = "";
                    MessageBox.Show("Пароли не могут совпадать!");
                }
                else
                {
                var user = Core.kursEntities.Users.Find(Core.user.UserId);
                user.Password = GetHash(PasswordBox2.Password);
                Core.kursEntities.SaveChanges();
                MessageBox.Show("Пароль был изменён!");

                }
            }
        }
    }
}

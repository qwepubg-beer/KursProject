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

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
            loadpage();
        }
        void loadpage()
        {
            comboBxRole.SelectedIndex = 0;
        }
        void IsEmptyFields()
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text) ||string.IsNullOrEmpty(TextBoxFio.Text) || string.IsNullOrEmpty(PasswordBox.Password) || string.IsNullOrEmpty(PasswordBox2.Password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

        }
    }
}

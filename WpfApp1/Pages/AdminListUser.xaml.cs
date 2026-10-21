using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    /// Логика взаимодействия для AdminListUser.xaml
    /// </summary>
    public partial class AdminListUser : Page
    {
        public ObservableCollection<Users> UsersList { get; set; }
        public List<Roles> RolesList { get; set; }

        public AdminListUser()
        {
            InitializeComponent();
            LoadData();
            DataContext = this;
        }
        private void LoadData()
        {
            RolesList = Core.kursEntities.Roles.ToList();
            UsersList = new ObservableCollection<Users>(Core.kursEntities.Users.Where(y => y.RoleId != 3).ToList()); // админы ид=3
            UserDataGrid.ItemsSource = UsersList;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            UserDataGrid.CommitEdit();
            var result = MessageBox.Show("Сохранить все изменения?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (var user in UsersList)
                    {
                        if (user.UserId == 0)
                        {
                            var newUser = new Users
                            {
                                Email = user.Email,
                                Password = user.Password,
                                IsActive = user.IsActive,
                                RoleId= ReturnRoleID(user.RoleId)
                            };
                            Core.kursEntities.Users.Add(newUser);
                        }
                        else
                        {
                            var existingUser = Core.kursEntities.Users.Find(user.UserId);
                            if (existingUser != null)
                            {
                                existingUser.Email = user.Email;
                                existingUser.Password = user.Password;
                                existingUser.IsActive = user.IsActive;
                                existingUser.RoleId = ReturnRoleID(user.RoleId);
                            }
                        }
                    }
                    Core.kursEntities.SaveChanges();
                    LoadData();
                    MessageBox.Show("Сохранено успешно!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var newUser = new Users
            {
                UserId = 0,
                Email = "",
                Password = "",
                RoleId = RolesList.FirstOrDefault()?.RoleId ?? 0,
                IsActive = true,
                CreatedAt = DateTime.Now,
            };

            UsersList.Add(newUser);
            UserDataGrid.SelectedItem = newUser;
            UserDataGrid.ScrollIntoView(newUser);
        }
        public int ReturnRoleID(int id)
        {
            Roles role = Core.kursEntities.Roles.Find(id);
            return role != null ? role.RoleId : 1;
        }

    }
}

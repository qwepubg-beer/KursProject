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
using WpfApp1.Functions;
using WpfApp1.res;

namespace WpfApp1.Pages
{
    public partial class AdminListUser : Page, ITrackChanges
    {
        public ObservableCollection<Users> UsersList { get; set; }
        public List<Roles> RolesList { get; set; }
        private bool _isDirty = false;
        public AdminListUser()
        {
            InitializeComponent();
            LoadData();
            DataContext = this;
            UserDataGrid.CellEditEnding += UserDataGrid_CellEditEnding;
        }
        public bool HasUnsavedChanges()
        {
            return _isDirty;
        }
        private void UserDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                _isDirty = true;
            }
        }

        private void LoadData()
        {
            RolesList = Core.kursEntities.Roles.ToList();
            UsersList = new ObservableCollection<Users>(
                Core.kursEntities.Users.Where(y => y.RoleId != 3).ToList()
            );
            UserDataGrid.ItemsSource = UsersList;
            _isDirty = false;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            UserDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            var result = MessageBox.Show("Сохранить все изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (var user in UsersList)
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
                    Core.kursEntities.SaveChanges();
                    LoadData();
                    MessageBox.Show("Сохранено успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        public int ReturnRoleID(int id)
        {
            Roles role = RolesList.FirstOrDefault(r => r.RoleId == id);
            return role != null ? role.RoleId : 1;
        }
    }
}

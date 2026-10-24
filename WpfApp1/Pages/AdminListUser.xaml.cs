using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
        }

        public bool HasUnsavedChanges()
        {
            return _isDirty;
        }

        private void LoadData()
        {
            RolesList = Core.kursEntities.Roles.ToList();
            var dbUsers = Core.kursEntities.Users.Where(y => y.RoleId != 3).ToList();
            foreach (var user in dbUsers)
            {
                user.RoleId = ConvertDbIdToComboIndex(user.RoleId);
                if (user is INotifyPropertyChanged npc)
                {
                    npc.PropertyChanged -= User_PropertyChanged;
                    npc.PropertyChanged += User_PropertyChanged;
                }
            }
            UsersList = new ObservableCollection<Users>(dbUsers);
            UsersList.CollectionChanged += UsersList_CollectionChanged;
            this.DataContext = this;
            UserDataGrid.ItemsSource = UsersList;
            _isDirty = false;
        }
        private void UsersList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _isDirty = true;
            if (e.NewItems != null)
            {
                foreach (Users newUser in e.NewItems)
                {
                    if (newUser is INotifyPropertyChanged npc)
                    {
                        npc.PropertyChanged += User_PropertyChanged;
                    }
                }
            }
        }
        private void User_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _isDirty = true;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Сохранить все изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (var user in UsersList)
                    {
                        var existingUser = Core.kursEntities.Users.Find(user.UserId);
                        int dbRoleId = ReturnRoleID(user.RoleId);

                        if (existingUser != null)
                        {
                            existingUser.Email = user.Email;
                            existingUser.Password = user.Password;
                            existingUser.IsActive = user.IsActive;
                            existingUser.RoleId = dbRoleId;
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
        private int ReturnRoleID(int comboIndex)
        {
            switch (comboIndex)
            {
                case 0: return 3;
                case 1: return 1;
                case 2: return 2;
                default: return 1; 
            }
        }

        private int ConvertDbIdToComboIndex(int dbRoleId)
        {
            switch (dbRoleId)
            {
                case 3: return 0;
                case 1: return 1;
                case 2: return 2;
                default: return 1;
            }
        }
    }
}

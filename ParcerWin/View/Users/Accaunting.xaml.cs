using ParcerWin.Service;
using ParcerWin.Service.Model;
using ParcerWin.View.MainScreen;
using ParcerWin.View.Splash;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ParcerWin.View.Users
{
    /// <summary>
    /// Interaction logic for Accaunting.xaml
    /// </summary>
    public partial class Accaunting : Page
    {
        private readonly IAuthService _authService;
        private readonly Dictionary<string, string> _roles;
        private UserViewModel _user;

        public byte CountError { get; private set; } = 0;

        public Accaunting(IAuthService authService, ObservableCollection<UserViewModel> users)
        {
            _authService = authService;

            _roles = new();
            _roles.Add("Работник", "worker");
            _roles.Add("Менеджер", "manager");
            _roles.Add("Администратор", "admin");

            InitializeComponent();

            for (var i = 0; i < users.Count; i++)
                users[i].Position = _roles.FirstOrDefault(pair => pair.Value == users[i].Position).Key;

            dgrdUsers.ItemsSource = users;

            _user = new();
            DataContext = _user;

            cbxRole.ItemsSource = _roles.Keys;
            cbxRole.SelectedIndex = 0;
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (CountError != 0)
            {
                var errorMessage = string.IsNullOrWhiteSpace(_user.Error) ? "" : _user.Error;

                MessageBox.Show("Есть ошибка");
                return;
            }

            var role = _roles[(string)cbxRole.SelectedValue];

            _user.Position = role;

            var user = new User()
            {
                Login = _user.Login,
                Password = _user.Password,
                UserInfo = new UserInfo()
                {
                    FirstName = _user.FirstName,
                    LastName = _user.LastName,
                    Position = _user.Position,
                }
            };

            var result = await _authService.AddUser(user);

            if (!result)
                MessageBox.Show($"Пользователь с логином {user.Login} уже существует");

            var users = Task.Run(() => _authService.GetUsers()).Result;

            for (var i = 0; i < users.Count; i++)
                users[i].Position = _roles.FirstOrDefault(pair => pair.Value == users[i].Position).Key;

            dgrdUsers.ItemsSource = users;

            _user = new();
            DataContext = _user;
        }

        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action is ValidationErrorEventAction.Added)
                CountError++;
            else
                CountError--;
        }
    }
}

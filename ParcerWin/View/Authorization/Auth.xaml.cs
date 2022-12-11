using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
using Newtonsoft.Json;
using ParcerWin.Service;
using ParcerWin.Service.Model;
using ParcerWin.View.MainScreen;
using ParcerWin.View.Splash;

namespace ParcerWin.Authorization
{
    /// <summary>
    /// Interaction logic for Auth.xaml
    /// </summary>
    public partial class Auth : Page
    {
        private readonly IAuthService _authService;
        private readonly IDataService _dataService;
        private MainWindow _mainWindow;
        private AuthPanel _panel;

        public Auth(IAuthService authService, IDataService dataService, MainWindow mainWindow, AuthPanel panel)
        {
            _authService = authService;
            _dataService = dataService;
            _mainWindow = mainWindow;
            _panel = panel;

            InitializeComponent();
        }

        private async void Auth_Click(object sender, RoutedEventArgs e)
        {
            var user = new User()
            {
                Login = login.Text,
                Password = password.Text
            };

            var splash = new SplashPage();
            _panel.frmPanel.Navigate(splash);

            var tokens = await _authService.Login(user);

            _panel.frmPanel.Navigate(this);

            if (tokens is null)
                MessageBox.Show("Input other login or password");
            else
            {
                Global.Token = tokens;

                _mainWindow.frmScreen.Navigate(new MainPage(_authService, _dataService, _mainWindow));
            }
        }

        private async void Auth_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Auth_Click(sender, e);
            }
        }
    }
}

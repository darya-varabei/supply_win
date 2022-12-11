using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ParcerWin.Authorization;
using ParcerWin.Service;
using ParcerWin.View.MainScreen;
using ParcerWin.View.Splash;

namespace ParcerWin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IAuthService _authService;
        private readonly IDataService _dataService;

        public MainWindow(IAuthService authService, IDataService dataService)
        {
            _authService = authService;
            _dataService = dataService;

            InitializeComponent();
        }

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            try
            {
                var authPanel = new AuthPanel(_authService, _dataService, this);
                var splash = new SplashPage();

                authPanel.frmPanel.Navigate(splash);
                this.Show();

                frmScreen.Navigate(authPanel);
                
                var isAuth = await _authService.CheckKey();

                if (isAuth)
                    frmScreen.Navigate(new MainPage(_authService, _dataService, this));
                else
                    frmScreen.Navigate(new AuthPanel(_authService, _dataService, this));
            }
            catch (Exception)
            {
                MessageBox.Show($"Нет соединения с сервером", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }
    }
}

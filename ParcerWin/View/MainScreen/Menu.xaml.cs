using ParcerWin.Authorization;
using ParcerWin.Service;
using ParcerWin.View.MainScreen.Screens;
using ParcerWin.View.MainScreen.Screens.WhereHo;
using ParcerWin.View.Splash;
using ParcerWin.View.Users;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace ParcerWin.View.MainScreen
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        private MainPage _mainPage;
        private MainWindow _mainWindow;
        private readonly IAuthService _authService;
        private readonly IDataService _dataService;

        private readonly WhereHouse _whereHouse;

        public Menu(IAuthService authService, IDataService dataService, MainPage mainPage, MainWindow mainWindow)
        {
            _mainPage = mainPage;
            _mainWindow = mainWindow;
            _authService = authService;
            _dataService = dataService;

            _whereHouse = new WhereHouse(_dataService, _mainPage);
            
            InitializeComponent();

            _mainPage.frmPage.Navigate(_whereHouse);

            btnWerehouse.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00111D"));
            btnWerehouse.Opacity = 0.44;

            if (Global.User.UserInfo.Position == "manager")
                grdbtnUsers.Visibility = Visibility.Collapsed;
            else
                grdbtnUsers.Visibility = Visibility.Visible;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            using var file = File.Open("auth.bin", FileMode.Create);
            _mainWindow.frmScreen.Navigate(new AuthPanel(_authService, _dataService, _mainWindow));
        }

        private async void Wherehouse_Click(object sender, RoutedEventArgs e)
        {
            btnUsers.Background = Brushes.Transparent;
            btnCutting.Background = Brushes.Transparent;
            btnExit.Background = Brushes.Transparent;
            btnUsers.Opacity = 1;
            btnCutting.Opacity = 1;
            btnExit.Opacity = 1;

            btnWerehouse.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00111D"));
            btnWerehouse.Opacity = 0.44;

            _mainPage.frmPage.Navigate(new SplashPage());

            _whereHouse._dataGrid.dgrdPackages.ItemsSource = _whereHouse._selectStatus switch
            {
                Status.Available => await _dataService.GetPackages("Имеется"),
                Status.InProcessing => await _dataService.GetPackages("В обработке"),
                Status.WithDefect => await _dataService.GetPackages("С дефектом"),
                Status.Used => await _dataService.GetPackages("Использован"),
            };

            _mainPage.frmPage.Navigate(_whereHouse);
        }

        private async void Users_Click(object sender, RoutedEventArgs e)
        {
            btnWerehouse.Background = Brushes.Transparent;
            btnCutting.Background = Brushes.Transparent;
            btnExit.Background = Brushes.Transparent;
            btnWerehouse.Opacity = 1;
            btnCutting.Opacity = 1;
            btnExit.Opacity = 1;

            btnUsers.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
            btnUsers.Opacity = 0.2;

            _mainPage.frmPage.Navigate(new SplashPage());

            var users = await _authService.GetUsers();

            _mainPage.frmPage.Navigate(new Accaunting(_authService, users));
        }

        private void btnCutting_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Данная возможность находиться в разработке", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);

            //btnWerehouse.Background = Brushes.Transparent;
            //btnUsers.Background = Brushes.Transparent;
            //btnExit.Background = Brushes.Transparent;
            //btnWerehouse.Opacity = 1;
            //btnUsers.Opacity = 1;
            //btnExit.Opacity = 1;

            //btnCutting.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
            //btnCutting.Opacity = 0.2;
        }
    }
}

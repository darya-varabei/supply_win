using ParcerWin.Service;
using ParcerWin.Service.Model;
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

namespace ParcerWin.View.MainScreen.Screens.WhereHo
{
    /// <summary>
    /// Interaction logic for DataGrid.xaml
    /// </summary>
    public partial class DataGrid : Page
    {
        private readonly MainPage _mainPage;
        private readonly WhereHouse _whereHouse;
        private readonly IDataService _dataService;

        public DataGrid(MainPage mainPage, WhereHouse whereHouse, IDataService dataService, ObservableCollection<PackageModel> packages)
        {
            _mainPage = mainPage;
            _whereHouse = whereHouse;
            _dataService = dataService;

            InitializeComponent();

            dgrdPackages.ItemsSource = packages;
        }

        private void dgrdPackages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void Edit_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridCellInfo cellInfo = dgrdPackages.SelectedCells[0];

            var package = (PackageModel)cellInfo.Item;
            var splash = new SplashPage();

            _mainPage.frmPage.Navigate(splash);

            var pac = await _dataService.GetPackage(package.PackageId);

            _mainPage.frmPage.Navigate(new WhereHouseEdit(_dataService, pac, _whereHouse, _mainPage));
        }

        private async void Details_Click(object sender, RoutedEventArgs e)
        {
            var splash = new SplashPage();
            _mainPage.frmPage.Navigate(splash);

            var package = await _dataService.GetPackage(((PackageModel)dgrdPackages.SelectedItem).PackageId);

            _mainPage.frmPage.Navigate(new WhereHouseDetails(package, _mainPage, _whereHouse, _dataService));
        }
    }
}

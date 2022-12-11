using ParcerWin.Service;
using ParcerWin.Service.Model;
using ParcerWin.View.MainScreen.Screens.WhereHo.SelectionPages;
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
    /// Interaction logic for WhereHouseAdd.xaml
    /// </summary>
    public partial class WhereHouseAdd : Page
    {
        private readonly IDataService _dataService;
        private readonly MainPage _mainPage;
        private readonly WhereHouse _whereHouse;
        private ExtendedPackageViewModel _package;

        private Total _totalPage;
        private Chemistry _chemistryPage;
        private Phis _phisPage;

        public WhereHouseAdd(MainPage mainPage, WhereHouse whereHouse, IDataService dataService)
        {
            _dataService = dataService;
            _mainPage = mainPage;
            _whereHouse = whereHouse;

            InitializeComponent();

            _package = new ExtendedPackageViewModel();
            _package.SupplyDate = DateTime.Now;

            _totalPage = new Total(dataService, _package);
            _chemistryPage = new Chemistry(_package);
            _phisPage = new Phis(_package);

            frmSelect.Navigate(_totalPage);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _mainPage.frmPage.Navigate(_whereHouse);
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            var chemCountOfErrors = _chemistryPage.CountError;
            var phisCountOfErrors = _phisPage.CountError;
            var totalCountOfErrors = _totalPage.CountError;

            var sumOfErrors = chemCountOfErrors + phisCountOfErrors + totalCountOfErrors;

            if (sumOfErrors != 0)
            {
                var errorMassage = string.IsNullOrWhiteSpace(_package.Error) ? "Какие-то из параметров введены не верно" : _package.Error;

                MessageBox.Show($"{errorMassage}");
                return;
            }

            _package.NumberOfCertificate = _totalPage.tbNumberCertificate.Text;
            _package.Supplier = _totalPage.tbSupplier.Text;

            _mainPage.frmPage.Navigate(new SplashPage());

            await _dataService.AddPackage(_package);

            _mainPage.frmPage.Navigate(new WhereHouse(_dataService, _mainPage));
        }

        private void Total_Click(object sender, RoutedEventArgs e)
        {
            totalLn.Visibility = Visibility.Visible;
            chemistryLn.Visibility = Visibility.Hidden;
            PhisLn.Visibility = Visibility.Hidden;

            frmSelect.Navigate(_totalPage);
        }

        private void Chemistry_Click(object sender, RoutedEventArgs e)
        {
            totalLn.Visibility = Visibility.Hidden;
            chemistryLn.Visibility = Visibility.Visible;
            PhisLn.Visibility = Visibility.Hidden;

            frmSelect.Navigate(_chemistryPage);
        }

        private void Phis_Click(object sender, RoutedEventArgs e)
        {
            totalLn.Visibility = Visibility.Hidden;
            chemistryLn.Visibility = Visibility.Hidden;
            PhisLn.Visibility = Visibility.Visible;

            frmSelect.Navigate(_phisPage);
        }
    }
}

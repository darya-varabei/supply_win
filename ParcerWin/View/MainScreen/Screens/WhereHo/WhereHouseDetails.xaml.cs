using ParcerWin.Service;
using ParcerWin.Service.Model;
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

namespace ParcerWin.View.MainScreen.Screens.WhereHo
{
    /// <summary>
    /// Interaction logic for WhereHouseDetails.xaml
    /// </summary>
    public partial class WhereHouseDetails : Page
    {
        private ExtendedPackageViewModel _package;
        private readonly MainPage _mainPage;
        private readonly WhereHouse _whereHouse;

        public WhereHouseDetails(ExtendedPackageViewModel package, MainPage mainPage, WhereHouse whereHouse, IDataService dataService)
        {
            _package = package;
            _mainPage = mainPage;
            _whereHouse = whereHouse;

            if (!string.IsNullOrWhiteSpace(_package.Destination))
            {
                _package.Grade += $"-{_package.Destination}";
            }

            _package.CategoryOfDrawing += $"{_package.Elongation:F1}";
            _package.CoatingType += _package.CoatingClass;

            InitializeComponent();

            tbCertNumb.Text = $"№ {package.NumberOfCertificate}";
            tbCaseNumb.Text = $"№ {package.Batch}";

            dgrdTotal.ItemsSource = new ExtendedPackageViewModel[] { package };
            dgrdChemical.ItemsSource = new ExtendedPackageViewModel[] { package };
            dgrdResults.ItemsSource = new ExtendedPackageViewModel[] { package };
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _mainPage.frmPage.Navigate(_whereHouse);
        }
    }
}

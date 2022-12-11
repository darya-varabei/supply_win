using ParcerWin.Service;
using ParcerWin.Service.Logic;
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

namespace ParcerWin.View.MainScreen.Screens.WhereHo.SelectionPages
{
    /// <summary>
    /// Interaction logic for Total.xaml
    /// </summary>
    public partial class Total : Page
    {
        public byte CountError { get; private set; } = 0;
        private readonly IDataService _dataService;
        private ExtendedPackageViewModel _package;

        public Total(IDataService dataService, ExtendedPackageViewModel package)
        {
            _dataService = dataService;
            _package = package;

            InitializeComponent();

            DataContext = package;

            //var listOfCertNumbers = Task.Run(() => dataService.GetNumbersOfCertificates()).Result;

            //tbNumberCertificate.IsEnabled = true;
            //tbNumberCertificate.ItemsSource = listOfCertNumbers;
            //tbNumberCertificate.SelectedItem = listOfCertNumbers is not null ? listOfCertNumbers[0] : null;

            tbDate.Text = DateTime.Now.ToShortDateString();
        }

        public Total(ExtendedPackageViewModel package)
        {
            InitializeComponent();

            DataContext = package;

            //tbNumberCertificate.ItemsSource = new List<string>() { package.NumberOfCertificate };
            //tbNumberCertificate.SelectedItem = package.NumberOfCertificate;
            //tbSupplier.ItemsSource = new List<string>() { package.Supplier };
            //tbSupplier.SelectedItem = package.Supplier;
        }

        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action is ValidationErrorEventAction.Added)
                CountError++;
            else
                CountError--;
        }

        private async void tbNumberCertificate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_dataService is null)
                return;

            //var supplier = await _dataService.GetSupplierByNumberOfCertificate((string)tbNumberCertificate.SelectedValue);

            //tbSupplier.ItemsSource = new List<string>() { supplier };
            //tbSupplier.SelectedItem = supplier;
        }
    }
}

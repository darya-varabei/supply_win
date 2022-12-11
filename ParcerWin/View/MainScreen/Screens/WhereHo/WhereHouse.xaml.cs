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
    /// Interaction logic for WhereHouse.xaml
    /// </summary>
    public partial class WhereHouse : Page
    {
        private readonly IDataService _dataService;
        private MainPage _mainPage;
        private readonly SplashPage _splashPage;
        public Status _selectStatus = Status.Available;
        public DataGrid _dataGrid;

        public WhereHouse(IDataService dataService, MainPage mainPage)
        {
            _dataService = dataService;
            _mainPage = mainPage;
            _splashPage = new SplashPage();
            _splashPage.rect.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#485EAA"));

            InitializeComponent();
        }

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            frmDataGrid.Navigate(_splashPage);

            var packages = EditValuesOfPackages(await _dataService.GetPackages("Имеется"));

            _dataGrid = new DataGrid(_mainPage, this, _dataService, packages);

            frmDataGrid.Navigate(_dataGrid);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _mainPage.frmPage.Navigate(new WhereHouseAdd(_mainPage, this, _dataService));
        }

        private async void search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                frmDataGrid.Navigate(_splashPage);

                if (!string.IsNullOrWhiteSpace(search.Text))
                    _dataGrid.dgrdPackages.ItemsSource = _selectStatus switch
                    {
                        Status.Available => await _dataService.Search(search.Text, "Имеется"),
                        Status.InProcessing => await _dataService.Search(search.Text, "В обработке"),
                        Status.WithDefect => await _dataService.Search(search.Text, "С дефектом"),
                        Status.Used => await _dataService.Search(search.Text, "Использован"),
                    };
                else
                    _dataGrid.dgrdPackages.ItemsSource = _selectStatus switch
                    {
                        Status.Available => await _dataService.GetPackages("Имеется"),
                        Status.InProcessing => await _dataService.GetPackages("В обработке"),
                        Status.WithDefect => await _dataService.GetPackages("С дефектом"),
                        Status.Used => await _dataService.GetPackages("Использован"),
                    };

                frmDataGrid.Navigate(_dataGrid);
            }            
        }

        private async void Available_Click(object sender, RoutedEventArgs e)
        {
            frmDataGrid.Navigate(_splashPage);

            _dataGrid.dgrdPackages.ItemsSource = EditValuesOfPackages(await _dataService.GetPackages("Имеется"));

            _selectStatus = Status.Available;
            availableLn.Visibility = Visibility.Visible;
            inProcessingLn.Visibility = Visibility.Hidden;
            withDefectLn.Visibility = Visibility.Hidden;
            usedLn.Visibility = Visibility.Hidden;

            frmDataGrid.Navigate(_dataGrid);
        }

        private async void InProcessing_Click(object sender, RoutedEventArgs e)
        {
            frmDataGrid.Navigate(_splashPage);

            _dataGrid.dgrdPackages.ItemsSource = EditValuesOfPackages(await _dataService.GetPackages("В обработке"));

            _selectStatus = Status.InProcessing;
            availableLn.Visibility = Visibility.Hidden;
            inProcessingLn.Visibility = Visibility.Visible;
            withDefectLn.Visibility = Visibility.Hidden;
            usedLn.Visibility = Visibility.Hidden;

            frmDataGrid.Navigate(_dataGrid);
        }

        private async void WithDefect_Click(object sender, RoutedEventArgs e)
        {
            frmDataGrid.Navigate(_splashPage);

            _dataGrid.dgrdPackages.ItemsSource = EditValuesOfPackages(await _dataService.GetPackages("С дефектом"));

            _selectStatus = Status.WithDefect;
            availableLn.Visibility = Visibility.Hidden;
            inProcessingLn.Visibility = Visibility.Hidden;
            withDefectLn.Visibility = Visibility.Visible;
            usedLn.Visibility = Visibility.Hidden;

            frmDataGrid.Navigate(_dataGrid);
        }

        private async void Used_Click(object sender, RoutedEventArgs e)
        {
            frmDataGrid.Navigate(_splashPage);

            _dataGrid.dgrdPackages.ItemsSource = EditValuesOfPackages(await _dataService.GetPackages("Использован"));

            _selectStatus = Status.Used;
            availableLn.Visibility = Visibility.Hidden;
            inProcessingLn.Visibility = Visibility.Hidden;
            withDefectLn.Visibility = Visibility.Hidden;
            usedLn.Visibility = Visibility.Visible;

            frmDataGrid.Navigate(_dataGrid);
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private ObservableCollection<PackageModel> EditValuesOfPackages(ObservableCollection<PackageModel> packages)
        {
            foreach (var package in packages)
            {
                if (!string.IsNullOrWhiteSpace(package.Destination))
                {
                    package.Grade += $"-{package.Destination}";
                }

                package.CategoryOfDrawing += $"{package.Elongation:F1}";
                package.CoatingType += package.CoatingClass;
            }

            return packages;
        }
    }
}

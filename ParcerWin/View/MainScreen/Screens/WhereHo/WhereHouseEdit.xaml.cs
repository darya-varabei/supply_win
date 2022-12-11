using ParcerWin.Service;
using ParcerWin.Service.Model;
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
using System.Drawing;
using Microsoft.Win32;
using ParcerWin.View.MainScreen.Screens.WhereHo.SelectionPages;
using System.Globalization;
using ParcerWin.View.Splash;

namespace ParcerWin.View.MainScreen.Screens.WhereHo
{
    /// <summary>
    /// Interaction logic for WhereHouseEdit.xaml
    /// </summary>
    public partial class WhereHouseEdit : Page
    {
        private readonly IDataService _dataService;
        private ExtendedPackageViewModel _package;
        private readonly MainPage _mainPage;
        private readonly WhereHouse _whereHouse;
        private List<Image> _listOfImages;

        private Total _totalPage;
        private Chemistry _chemistryPage;
        private Phis _phisPage;

        public WhereHouseEdit(IDataService dataService, ExtendedPackageViewModel package, WhereHouse whereHouse, MainPage mainPage)
        {
            _mainPage = mainPage;

            _dataService = dataService;
            _package = package;
            _whereHouse = whereHouse;

            InitializeComponent();

            DataContext = _package;

            _totalPage = new Total(_package);

            _totalPage.tbSupplier.IsReadOnly = true;
            _totalPage.tbNumberCertificate.IsReadOnly = true;

            _chemistryPage = new Chemistry(_package);
            _phisPage = new Phis(_package);

            frmSelect.Navigate(_totalPage);

            _listOfImages = new List<Image>() { tbPhoto1, tbPhoto2, tbPhoto3, tbPhoto4, tbPhoto5, tbPhoto6 };

            if (_package.Photo is not null)
            {
                btnDownload.Visibility = Visibility.Visible;

                for (var i = 0; i < _package.Photo.Count && i < 6; i++)
                    _listOfImages[i].Source = LoadImage(_package.Photo[i]);
            }
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

            _mainPage.frmPage.Navigate(new SplashPage());

            await _dataService.UpdatePackage(_package);

            _whereHouse._dataGrid.dgrdPackages.ItemsSource = _whereHouse._selectStatus switch
            {
                Status.Available => await _dataService.GetPackages("Имеется"),
                Status.InProcessing => await _dataService.GetPackages("В обработке"),
                Status.WithDefect => await _dataService.GetPackages("С дефектом"),
                Status.Used => await _dataService.GetPackages("Использован"),
            };

            _mainPage.frmPage.Navigate(_whereHouse);
        }

        private void SaveImages_Click(object sender, RoutedEventArgs e)
        {
            for (var i = 0; i < _package.Photo.Count && i < 6; i++)
            {
                var picture = LoadImage(_package.Photo[i]);

                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Image file (*.png)|*.png";

                if (saveFileDialog.ShowDialog() == true)
                    picture.Save(saveFileDialog.FileName);
            }
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

        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) 
                return null;

            var image = new BitmapImage();

            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.Rotation = Rotation.Rotate90;
                image.EndInit();
            }

            image.Freeze();
            
            return image;
        }
    }
}

using ParcerWin.Service;
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

namespace ParcerWin.Authorization
{
    /// <summary>
    /// Interaction logic for AuthPanel.xaml
    /// </summary>
    public partial class AuthPanel : Page
    {
        private readonly IAuthService _authService;
        private readonly IDataService _dataService;
        private MainWindow _mainWindow;

        public AuthPanel(IAuthService authService, IDataService dataService, MainWindow mainWindow)
        {
            _authService = authService;
            _dataService = dataService;
            _mainWindow = mainWindow;

            InitializeComponent();

            frmPanel.Navigate(new Auth(authService, dataService, mainWindow, this));
        }
    }
}

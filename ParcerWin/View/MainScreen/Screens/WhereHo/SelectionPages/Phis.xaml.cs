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
    /// Interaction logic for Phis.xaml
    /// </summary>
    public partial class Phis : Page
    {
        public byte CountError { get; private set; } = 0;

        public Phis(ExtendedPackageViewModel package)
        {
            InitializeComponent();

            DataContext = package;
        }

        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action is ValidationErrorEventAction.Added)
                CountError++;
            else
                CountError--;
        }
    }
}

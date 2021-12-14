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
using System.Windows.Shapes;

namespace FinTracker.Loans
{
    /// <summary>
    /// Interaction logic for EditLoanWindow.xaml
    /// </summary>
    public partial class EditLoanWindow : Window
    {
        MainWindow _mainWindow;
        public EditLoanWindow(MainWindow mainWindow)
        {
            InitializeComponent();
        }
    }
}

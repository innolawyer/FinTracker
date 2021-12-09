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

namespace FinTracker
{
    /// <summary>
    /// Interaction logic for AddExtraPayment.xaml
    /// </summary>
    public partial class AddExtraPayment : Window
    {
        ViewLoanPaymentsWindow _viewLoanPaymentsWindow;
        Storage _storage;
        public AddExtraPayment(ViewLoanPaymentsWindow viewLoanPaymentsWindow)
        {
            InitializeComponent();
        }

        private void ButtonCreateExtraPayment_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

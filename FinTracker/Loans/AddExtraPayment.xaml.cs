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
        MainWindow _mainWindow;        
        private Storage _storage = Storage.GetStorage();
        public AddExtraPayment(MainWindow mainWindow)
        {
            InitializeComponent();
            FillingComboBoxExtraPaymentPurpose();
            _mainWindow = mainWindow;
        }

        public void FillingComboBoxExtraPaymentPurpose()
        {
            ComboBoxExtraPaymentPurpose.Items.Add("Уменьшение срока");
            ComboBoxExtraPaymentPurpose.Items.Add("Уменьшение платежа");
        }

        private void ButtonCreateExtraPayment_Click(object sender, RoutedEventArgs e)
        {
            Loan loan = ((Loan)_mainWindow.ListViewLoans.SelectedItem);
            if (ComboBoxExtraPaymentPurpose.SelectedItem == "Уменьшение платежа")
            {
               loan.DoExtraPaymentToDecreasePayment(Convert.ToDateTime(DatePickerOfExtraPayment.SelectedDate.Value), Convert.ToDouble(TextBoxAmountOfExtraPayment.Text));
                _mainWindow.ListViewLoans.Items.Refresh();
                this.Close();
            }
            else if (ComboBoxExtraPaymentPurpose.SelectedItem == "Уменьшение срока")
            {
                loan.DoExtraPaymentToDecreaseLoanTerm(Convert.ToDateTime(DatePickerOfExtraPayment.SelectedDate.Value), Convert.ToDouble(TextBoxAmountOfExtraPayment.Text));
                _mainWindow.ListViewLoans.Items.Refresh();
                this.Close();
            }
        }
    }
}

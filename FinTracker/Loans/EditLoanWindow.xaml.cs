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
        private Storage _storage = Storage.GetStorage();
        public EditLoanWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            
            FillingComboBoxLoanAssetEdit();
            FillingComboboxLoanStatusEdit();
            FillingEditLoanWindow();
        }

        public void FillingComboBoxLoanAssetEdit()
        {
            foreach (Asset asset in _storage.actualUser.Assets)
            {
                ComboBoxLoanAssetEdit.Items.Add(asset.Name);
            }
        }

        public void FillingComboboxLoanStatusEdit()
        {
            ComboBoxLoanStatusEdit.Items.Add("Не выплачен");
            ComboBoxLoanStatusEdit.Items.Add("Выплачен");

        }

        public void FillingEditLoanWindow()
        {
            
            Loan loan = ((Loan)_mainWindow.ListViewLoans.SelectedItem);
            DatePickerLoanStartEdit.SelectedDate = Convert.ToDateTime(loan.ActualPaymentDateTime);
            ComboBoxLoanAssetEdit.SelectedItem = Convert.ToString(loan.Asset.Name);
            TextBoxLoanCreditorNameEdit.Text = loan.CreditorsName;
            TextBoxLoanPercentEdit.Text = Convert.ToString(loan.Percent);
            TextBoxLoanPeriodEdit.Text = Convert.ToString(loan.Period);
            TextBoxLoanAmountEdit.Text = Convert.ToString(loan.Amount);
            ComboBoxLoanStatusEdit.SelectedItem = loan.Status;
            TextBoxRemainingTermEdit.Text = Convert.ToString(loan.RemainingTerm);
        }

        private void ButtonEditLoan_Click(object sender, RoutedEventArgs e)
        {
            User user = _storage.actualUser;
            Asset asset = user.GetAssetByName(ComboBoxLoanAssetEdit.SelectedItem.ToString());
            Loan loan = ((Loan)_mainWindow.ListViewLoans.SelectedItem);
            loan.ActualPaymentDateTime = Convert.ToDateTime(DatePickerLoanStartEdit.SelectedDate);
            loan.Asset = asset;
            loan.CreditorsName = TextBoxLoanCreditorNameEdit.Text;
            loan.Percent = Convert.ToDouble(TextBoxLoanPercentEdit.Text);
            loan.Period = Convert.ToDouble(TextBoxLoanPeriodEdit.Text);
            loan.Amount = Convert.ToDouble(TextBoxLoanAmountEdit.Text);
            loan.Status = Convert.ToString(ComboBoxLoanStatusEdit.SelectedItem);
            loan.RemainingTerm = Convert.ToDouble(TextBoxRemainingTermEdit.Text);
            _mainWindow.ListViewLoans.Items.Refresh();
            _mainWindow.LabelRemainingDays.Content = Convert.ToString((loan.ActualPaymentDateTime - DateTime.Today).TotalDays);
            loan.TotalAmountOfPercents = loan.Amount * ((loan.Percent / 1200) * loan.Period);
            _mainWindow.LabelTotalAmountOfPercents.Content = loan.TotalAmountOfPercents;
            this.Close();
        }
    }
}

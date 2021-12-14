using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddLoanWindow.xaml
    /// </summary>
    public partial class AddLoanWindow : Window
    {
        private Storage _storage = Storage.GetStorage();
        MainWindow _mainWindow;
        public AddLoanWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            DatePickerLoanStart.SelectedDate = DateTime.Now;
            DatePickerLoanStart.SelectedDate.Value.Date.ToShortDateString();
            FillingComboBoxLoanAsset();
            FillingComboboxLoanStatus();
            ButtonCreateLoan_IsEnabled();
            _mainWindow = mainWindow;

        }

        public void FillingComboBoxLoanAsset ()
        {
            foreach (Asset asset in _storage.actualUser.Assets)
            {
                ComboBoxLoanAsset.Items.Add(asset.Name);
            }
        }

        

        public void FillingComboboxLoanStatus ()
        {
            ComboBoxLoanStatus.Items.Add("Не выплачен");
            ComboBoxLoanStatus.Items.Add("Выплачен");
            
        }



        public void ButtonCreateLoan_Click(object sender, RoutedEventArgs e)
        {
            int id = 1;
            if (_storage.actualUser.Loans.Count != 0)
            {
                id = _storage.actualUser.Loans.Count+1;
            }
            User user = _storage.actualUser;
            Asset asset = user.GetAssetByName(ComboBoxLoanAsset.SelectedItem.ToString());
            Loan nLoan = new Loan (asset, id, (DateTime)Convert.ToDateTime(DatePickerLoanStart.SelectedDate.Value.ToShortDateString()), (String)TextBoxLoanCreditorName.Text,
                (Double)Convert.ToDouble(TextBoxLoanPercent.Text), (Double)Convert.ToDouble (TextBoxLoanPeriod.Text),
                (String)Convert.ToString(ComboBoxLoanStatus.SelectedItem),(Double)Convert.ToDouble(TextBoxRemainingTerm.Text), 
                (Double)Convert.ToDouble(TextBoxLoanAmount.Text));
            user.AddLoan(nLoan);
            _mainWindow.ListViewLoans.Items.Add(nLoan);
            this.Close();
        }

        private void ButtonCreateLoan_IsEnabled()
        {
            if (TextBoxLoanCreditorName.Text == "" ||
                TextBoxLoanPercent.Text == "" ||
                TextBoxLoanPeriod.Text == "" ||
                TextBoxRemainingTerm.Text == "" ||
                TextBoxLoanAmount.Text == "")
            {
                ButtonCreateLoan.IsEnabled = false;
            }

            else if (TextBoxLoanCreditorName.Text != "" &&
                TextBoxLoanPercent.Text != "" &&
                TextBoxLoanPeriod.Text != "" &&
                TextBoxRemainingTerm.Text != "" &&
                TextBoxLoanAmount.Text != "")
            {
                ButtonCreateLoan.IsEnabled = true;
            }
        }

        private void TextBoxLoanCreditorName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ButtonCreateLoan_IsEnabled();
        }

        private void TextBoxLoanPercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            ButtonCreateLoan_IsEnabled();
        }

        private void TextBoxLoanPeriod_TextChanged(object sender, TextChangedEventArgs e)
        {
            ButtonCreateLoan_IsEnabled();
        }

        private void TextBoxLoanAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            ButtonCreateLoan_IsEnabled();
        }

        private void TextBoxRemainingTerm_TextChanged(object sender, TextChangedEventArgs e)
        {
            ButtonCreateLoan_IsEnabled();
        }

        private static readonly Regex _regex = new Regex("[^0-9.-]+");

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void TextBoxLoanPercent_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
            IsTextAllowed(TextBoxLoanPercent.Text);
        }

        private void TextBoxLoanPeriod_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
            IsTextAllowed(TextBoxLoanPeriod.Text);
        }

        private void TextBoxLoanAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
            IsTextAllowed(TextBoxLoanAmount.Text);
        }

        private void TextBoxRemainingTerm_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
            IsTextAllowed(TextBoxRemainingTerm.Text);
        }
    }
}

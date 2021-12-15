using FinTracker.Assets;
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
    /// Interaction logic for AddDeposit.xaml
    /// </summary>
    public partial class AddDeposit : Window
    {
        private Storage _storage = Storage.GetStorage();
        MainWindow _mainWindow;

        public AddDeposit(MainWindow mainWindow)
        {
            InitializeComponent();
            FillingComboBoxDepositSpendAsset();
            CheckBoxСapitalizationIsChecked();
            ComboBoxPeriod.Items.Add(Storage.period.Год);
            ComboBoxPeriod.Items.Add(Storage.period.Месяц);
            ComboBoxPeriod.Items.Add(Storage.period.Неделя);
            ComboBoxPeriod.Items.Add(Storage.period.День);
            DatePickerDepositStart.SelectedDate = DateTime.Today;
            DatePickerDepositStart.SelectedDate.Value.Date.ToShortDateString();
            _mainWindow = mainWindow;
        }
        
        public void FillingComboBoxDepositSpendAsset()
        {
            ComboBoxDepositSpendAsset.Items.Clear();
            foreach (Asset asset in _storage.actualUser.Assets)
            {
                ComboBoxDepositSpendAsset.Items.Add(asset.Name);
            }
        }

        private void ButtonSaveNewDeposit_Click(object sender, RoutedEventArgs e)
        {
            //User user = _storage.actualUser;
            if (CheckBoxСapitalization.IsChecked == false)
            {
                if (ComboBoxDepositSpendAsset.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите счет для начисления % по вкладу");
                    return;
                }
                else
                {
                    AbstractAsset asset = _storage.actualUser.GetAssetByName(ComboBoxDepositSpendAsset.SelectedItem.ToString());
                }
            }
         
            Deposit deposit = new Deposit(TextBoxNameAsset.Text, TextBoxBankName.Text, Convert.ToDouble(TextBoxDepositAmount.Text), (bool)CheckBoxWithdrawable.IsChecked,
                (bool)CheckBoxPutable.IsChecked, (bool)CheckBoxСapitalization.IsChecked, Convert.ToInt32(TextBoxTermDeposit.Text),
                Convert.ToDateTime(DatePickerDepositStart.Text), Convert.ToDouble(TextBoxPercent.Text), (Storage.period)(ComboBoxPeriod.SelectedItem), 
                _storage.actualUser.GetAssetByName(ComboBoxDepositSpendAsset.SelectedItem.ToString()));

            _storage.actualUser.AddDeposit(deposit);

            _mainWindow.ListViewDeposit.Items.Add(deposit);
            this.Close();
        }

        public void CheckBoxСapitalizationIsChecked()
        {
            if (CheckBoxСapitalization.IsChecked == true)
            {
                ComboBoxDepositSpendAsset.IsEnabled = false;
            }
            else
            {
                ComboBoxDepositSpendAsset.IsEnabled = true;
            }
        }

    }
}
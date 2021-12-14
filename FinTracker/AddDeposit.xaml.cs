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
            ComboBoxPeriod.Items.Add("Год");
            ComboBoxPeriod.Items.Add("Месяц");
            ComboBoxPeriod.Items.Add("Неделя");
            ComboBoxPeriod.Items.Add("День");
            DatePickerDepositStart.SelectedDate = DateTime.Today;
            DatePickerDepositEnd.SelectedDate = DateTime.Today.AddYears(1);
            DatePickerDepositStart.SelectedDate.Value.Date.ToShortDateString();
            DatePickerDepositEnd.SelectedDate.Value.Date.ToShortDateString();
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
            User user = _storage.actualUser;
            Asset asset = user.GetAssetByName(ComboBoxDepositSpendAsset.SelectedItem.ToString());
            Storage.period Period;
            if (ComboBoxPeriod.SelectedIndex == 0)
            {
                Period = Storage.period.year;
            }
            else if (ComboBoxPeriod.SelectedIndex == 1)
            {
                Period = Storage.period.month;
            }
            else if(ComboBoxPeriod.SelectedIndex == 2)
            {
                Period = Storage.period.week;
            }
            else
            {
                Period = Storage.period.day;
            }
            Deposit deposit = new Deposit(TextBoxNameAsset.Text, Convert.ToDouble(TextBoxDepositAmount.Text), (bool)CheckBoxWithdrawable.IsChecked,
                (bool)CheckBoxPutable.IsChecked, (bool)CheckBoxСapitalization.IsChecked, Convert.ToDateTime(DatePickerDepositEnd.Text),
                Convert.ToDateTime(DatePickerDepositStart.Text), Convert.ToDouble(TextBoxPercent.Text), Period);

            user.AddDeposit(TextBoxNameAsset.Text, Convert.ToDouble(TextBoxDepositAmount.Text), (bool)CheckBoxWithdrawable.IsChecked,
                (bool)CheckBoxPutable.IsChecked, (bool)CheckBoxСapitalization.IsChecked, Convert.ToDateTime(DatePickerDepositEnd.Text),
                Convert.ToDateTime(DatePickerDepositStart.Text), Convert.ToDouble(TextBoxPercent.Text), Period);

            _mainWindow.ListViewDeposit.Items.Add(deposit);
            this.Close();
        }
    }
}

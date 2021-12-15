﻿using System;
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

namespace FinTracker.DepositWindows
{
    /// <summary>
    /// Interaction logic for EditDeposit.xaml
    /// </summary>
    public partial class EditDeposit : Window
    {
        private Storage _storage = Storage.GetStorage();
        MainWindow _mainWindow;

        public EditDeposit(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            Deposit deposit = ((Deposit)_mainWindow.ListViewDeposit.SelectedItem);

            FillingComboBoxDepositSpendAsset();
            CheckBoxСapitalizationIsChecked();
            ComboBoxEditPeriod.Items.Add(Storage.period.Год);
            ComboBoxEditPeriod.Items.Add(Storage.period.Месяц);
            ComboBoxEditPeriod.Items.Add(Storage.period.Неделя);
            ComboBoxEditPeriod.Items.Add(Storage.period.День);

            TextBoxEditNameAsset.Text = Convert.ToString(deposit.Name);
            TextBoxEditBankName.Text = Convert.ToString(deposit.BankName);
            TextBoxEditDepositAmount.Text = Convert.ToString(deposit.Amount);
            DatePickerEditDepositStart.Text = Convert.ToString(deposit.OpeningDate);
            TextBoxEditTermDeposit.Text = Convert.ToString(deposit.TermDeposit);
            TextBoxEditPercent.Text = Convert.ToString(deposit.Percent);

            foreach (Object items in ComboBoxEditDepositSpendAsset.Items)
            {
                if (deposit.Asset.Name == items.ToString())
                {
                    ComboBoxEditDepositSpendAsset.SelectedItem = items;
                }
            }

            foreach (Object items in ComboBoxEditPeriod.Items)
            {
                if (deposit.Period == (Storage.period)(items))
                {
                    ComboBoxEditPeriod.SelectedItem = items;
                }
            }
            CheckBoxEditWithdrawable.IsChecked = deposit.Withdrawable;
            CheckBoxEditPutable.IsChecked = deposit.Putable;
            CheckBoxEditСapitalization.IsChecked = deposit.Сapitalization;
        }

        private void ButtonEditDeposit_Click(object sender, RoutedEventArgs e)
        {

            Deposit deposit = ((Deposit)_mainWindow.ListViewDeposit.SelectedItem);
            deposit.Name = TextBoxEditNameAsset.Text;
            deposit.BankName = TextBoxEditBankName.Text;
            deposit.Amount = Convert.ToDouble(TextBoxEditDepositAmount.Text);
            deposit.OpeningDate = Convert.ToDateTime(DatePickerEditDepositStart.Text);
            deposit.TermDeposit = Convert.ToInt32(TextBoxEditTermDeposit.Text);
            deposit.Period = (Storage.period)(ComboBoxEditPeriod.SelectedItem);
            deposit.Percent = Convert.ToDouble(TextBoxEditPercent.Text);
            deposit.Asset = _storage.actualUser.GetAssetByName(ComboBoxEditDepositSpendAsset.SelectedItem.ToString());
            _mainWindow.ListViewDeposit.Items.Refresh();
            this.Close();
        }

        public void CheckBoxСapitalizationIsChecked()
        {
            if (CheckBoxEditСapitalization.IsChecked == true)
            {
                ComboBoxEditDepositSpendAsset.IsEnabled = false;
            }
            else
            {
                ComboBoxEditDepositSpendAsset.IsEnabled = true;
            }
        }
        public void FillingComboBoxDepositSpendAsset()
        {
            ComboBoxEditDepositSpendAsset.Items.Clear();
            foreach (Asset asset in _storage.actualUser.Assets)
            {
                ComboBoxEditDepositSpendAsset.Items.Add(asset.Name);
            }
        }
    }
}

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

namespace FinTracker
{
    /// <summary>
    /// Interaction logic for EditAsset.xaml
    /// </summary>
    public partial class EditAsset : Window
    {
        private Storage _storage = Storage.GetStorage();
        private MainWindow _mainWindow;
        public EditAsset(MainWindow mainWindow)
        {
            InitializeComponent();
            
            ComboBoxAssetTypeEdit.Items.Add("Карта");
            ComboBoxAssetTypeEdit.Items.Add("Наличные");

            foreach (string category in _storage.actualUser.CategoriesSpend)
            {
                ComboBoxCashCategory.Items.Add(category);
            }
            if (_storage.actualAsset != null)
            {
                TextBoxAssetNameEdit.Text = _storage.actualAsset.Name;
                TextBoxAmountEdit.Text = Convert.ToString(_storage.actualAsset.Amount);

                if (_storage.actualAsset is Card)
                {
                    ComboBoxAssetTypeEdit.SelectedIndex = 0;
                    Card card = (Card)_storage.actualAsset;
                    TextBoxYearInterestEdit.Text = Convert.ToString(card.YearInterest * 100);
                    TextBoxMonthFeeEdit.Text = Convert.ToString(card.ServiceFee);
                    TextBoxFixCashbackEdit.Text = Convert.ToString(card.FixCashback * 100);

                    DatePickerDateSpendServiceFeeEdit.SelectedDate = card.DateSpendServiceFee;
                    DatePickerEnrollDateCashEdit.SelectedDate = card.EnrollDateCash;
                    DatePickerEnrollDateYearInterestEdit.SelectedDate = card.EnrollDateYearInterest;
                }
                else
                {
                    ComboBoxAssetTypeEdit.SelectedIndex = 1;

                }
            }

            TextBoxAmountEdit.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmountEdit_GotFocus);
            TextBoxAssetNameEdit.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmountEdit_GotFocus);
            TextBoxYearInterestEdit.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmountEdit_GotFocus);
            TextBoxFixCashbackEdit.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmountEdit_GotFocus);
            TextBoxMonthFeeEdit.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmountEdit_GotFocus);


            _mainWindow = mainWindow;
            _mainWindow.IsEnabled = false;
        }

        private void TextBoxAmountEdit_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = "";
        }

        private void ButtonAddNewPercentCashbackCategory_Click(object sender, RoutedEventArgs e)
        {
            Card card = (Card)_storage.actualAsset;
            card.AddCategoryCashback(ComboBoxCashCategory.Text, Convert.ToDouble(TextBoxNewPercent.Text));
        }

        private void ButtonEditAsset_Click(object sender, RoutedEventArgs e) 
        {

            Asset asset = (Asset)_storage.actualAsset;
            if (ComboBoxAssetTypeEdit.SelectedItem.ToString() == "Наличные")
            {
                asset.EditAsset(TextBoxAssetNameEdit.Text, Convert.ToDouble(TextBoxAmountEdit.Text));
            }

            if (ComboBoxAssetTypeEdit.SelectedItem.ToString() == "Карта")
            {
                Card card = (Card)_storage.actualAsset;

                card.EditCard(TextBoxAssetNameEdit.Text, Convert.ToDouble(TextBoxAmountEdit.Text),
                     Convert.ToDouble(TextBoxYearInterestEdit.Text), Convert.ToDouble(TextBoxFixCashbackEdit.Text), Convert.ToDouble(TextBoxMonthFeeEdit.Text),
                     Convert.ToDateTime(DatePickerEnrollDateCashEdit.Text), Convert.ToDateTime(DatePickerEnrollDateYearInterestEdit.Text),
                     Convert.ToDateTime(DatePickerDateSpendServiceFeeEdit.Text));
            }
            //_mainWindow.FillAssetListBox();
            _mainWindow.FillAssetsStackPanel();
            _mainWindow.LabelCurrentAmount.Content = _storage.actualAsset.Amount;
            _mainWindow.IsEnabled = true;
            //НЕ РАБОТАЕТ
            this.Close();
        }

        private void ComboBoxAssetType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonEditAsset.IsEnabled = true;

            if (ComboBoxAssetTypeEdit.SelectedIndex == 1)
            {
                TextBoxYearInterestEdit.IsEnabled = false;
                TextBoxFixCashbackEdit.IsEnabled = false;
                TextBoxMonthFeeEdit.IsEnabled = false;
                DatePickerEnrollDateCashEdit.IsEnabled = false;
                DatePickerEnrollDateYearInterestEdit.IsEnabled = false;
                DatePickerDateSpendServiceFeeEdit.IsEnabled = false;
                ComboBoxCashCategory.IsEnabled = false;
                TextBoxNewPercent.IsEnabled = false;
                ButtonAddNewPercentCashbackCategory.IsEnabled = false;
            }
            if (ComboBoxAssetTypeEdit.SelectedIndex == 0)
            {
                TextBoxYearInterestEdit.IsEnabled = true;
                TextBoxFixCashbackEdit.IsEnabled = true;
                TextBoxMonthFeeEdit.IsEnabled = true;
                DatePickerEnrollDateCashEdit.IsEnabled = true;
                DatePickerEnrollDateYearInterestEdit.IsEnabled = true;
                DatePickerDateSpendServiceFeeEdit.IsEnabled = true;
                ComboBoxCashCategory.IsEnabled = true;
                TextBoxNewPercent.IsEnabled = true;
                ButtonAddNewPercentCashbackCategory.IsEnabled = true;
            }
        }
    }
}

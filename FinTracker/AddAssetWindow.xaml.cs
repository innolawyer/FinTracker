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
    /// Interaction logic for AddAssetWindow.xaml
    /// </summary>
    public partial class AddAssetWindow : Window
    {
        private Storage _storage = Storage.GetStorage();
        private MainWindow _mainWindow;
        public AddAssetWindow (MainWindow mainWindow)        
        {
            InitializeComponent();
            DatePickerDateSpendServiceFee.SelectedDate = DateTime.Today;
            DatePickerDateSpendServiceFee.DisplayDateStart = DateTime.Today;
            DatePickerEnrollDateCash.SelectedDate = DateTime.Today;
            DatePickerEnrollDateCash.DisplayDateStart = DateTime.Today;
            DatePickerEnrollDateYearInterest.SelectedDate = DateTime.Today;
            DatePickerEnrollDateYearInterest.DisplayDateStart= DateTime.Today;

            ComboBoxAssetType.Items.Add( "Карта");
            ComboBoxAssetType.Items.Add("Наличные");
            foreach (string category in _storage.actualUser.CategoriesSpend)
            {
                ComboBoxCashCategory.Items.Add(category);
            }

            TextBoxAmount.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmount_GotFocus);
            TextBoxAssetName.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmount_GotFocus);
            TextBoxYearInterest.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmount_GotFocus);
            TextBoxFixCashback.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmount_GotFocus);
            TextBoxMonthFee.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmount_GotFocus);

            ButtonCreateAsset.IsEnabled = false;
             
            _mainWindow = mainWindow;
            _mainWindow.IsEnabled = false;
        }

        private void TextBoxAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = "";           
        }

        private void ButtonCreateAsset_Click(object sender, RoutedEventArgs e)
        {
            if (_storage.actualUser.IsUniqeAsset(TextBoxAssetName.Text))
             {
                User user = _storage.actualUser;
                if (ComboBoxAssetType.SelectedItem.ToString() == "Наличные")
                {
                    Asset asset = new Asset(TextBoxAssetName.Text, Convert.ToDouble(TextBoxAmount.Text));
                    user.AddAsset(TextBoxAssetName.Text, Convert.ToDouble(TextBoxAmount.Text));
                }
             
                if (ComboBoxAssetType.SelectedItem.ToString() == "Карта")
                {
                    Card card = new Card(TextBoxAssetName.Text, Convert.ToDouble(TextBoxAmount.Text),
                        Convert.ToDouble(TextBoxYearInterest.Text), Convert.ToDouble(TextBoxFixCashback.Text), Convert.ToDouble(TextBoxMonthFee.Text),
                        Convert.ToDateTime(DatePickerEnrollDateCash.Text), Convert.ToDateTime(DatePickerEnrollDateYearInterest.Text), Convert.ToDateTime(DatePickerDateSpendServiceFee.Text));
                    user.AddCard(TextBoxAssetName.Text, Convert.ToDouble(TextBoxAmount.Text),
                         Convert.ToDouble(TextBoxYearInterest.Text), Convert.ToDouble(TextBoxFixCashback.Text), Convert.ToDouble(TextBoxMonthFee.Text),
                         Convert.ToDateTime(DatePickerEnrollDateCash.Text), Convert.ToDateTime(DatePickerEnrollDateYearInterest.Text),
                         Convert.ToDateTime(DatePickerDateSpendServiceFee.Text));
                }
                _mainWindow.FillAssetListBox();
             
                Button buttonAsset = new Button();
                buttonAsset.Content = TextBoxAssetName.Text;
                buttonAsset.Click += _mainWindow.SetActualAsset;
                buttonAsset.Click += _mainWindow.LabelCurrentAmount_Display;
                buttonAsset.Click += _mainWindow.AddTransactionVisibility;
                buttonAsset.Click += _mainWindow.FillingTransactionsStackPanel;

                _mainWindow.StackPanelAssetList.Children.Add(buttonAsset);
                this.Close();
            }
            else
            {
                MessageBox.Show("Счет с таким именем уже существует");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _mainWindow.IsEnabled = true;
        }

        private void ButtonAddNewPercentCashbackCategory_Click(object sender, RoutedEventArgs e)
        {
            //AddCategoryCashback(ComboBoxCashCategory.Text, TextBoxNewPercent);
        }

        private void ComboBoxAssetType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonCreateAsset.IsEnabled = true;

            if (ComboBoxAssetType.SelectedIndex == 1)
            {
                TextBoxYearInterest.IsEnabled = false;
                TextBoxFixCashback.IsEnabled = false;
                TextBoxMonthFee.IsEnabled = false;
                DatePickerEnrollDateCash.IsEnabled = false;
                DatePickerEnrollDateYearInterest.IsEnabled = false;
                DatePickerDateSpendServiceFee.IsEnabled = false;
                ComboBoxCashCategory.IsEnabled = false;
                TextBoxNewPercent.IsEnabled = false;
                ButtonAddNewPercentCashbackCategory.IsEnabled = false;
            }
            if (ComboBoxAssetType.SelectedIndex == 0)
            {
                TextBoxYearInterest.IsEnabled = true;
                TextBoxFixCashback.IsEnabled = true;
                TextBoxMonthFee.IsEnabled = true;
                DatePickerEnrollDateCash.IsEnabled = true;
                DatePickerEnrollDateYearInterest.IsEnabled =true;
                DatePickerDateSpendServiceFee.IsEnabled = true;
                ComboBoxCashCategory.IsEnabled = true;
                TextBoxNewPercent.IsEnabled = true;
                ButtonAddNewPercentCashbackCategory.IsEnabled = true;
            }
        }
    }
}

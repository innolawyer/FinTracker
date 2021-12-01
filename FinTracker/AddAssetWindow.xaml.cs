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
        private MainWindow _mainWindow;
        public AddAssetWindow (MainWindow mainWindow)        

        {
            InitializeComponent();
            TextBoxAmount.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmount_GotFocus);
            TextBoxAssetName.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmount_GotFocus);
            TextBoxYearInterest.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmount_GotFocus);
            TextBoxFixCashback.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmount_GotFocus);
            TextBoxMonthFee.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmount_GotFocus);
            _mainWindow = mainWindow;
            _mainWindow.IsEnabled = false;
        }

        private void FillingTransactionsStackPanel(object sender, RoutedEventArgs e)
        {
            _mainWindow.StackPanelTransactionList.Children.Clear();
            foreach (Transaction transaction in _mainWindow.actualAsset.Transactions)
            {
                Button nTransactionButton = new Button();
                nTransactionButton.Content = $"{transaction.Date} {transaction.Sign}{transaction.Amount} {transaction.Category}";
                _mainWindow.StackPanelTransactionList.Children.Add(nTransactionButton);
            }
        }

        private void SetActualAsset(object sender, RoutedEventArgs e)
        {
            _mainWindow.actualAsset = _mainWindow.GetAssetByName(Convert.ToString(((Button)sender).Content));
        }

        private void TextBoxAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = "";           
        }

        private void ButtonCreateAsset_Click(object sender, RoutedEventArgs e)
        {
            User user = _mainWindow.actualUser;
            Asset asset = new Asset(TextBoxAssetName.Text, Convert.ToDouble(TextBoxAmount.Text));

            user.AddAsset(TextBoxAssetName.Text, Convert.ToDouble(TextBoxAmount.Text), Convert.ToDouble(TextBoxYearInterest.Text), 
                                                Convert.ToDouble(TextBoxFixCashback.Text), Convert.ToDouble(TextBoxMonthFee.Text));
            Button buttonAsset = new Button();
            buttonAsset.Content = TextBoxAssetName.Text;
            buttonAsset.Click += SetActualAsset;
            buttonAsset.Click += _mainWindow.LabelCurrentAmount_Display;
            buttonAsset.Click += FillingTransactionsStackPanel;

            _mainWindow.StackPanelAssetList.Children.Add(buttonAsset);
            //_mainWindow.asset.LabelCurrentAmount_Display();
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _mainWindow.IsEnabled = true;
        }
    }
}

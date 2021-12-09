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
            TextBoxAmount.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmount_GotFocus);
            TextBoxAssetName.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmount_GotFocus);
            TextBoxYearInterest.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmount_GotFocus);
            TextBoxFixCashback.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmount_GotFocus);
            TextBoxMonthFee.GotFocus += new System.Windows.RoutedEventHandler(this.TextBoxAmount_GotFocus);
            _mainWindow = mainWindow;
            _mainWindow.IsEnabled = false;
            ComboBoxChoiceAssetType.Items.Add("Карта");
            ComboBoxChoiceAssetType.Items.Add("Наличные деньги");
        }

        private void TextBoxAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = "";           
        }

        private void ButtonCreateAsset_Click(object sender, RoutedEventArgs e)
        {
            if (_storage.actualUser.IsUniqeAsset(TextBoxAssetName.Text))
             {
                if (ComboBoxChoiceAssetType.SelectedValue.ToString() == "Карта")
                {
                    User user = _storage.actualUser;
                    Card asset = new Card(TextBoxAssetName.Text, Convert.ToDouble(TextBoxAmount.Text),
                                            Convert.ToDouble(TextBoxYearInterest.Text),
                                            Convert.ToDouble(TextBoxFixCashback.Text),
                                            Convert.ToDouble(TextBoxMonthFee.Text),
                                            Convert.ToDateTime(DatePickerEnrollDateCash.Text), "Год");

                    user.AddAsset(TextBoxAssetName.Text, Convert.ToDouble(TextBoxAmount.Text), Convert.ToDouble(TextBoxYearInterest.Text),
                                                        Convert.ToDouble(TextBoxFixCashback.Text), Convert.ToDouble(TextBoxMonthFee.Text));
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
                else if (ComboBoxChoiceAssetType.SelectedValue.ToString() == "Наличные деньги")
                {
                    User user = _storage.actualUser;
                    Asset asset = new Asset(TextBoxAssetName.Text, Convert.ToDouble(TextBoxAmount.Text));

                    user.AddAsset(TextBoxAssetName.Text, Convert.ToDouble(TextBoxAmount.Text), Convert.ToDouble(TextBoxYearInterest.Text),
                                                        Convert.ToDouble(TextBoxFixCashback.Text), Convert.ToDouble(TextBoxMonthFee.Text));
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
            }
            else
            {
                MessageBox.Show("Счет с таким именем уже существует");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _mainWindow.IsEnabled = true;
            _mainWindow.GetAccessToLoans();
        }

        private void ComboBoxChoiceAssetType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxChoiceAssetType.SelectedValue.ToString() == "Карта")
            {
                TextBoxYearInterest.IsEnabled = true;
                TextBoxMonthFee.IsEnabled = true;
                TextBoxFixCashback.IsEnabled = true;
            }
            else if (ComboBoxChoiceAssetType.SelectedValue.ToString() == "Наличные деньги")
            {
                TextBoxYearInterest.IsEnabled = false;
                TextBoxMonthFee.IsEnabled = false;
                TextBoxFixCashback.IsEnabled = false;
            }
        }
    }
}

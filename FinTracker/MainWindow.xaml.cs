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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Storage _storage = Storage.GetStorage();

        public MainWindow()
        {
            InitializeComponent();

            DatePickerTransaction.SelectedDateFormat = DatePickerFormat.Short;
            DatePickerTransaction.SelectedDate = DateTime.Today;
            
            FillingComboBoxUser();
            ComboBoxChangeUser_SelectionDone();
            //FillCategoriesIncome();
            //FillCategories();
            FillAssetListBox();
            FillAssetsStackPanel();

            if (_storage.actualAsset == null)
            {
                ButtonConfirmTransaction.IsEnabled = false;
                ButtonConfirmTransaction.IsEnabled = false;
            }
            if (_storage.actualUser != null)
            {
                foreach (Card asset in _storage.actualUser.Assets)
                {
                    asset.GetMinAmount();
                    asset.EnrollmentCashbak();
                    asset.EnrollmentSumYearInterest();
                    asset.EnrollmentServiceFee();
                }
            }
        }

        public void FillingComboBoxUser()
        {
            ComboBoxChangeUser.Items.Clear();
            foreach (User user in _storage.Users)
            {
                ComboBoxChangeUser.Items.Add($"{user.Name}");
            }
        }
        
        public void LabelCurrentAmount_Display(object sender, RoutedEventArgs e)
        {
            LabelCurrentAmount.Content = _storage.actualAsset.GetAmount();
        }

        public void CurrentTransaction(object sender, RoutedEventArgs e)
        {
            _storage.actualTransaction = _storage.actualAsset.Transactions[StackPanelTransactionList.Children.IndexOf((Button)sender)];
        }


        public void FillAssetListBox()
        {
            ComboBoxCategoriesTransaction.Items.Clear();
            if (_storage.actualUser != null)
            {
                foreach (Asset asset in _storage.actualUser.Assets)
                {
                    ComboBoxCategoriesTransaction.Items.Add(asset.Name);
                }
            }
        }

        //public void FillTransactionCategories()
        //{
        //    if(RadioButtonIncome.IsChecked == true)
        //    {
        //        ComboBoxCategoriesTransaction.Items.Clear();
        //        if (_storage.actualUser != null)
        //        {
        //            foreach (string category in _storage.actualUser.CategoriesIncome)
        //            {
        //                ComboBoxCategoriesTransaction.Items.Add(category);
        //            }
        //        }
        //    }
        //    else if(RadioButtonСonsumption.IsChecked == true)
        //    {
        //        if (_storage.actualUser != null)
        //        {
        //            foreach (string category in _storage.actualUser.CategoriesSpend)
        //            {
        //                ComboBoxCategoriesTransaction.Items.Add(category);
        //            }
        //        }
        //    }
        //    else if(RadioButtonTransfer.IsChecked == true)
        //    {

        //    }
        //}

        //public void FillCategoriesIncome()
        //{
        //    ComboBoxCategoriesTransaction.Items.Clear();
        //    if (_storage.actualUser != null)
        //    {
        //        foreach (string category in _storage.actualUser.CategoriesIncome)
        //        {
        //            ComboBoxCategoriesTransaction.Items.Add(category);
        //        }
        //    }
        //}

        public void FillCategories(List <string> listCategories)
        {
            ComboBoxCategoriesTransaction.Items.Clear();
            if (_storage.actualUser != null)
            {
                foreach (string category in listCategories)
                {
                    ComboBoxCategoriesTransaction.Items.Add(category);
                }
            }
        }

        public void FillingTransactionsStackPanel(object sender, RoutedEventArgs e)
        {
            StackPanelTransactionList.Children.Clear();
            foreach (Transaction transaction in _storage.actualAsset.Transactions)
            {
                Button nTransactionButton = new Button();
                nTransactionButton.Content = $"{transaction.Date} {transaction.Sign}{transaction.Amount} {transaction.Category}";
                nTransactionButton.Click += CurrentTransaction;
                nTransactionButton.Click += SetTransactionData;
                StackPanelTransactionList.Children.Add(nTransactionButton);
            }
        }

        public void SetTransactionData(object sender, RoutedEventArgs e)
        {
            DatePickerTransaction.Text = _storage.actualTransaction.Date.ToString();
            TextBoxAmount.Text = _storage.actualTransaction.Amount.ToString();
            ComboBoxCategoriesTransaction.Text = _storage.actualTransaction.Category.ToString();
            TextBoxComment.Text = _storage.actualTransaction.Comment.ToString();
            if (_storage.actualTransaction.Sign == Storage.sign.income)
            {
                RadioButtonIncome.IsChecked = true;
            }
            else if (_storage.actualTransaction.Sign == Storage.sign.spend)
            {
                RadioButtonСonsumption.IsChecked = true;
            }
        }

        public void SetActualAsset(object sender, RoutedEventArgs e)
        {
            _storage.actualAsset = _storage.actualUser.GetAssetByName(Convert.ToString(((Button)sender).Content));
        }

        public void FillAssetsStackPanel()
        {
            if (_storage.actualUser != null)
            {
                StackPanelAssetList.Children.Clear();
                foreach (Asset asset in _storage.actualUser.Assets)
                {
                    Button buttonAsset = new Button();
                    buttonAsset.Content = asset.Name;
                    buttonAsset.Click += SetActualAsset; //кладет в сторадж
                    buttonAsset.Click += LabelCurrentAmount_Display;
                    buttonAsset.Click += AddTransactionVisibility; //активирует кнопки доход и расход
                    buttonAsset.Click += FillingTransactionsStackPanel; 

                    StackPanelAssetList.Children.Add(buttonAsset);
                }
            }
        }

        public void AddTransactionVisibility(object sender, RoutedEventArgs e)
        {
            if (_storage.actualAsset == null)
            {
                ButtonConfirmTransaction.IsEnabled = false;
                ButtonConfirmTransaction.IsEnabled = false;
            }
            else
            {
                ButtonConfirmTransaction.IsEnabled = true;
                ButtonConfirmTransaction.IsEnabled = true;
            }
        }

        private void ButtonCreateNewUser_Click(object sender, RoutedEventArgs e)
        {
            if (_storage.IsUniqeUser(TextBoxUserName.Text) == true)
            {
                _storage.Users.Add(new User(TextBoxUserName.Text));
                TextBoxUserName.Text = "";
                FillingComboBoxUser();
            }
            else
            {
                MessageBox.Show("Пользователь с таким именем уже создан");
            }
        }

        private void ButtonDeleteUser_Copy_Click(object sender, RoutedEventArgs e)
        {
            _storage.DeleteUser(((string)ComboBoxChangeUser.SelectedValue));

                StackPanelAssetList.Children.Clear();
                FillingComboBoxUser();         
        }

        private void ButtonDeleteAsset_Click(object sender, RoutedEventArgs e)
        {
            _storage.actualUser.DeleteAsset(_storage.actualAsset);
            LabelCurrentAmount.Content = "";
            StackPanelTransactionList.Children.Clear();
            _storage.actualAsset = null;
            ButtonConfirmTransaction.IsEnabled = false;
            ButtonConfirmTransaction.IsEnabled = false;
            FillAssetsStackPanel();
            FillAssetListBox();
            
        }

        //private void ButtonSpend_Click(object sender, RoutedEventArgs e)
        //{
        //    if (_storage.actualAsset.Amount >= Convert.ToDouble(TextBoxAmount.Text))
        //    {
        //        Transaction nTransaction = new Transaction("-", Convert.ToDouble(TextBoxAmount.Text),
        //                                Convert.ToDateTime(DatePickerTransaction.Text),
        //                                TextBoxComment.Text,
        //                                (string)ComboBoxCategoriesTransaction.SelectedValue);
        //        _storage.actualAsset.AddTransactions(nTransaction);
        //        Button nTransactionButton = new Button();
        //        nTransactionButton.Content = $"{nTransaction.Date} {nTransaction.Sign}{nTransaction.Amount} {nTransaction.Category}";
        //        nTransactionButton.Click += CurrentTransaction;
        //        nTransactionButton.Click += SetTransactionData;
        //        StackPanelTransactionList.Children.Add(nTransactionButton);
        //        LabelCurrentAmount.Content = Convert.ToDouble(LabelCurrentAmount.Content) - nTransaction.Amount;
        //    }
        //    else
        //    {
        //        MessageBox.Show("Сумма операции превышает остаток по выбранному счету");
        //    }
        //}

        //private void ButtonIncome_Click(object sender, RoutedEventArgs e)
        //{
        //    Transaction nTransaction = new Transaction("+", Convert.ToDouble(TextBoxAmount.Text),
        //                                Convert.ToDateTime(DatePickerTransaction.Text),
        //                                TextBoxComment.Text,
        //                                (string)ComboBoxCategoriesTransaction.SelectedValue);
        //    _storage.actualAsset.AddTransactions(nTransaction);
        //    Button nTransactionButton = new Button();
        //    nTransactionButton.Content = $"{nTransaction.Date} {nTransaction.Sign}{nTransaction.Amount} {nTransaction.Category}";
        //    nTransactionButton.Click += CurrentTransaction;
        //    nTransactionButton.Click += SetTransactionData;
        //    StackPanelTransactionList.Children.Add(nTransactionButton);
        //    LabelCurrentAmount.Content = Convert.ToDouble(LabelCurrentAmount.Content) + nTransaction.Amount;
        //}

        private void ButtonAddAsset_Click(object sender, RoutedEventArgs e)
        {
            AddAssetWindow addAssetWindow = new AddAssetWindow(this);
            addAssetWindow.Show();
        }

        private void ButtonDeleteTransaction_Click(object sender, RoutedEventArgs e)
        {
            _storage.actualAsset.DeleteTransaction(_storage.actualTransaction);
            FillingTransactionsStackPanel(sender,e);
            LabelCurrentAmount.Content = Convert.ToString(_storage.actualAsset.GetAmount());
        }

        private void ButtonEditTransaction_Click(object sender, RoutedEventArgs e)  // сделать что-то с доход и расход
        {
            Storage.sign sign = Storage.sign.income;

            if (RadioButtonСonsumption.IsChecked == true)
            {
                sign = Storage.sign.spend;
            }

            _storage.actualTransaction.EditTransaction(sign, Convert.ToDouble(TextBoxAmount.Text),
                                                        Convert.ToDateTime(DatePickerTransaction.Text),
                                                        TextBoxComment.Text,
                                                        ComboBoxCategoriesTransaction.Text);
            FillingTransactionsStackPanel(sender, e);
            LabelCurrentAmount.Content = Convert.ToString(_storage.actualAsset.GetAmount());
        }

        private void ComboBoxChangeUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _storage.actualUser = _storage.GetUserByName(((string)ComboBoxChangeUser.SelectedValue));
            _storage.actualAsset = null;  // Так можно?
            ButtonConfirmTransaction.IsEnabled = false;
            ButtonConfirmTransaction.IsEnabled = false;
            StackPanelAssetList.Children.Clear();
            StackPanelTransactionList.Children.Clear();
            ComboBoxChangeUser_SelectionDone();
            //FillCategories();
            //FillCategoriesIncome();
            FillAssetsStackPanel();
            foreach (Loan loan in _storage.actualUser.Loans)
            {
                loan.DoRegularPayment();
            }
        }

        private void ComboBoxChangeUser_SelectionDone()
        {
            if(ComboBoxChangeUser.Items.Count == 0)
            {
                TabItemAssets.IsEnabled = false;
                TabItemLoans.IsEnabled = false;
                TabItemAnalytics.IsEnabled = false;
                TabItemPlanning.IsEnabled = false;
                TabItemDeposits.IsEnabled = false;
            }
            else
            {
                TabItemAssets.IsEnabled = true;
                TabItemLoans.IsEnabled = true;
                TabItemAnalytics.IsEnabled = true;
                TabItemPlanning.IsEnabled = true;
                TabItemDeposits.IsEnabled = true;
            }
            LabelCurrentAmount.Content = "";
        }

        //private void ButtonDeleteCategory_Click(object sender, RoutedEventArgs e)
        //{
        //    _storage.actualUser.CategoriesSpend.Remove((string)ComboBoxCategoriesTransaction.SelectedValue);
        //    //FillCategories();
        //}

        //private void ButtonAddCategory_Click(object sender, RoutedEventArgs e)
        //{
        //    AddCategories addCategories = new AddCategories(this);
        //    addCategories.Show();
        //}

        //private void ButtonAddCategoryIncome_Click(object sender, RoutedEventArgs e) 
        //{
        //    AddCategories addCategoriesIncome = new AddCategories(this);
        //    addCategoriesIncome.Show();
        //}

        //private void ButtonDeleteCategoryIncome_Click(object sender, RoutedEventArgs e)
        //{
        //    _storage.actualUser.CategoriesIncome.Remove((string)ComboBoxCategoriesTransaction.SelectedValue);
        //    //FillCategoriesIncome();
        //}

        private void ButtonAddCategory_Click(object sender, RoutedEventArgs e)
        {
                AddCategories addCategories = new AddCategories(this);
                addCategories.Show();
        }

        //private void ButtonTransfer_Click(object sender, RoutedEventArgs e)
        //{
        //    if (_storage.actualAsset != null)
        //    {
        //        if (_storage.actualAsset.Amount >= Convert.ToDouble(TextBoxAmount.Text))
        //        {
        //            Asset crntAsset = _storage.actualAsset;
        //            ButtonSpend_Click(_storage.actualAsset, e);
        //            _storage.actualAsset = _storage.actualUser.GetAssetByName(ComboBoxCategoriesTransaction.Text);
        //            ButtonIncome_Click(_storage.actualAsset, e);
        //            _storage.actualAsset = crntAsset;
        //            FillingTransactionsStackPanel(sender, e);
        //            LabelCurrentAmount.Content = _storage.actualAsset.GetAmount();
        //        }
        //        else
        //        {
        //            MessageBox.Show("На выбранном счету недостаточно средств для перевода");
        //        }
        //    }
        //}


        private void ButtoanAddLoan_Click(object sender, RoutedEventArgs e)
        {
            AddLoanWindow addLoanWindow = new AddLoanWindow(this);
            addLoanWindow.Show();
        }

        private void ComboBoxListAsset_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxCategoriesTransaction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBoxComment_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ButtoanRemoveLoan_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButtonIncome_Click(object sender, RoutedEventArgs e)
        {
            FillCategories(_storage.actualUser.CategoriesIncome);
        }

        private void RadioButtonСonsumption_Click(object sender, RoutedEventArgs e)
        {
            FillCategories(_storage.actualUser.CategoriesSpend);
        }

        private void RadioButtonTransfer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if(RadioButtonIncome.IsChecked == true)
            {
                _storage.actualUser.CategoriesIncome.Remove((string)ComboBoxCategoriesTransaction.SelectedValue);
                FillCategories(_storage.actualUser.CategoriesIncome);
            }
            else if(RadioButtonСonsumption.IsChecked == true)
            {
                _storage.actualUser.CategoriesSpend.Remove((string)ComboBoxCategoriesTransaction.SelectedValue);
                FillCategories(_storage.actualUser.CategoriesSpend);
            }
        }

        private void ButtonConfirmTransaction_Click(object sender, RoutedEventArgs e)
        {

            if (RadioButtonСonsumption.IsChecked == true)
            {
                if (_storage.actualAsset.Amount >= Convert.ToDouble(TextBoxAmount.Text))
                {
                    Transaction nTransaction = new Transaction(Storage.sign.spend, Convert.ToDouble(TextBoxAmount.Text),
                                        Convert.ToDateTime(DatePickerTransaction.Text),
                                        TextBoxComment.Text,
                                        (string)ComboBoxCategoriesTransaction.SelectedValue);
                    _storage.actualAsset.AddTransactions(nTransaction);

                    Button nTransactionButton = new Button();
                    nTransactionButton.Content = $"{nTransaction.Date} {nTransaction.Sign}{nTransaction.Amount} {nTransaction.Category}";
                    nTransactionButton.Click += CurrentTransaction;
                    nTransactionButton.Click += SetTransactionData;
                    StackPanelTransactionList.Children.Add(nTransactionButton);
                    LabelCurrentAmount.Content = Convert.ToDouble(LabelCurrentAmount.Content) - nTransaction.Amount;
                    if (_storage.actualAsset is Card)
                    {
                        Card card = (Card)_storage.actualAsset;
                        card.GetMinAmount();
                    }
                }
            }

            else if (RadioButtonIncome.IsChecked == true)
            {
                Transaction nTransaction = new Transaction(Storage.sign.income, Convert.ToDouble(TextBoxAmount.Text),
                                    Convert.ToDateTime(DatePickerTransaction.Text),
                                    TextBoxComment.Text,
                                    (string)ComboBoxCategoriesTransaction.SelectedValue);

                _storage.actualAsset.AddTransactions(nTransaction);
                Button nTransactionButton = new Button();
                nTransactionButton.Content = $"{nTransaction.Date} {nTransaction.Sign}{nTransaction.Amount} {nTransaction.Category}";
                nTransactionButton.Click += CurrentTransaction;
                nTransactionButton.Click += SetTransactionData;
                StackPanelTransactionList.Children.Add(nTransactionButton);
                LabelCurrentAmount.Content = Convert.ToDouble(LabelCurrentAmount.Content) + nTransaction.Amount;
            }
        }

        private void ButtonLoanPayments_Click(object sender, RoutedEventArgs e)
        {
            ViewLoanPaymentsWindow viewLoanPaymentsWindow = new ViewLoanPaymentsWindow();
            viewLoanPaymentsWindow.Show();
        }
    }
}

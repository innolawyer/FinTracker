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
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using FinTracker.Loans;
using FinTracker.Assets;

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
            _storage.GetSave();

            DatePickerTransaction.SelectedDateFormat = DatePickerFormat.Short;
            DatePickerTransaction.SelectedDate = DateTime.Today;

            FillingComboBoxUser(ComboBoxChangeUser);
            ComboBoxChangeUser_SelectionDone();
            FillAssetListBox(ComboBoxCategoriesTransaction);
            FillAssetListBox(ComboBoxAssetAnalisys);
            FillAssetsStackPanel();
            AllLoanButtonsAreEnabled();
            FillingListDeposit();
            FillingListLoans();

            if (_storage.actualAsset == null)
            {
                ButtonConfirmTransaction.IsEnabled = false;
                ButtonConfirmTransaction.IsEnabled = false;
            }
            if (_storage.actualUser != null && _storage.actualAsset != null)
            {
                foreach (AbstractAsset asset in _storage.actualUser.Assets)
                {
                    if (asset is Card)
                    {
                        Card card = (Card)asset;
                        card.GetMinAmount();
                        card.EnrollmentCashbak();
                        card.EnrollmentSumYearInterest();
                        card.EnrollmentServiceFee();
                    }
                    
                }
            }

            ComboBoxRangeDateAnalisys.Items.Add(Storage.DateRange.Месяц);
            ComboBoxRangeDateAnalisys.Items.Add(Storage.DateRange.Полгода);
            ComboBoxRangeDateAnalisys.Items.Add(Storage.DateRange.Год);
            ComboBoxRangeDateAnalisys.SelectedIndex = 0;
            if (_storage.actualUser != null && _storage.actualUser.Assets.Count != 0)
            {
                ComboBoxAssetAnalisys.SelectedIndex = 0;

                SeriesCollectionIncome = Analisys.GetCategoriesSeriesCollectionByAsset(
                    _storage.actualUser.Name,
                    _storage.actualUser.Assets[0].Name,
                    _storage.actualUser.CategoriesIncome,
                    (Storage.DateRange)ComboBoxRangeDateAnalisys.SelectedItem);

                SeriesCollectionSpend = Analisys.GetCategoriesSeriesCollectionByAsset(
                    _storage.actualUser.Name,
                    _storage.actualUser.Assets[0].Name,
                    _storage.actualUser.CategoriesSpend,
                    (Storage.DateRange)ComboBoxRangeDateAnalisys.SelectedItem);

                SeriesCollectionColSpend = Analisys.GetAverageAmountByCategory(_storage.actualUser.CategoriesSpend,
                                                                          (Storage.DateRange)ComboBoxRangeDateAnalisys.SelectedItem,
                                                                         _storage.actualUser.Assets[0].Name);

                SeriesCollectionColIncome = Analisys.GetAverageAmountByCategory(_storage.actualUser.CategoriesIncome,
                                                                          (Storage.DateRange)ComboBoxRangeDateAnalisys.SelectedItem,
                                                                         _storage.actualUser.GetAssetByName(ComboBoxAssetAnalisys.SelectedItem.ToString()).Name);
            }


            DataContext = this;
        }

        public SeriesCollection SeriesCollectionIncome { get; set;}
        public SeriesCollection SeriesCollectionSpend { get; set; }
        public SeriesCollection SeriesCollectionColSpend { get; set; }
        public SeriesCollection SeriesCollectionColIncome { get; set; }

        public void FillingComboBoxUser(ComboBox box)
        {
            box.Items.Clear();  //ComboBoxChangeUser
            foreach (User user in _storage.Users)
            {
                box.Items.Add($"{user.Name}");
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


        public void FillAssetListBox(ComboBox box)
        {
            box.Items.Clear();
            if (_storage.actualUser != null)
            {
                foreach (AbstractAsset asset in _storage.actualUser.Assets)
                {
                    box.Items.Add(asset.Name);
                }
            }
        }

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
                foreach (AbstractAsset asset in _storage.actualUser.Assets)
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

        public void GetAccessToLoans()
        {
            if (_storage.actualUser != null)
            {
                if (_storage.actualUser.Assets.Count != 0)
                {
                    TabItemLoans.IsEnabled = true;
                }
                else
                {
                    TabItemLoans.IsEnabled = false;
                }
            }
        }

        public void LoanLabels_Update()
        {
            Loan loan = (Loan)ListViewLoans.SelectedItem;
            LabelRemainingDays.Content = Convert.ToString((loan.ActualPaymentDateTime - DateTime.Today).TotalDays);
            LabelTotalAmountOfPercents.Content = Math.Round(loan.TotalAmountOfPercents, 2);
        }

        private void ButtonCreateNewUser_Click(object sender, RoutedEventArgs e)
        {
            if (_storage.IsUniqeUser(TextBoxUserName.Text) == true)
            {
                _storage.Users.Add(new User(TextBoxUserName.Text));
                TextBoxUserName.Text = "";
                FillingComboBoxUser(ComboBoxChangeUser);
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
                FillingComboBoxUser(ComboBoxChangeUser);         
        }

        private void ButtonDeleteAsset_Click(object sender, RoutedEventArgs e)
        {
            _storage.actualUser.DeleteAsset((AbstractAsset)_storage.actualAsset);
            LabelCurrentAmount.Content = "";
            StackPanelTransactionList.Children.Clear();
            _storage.actualAsset = null;
            ButtonConfirmTransaction.IsEnabled = false;
            ButtonConfirmTransaction.IsEnabled = false;
            FillAssetsStackPanel();
            FillAssetListBox(ComboBoxCategoriesTransaction);
            GetAccessToLoans();
        }

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

        private void ButtonEditTransaction_Click(object sender, RoutedEventArgs e)
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
            FillAssetsStackPanel();
            FillingListDeposit();
            GetAccessToLoans();
            

            SeriesCollectionIncome = null;
            SeriesCollectionSpend = null;
            ComboBoxRangeDateAnalisys.SelectedIndex = 0;
            if (_storage.actualUser != null && _storage.actualUser.Assets.Count != 0)
            {

                SeriesCollectionIncome = Analisys.GetCategoriesSeriesCollectionByAsset(
                    _storage.actualUser.Name,
                    _storage.actualUser.Assets[0].Name,
                    _storage.actualUser.CategoriesIncome,
                    Storage.DateRange.Месяц);

                SeriesCollectionSpend = Analisys.GetCategoriesSeriesCollectionByAsset(
                    _storage.actualUser.Name,
                    _storage.actualUser.Assets[0].Name,
                    _storage.actualUser.CategoriesSpend,
                    Storage.DateRange.Месяц);

                PieChartIncome.Update();
                PieChartSpend.Update();
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

        private void ButtonAddCategory_Click(object sender, RoutedEventArgs e)
        {
                AddCategories addCategories = new AddCategories(this);
                addCategories.Show();
        }


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
            _storage.actualUser.RemoveLoan((Loan)ListViewLoans.SelectedItem);
            ListViewLoans.Items.Remove(ListViewLoans.SelectedItem);
            ListViewLoans.Items.Refresh();
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

       

        private void ListViewLoans_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AllLoanButtonsAreEnabled();
            LoanLabels_Update();
           

        }

        private void ButtonAddExtraPayment_Click(object sender, RoutedEventArgs e)
        {
            AddExtraPayment addExtraPayment = new AddExtraPayment(this);
            addExtraPayment.Show();
        }

        private void AllLoanButtonsAreEnabled()
        {
            if (ListViewLoans.SelectedItem == null)
            {
                ButtoanEditLoan.IsEnabled = false;
                ButtoanEditLoan.Opacity = 0;
                ButtoanRemoveLoan.IsEnabled = false;
                ButtoanRemoveLoan.Opacity = 0;                
                ButtonAddExtraPayment.IsEnabled = false;
                ButtonAddExtraPayment.Opacity = 0;
                RectangleLW.Opacity = 0;
                ListViewLoanPayments.IsEnabled = false;
                ListViewLoanPayments.Opacity = 0;
                LabelRemainingDays.Opacity = 0;
                LabelTAOP.Opacity = 0;
                LabelTotalAmountOfPercents.Opacity = 0;
                LabelRub.Opacity = 0;
                LabelUntilPayment.Opacity = 0;
                LabelDN.Opacity = 0;
                LabelTextPayments.Opacity = 0;
                

            }
            else if (ListViewLoans.SelectedItem !=null)
            {
                ButtoanEditLoan.IsEnabled = true;
                ButtoanEditLoan.Opacity = 1;
                ButtoanRemoveLoan.IsEnabled = true;
                ButtoanRemoveLoan.Opacity = 1;                
                ButtonAddExtraPayment.IsEnabled = true;
                ButtonAddExtraPayment.Opacity = 1;
                RectangleLW.Opacity = 1;
                ListViewLoanPayments.IsEnabled = true;
                ListViewLoanPayments.Opacity = 1;
                LabelRemainingDays.Opacity = 1;
                LabelTAOP.Opacity = 1;
                LabelTotalAmountOfPercents.Opacity = 1;
                LabelRub.Opacity = 1;
                LabelUntilPayment.Opacity = 1;
                LabelDN.Opacity = 1;
                LabelTextPayments.Opacity = 1;
            }
        }

        private void ButtoanEditLoan_Click(object sender, RoutedEventArgs e)
        {
            EditLoanWindow editLoanWindow = new EditLoanWindow(this);
            editLoanWindow.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _storage.Save();
        }

        private void ComboBoxAssetAnalisys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PieChartIncome.Series != null && PieChartSpend.Series != null && _storage.actualUser.Assets.Count != 0)
            {
                PieChartIncome.Series.Clear();
                PieChartSpend.Series.Clear();
                ColumnChartSpend.Series.Clear();
                ColumnChartIncome.Series.Clear();

                SeriesCollectionIncome = Analisys.GetCategoriesSeriesCollectionByAsset(
                    _storage.actualUser.Name,
                    _storage.actualUser.GetAssetByName(ComboBoxAssetAnalisys.SelectedItem.ToString()).Name,
                    _storage.actualUser.CategoriesIncome,
                    (Storage.DateRange)ComboBoxRangeDateAnalisys.SelectedItem);

                for (int i = 0; i < SeriesCollectionIncome.Count; i++)
                {
                    PieChartIncome.Series.Add(SeriesCollectionIncome[i]);
                }

                SeriesCollectionSpend = Analisys.GetCategoriesSeriesCollectionByAsset(
                    _storage.actualUser.Name,
                    _storage.actualUser.GetAssetByName(ComboBoxAssetAnalisys.SelectedItem.ToString()).Name,
                    _storage.actualUser.CategoriesSpend,
                    (Storage.DateRange)ComboBoxRangeDateAnalisys.SelectedItem);

                for (int i = 0; i < SeriesCollectionSpend.Count; i++)
                {
                    PieChartSpend.Series.Add(SeriesCollectionSpend[i]);
                }

                SeriesCollectionColSpend = Analisys.GetAverageAmountByCategory(_storage.actualUser.CategoriesSpend,
                                                                          (Storage.DateRange)ComboBoxRangeDateAnalisys.SelectedItem,
                                                                         _storage.actualUser.GetAssetByName(ComboBoxAssetAnalisys.SelectedItem.ToString()).Name);

                for (int i = 0; i < SeriesCollectionColSpend.Count; i++)
                {
                    ColumnChartSpend.Series.Add(SeriesCollectionColSpend[i]);
                }

                SeriesCollectionColIncome = Analisys.GetAverageAmountByCategory(_storage.actualUser.CategoriesIncome,
                                                                          (Storage.DateRange)ComboBoxRangeDateAnalisys.SelectedItem,
                                                                         _storage.actualUser.GetAssetByName(ComboBoxAssetAnalisys.SelectedItem.ToString()).Name);

                for (int i = 0; i < SeriesCollectionColIncome.Count; i++)
                {
                    ColumnChartIncome.Series.Add(SeriesCollectionColIncome[i]);
                }
            }


            PieChartIncome.Update(true, true);
            PieChartSpend.Update(true, true);
            ColumnChartSpend.Update();
            ColumnChartIncome.Update();
        }

        private void ComboBoxAssetAnalisys_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonEditAsset_Click(object sender, RoutedEventArgs e)
        {
            EditAsset editAsset = new EditAsset(this);
            editAsset.Show();
        }      

        private void ButtonCreateDeposit_Click(object sender, RoutedEventArgs e)
        {
            AddDeposit addDeposit = new AddDeposit(this);
            addDeposit.Show();
        }

        public void FillingListDeposit()
        {
            ListViewDeposit.Items.Clear();

            if (_storage.actualUser != null)
            {
                foreach (AbstractAsset asset in _storage.actualUser.Assets)
                {
                    if (asset is Deposit)
                    {
                        Deposit depo = (Deposit)asset;
                        ListViewDeposit.Items.Add(depo);
                    }
                }
            }
        }

        public void FillingListLoans()
        {
            ListViewLoans.Items.Clear();

            if (_storage.actualUser != null)
            {
                foreach (Loan loan in _storage.actualUser.Loans)
                {                    
                  ListViewLoans.Items.Add(loan);                    
                }
            }
        }
    }
}
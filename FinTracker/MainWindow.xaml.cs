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
        Storage storage = Storage.GetStorage();

        public MainWindow()
        {
            InitializeComponent();

           // StackPanelTransactionList.Children.Add(new Label());
            DatePickerTransaction.SelectedDateFormat = DatePickerFormat.Short;
            DatePickerTransaction.SelectedDate = DateTime.Today;
            
            FillingComboBoxUser();
            ComboBoxChangeUser_SelectionDone();
            FillCategoriesIncome();
            FillCategories();
            FillAssetListBox();
            FillAssetsStackPanel();
            
            if (storage.actualAsset == null)
            {
                ButtonIncome.IsEnabled = false;
                ButtonSpend.IsEnabled = false;
            }

        }

        public void FillingComboBoxUser()
        {
            ComboBoxChangeUser.Items.Clear();
            foreach (User user in storage.Users)
            {
                ComboBoxChangeUser.Items.Add($"{user.Name}");
            }
        }
        
        public void LabelCurrentAmount_Display(object sender, RoutedEventArgs e)
        {
            LabelCurrentAmount.Content = storage.actualAsset.GetAmount();
        }

        public void CurrentTransaction(object sender, RoutedEventArgs e)
        {
            storage.actualTransaction = storage.actualAsset.Transactions[StackPanelTransactionList.Children.IndexOf((Button)sender)];
        }

        public void FillCategoriesIncome()
        {
            ComboBoxCategoriesIncome.Items.Clear();
            if (storage.actualUser != null)
            {
                foreach (string category in storage.actualUser.CategoriesIncome)
                {
                    ComboBoxCategoriesIncome.Items.Add(category);
                }
            }
        }

        public void FillAssetListBox()
        {
            ComboBoxListAsset.Items.Clear();
            if (storage.actualUser != null)
            {
                foreach (Asset asset in storage.actualUser.Assets)
                {
                    ComboBoxListAsset.Items.Add(asset.Name);
                }
            }
        }

        public void FillCategories()
        {
            ComboBoxCategoriesTransaction.Items.Clear();
            if (storage.actualUser != null)
            {
                foreach (string category in storage.actualUser.CategoriesSpend)
                {
                    ComboBoxCategoriesTransaction.Items.Add(category);
                }
            }
        }

        public void FillingTransactionsStackPanel(object sender, RoutedEventArgs e)
        {
            StackPanelTransactionList.Children.Clear();
            foreach (Transaction transaction in storage.actualAsset.Transactions)
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
            DatePickerTransaction.Text = storage.actualTransaction.Date.ToString();
            TextBoxAmount.Text = storage.actualTransaction.Amount.ToString();
            ComboBoxCategoriesTransaction.Text = storage.actualTransaction.Category.ToString();
            TextBoxComment.Text = storage.actualTransaction.Comment.ToString();
        }

        public void SetActualAsset(object sender, RoutedEventArgs e)
        {
            storage.actualAsset = storage.actualUser.GetAssetByName(Convert.ToString(((Button)sender).Content));
        }

        public void FillAssetsStackPanel()
        {
            if (storage.actualUser != null)
            {
                StackPanelAssetList.Children.Clear();
                foreach (Asset asset in storage.actualUser.Assets)
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
            if (storage.actualAsset == null)
            {
                ButtonIncome.IsEnabled = false;
                ButtonSpend.IsEnabled = false;
            }
            else
            {
                ButtonIncome.IsEnabled = true;
                ButtonSpend.IsEnabled = true;
            }
        }

        private void ButtonCreateNewUser_Click(object sender, RoutedEventArgs e) //!!!
        {
            if (storage.IsUniqeUser(TextBoxUserName.Text) == true)
            {
                User user = new User(TextBoxUserName.Text);
                storage.Users.Add(user);
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
                storage.DeleteUser(((string)ComboBoxChangeUser.SelectedValue));

                StackPanelAssetList.Children.Clear();
                FillingComboBoxUser();         
        }

        private void ButtonDeleteAsset_Click(object sender, RoutedEventArgs e)
        {
            storage.actualUser.DeleteAsset(storage.actualAsset);
            LabelCurrentAmount.Content = "";
            StackPanelTransactionList.Children.Clear();
            storage.actualAsset = null;
            ButtonIncome.IsEnabled = false;
            ButtonSpend.IsEnabled = false;
            FillAssetsStackPanel();
            FillAssetListBox();
            
        }

        private void ButtonSpend_Click(object sender, RoutedEventArgs e)
        {
            if (storage.actualAsset.Amount >= Convert.ToDouble(TextBoxAmount.Text))
            {
                Transaction nTransaction = new Transaction("-", Convert.ToDouble(TextBoxAmount.Text),
                                        Convert.ToDateTime(DatePickerTransaction.Text),
                                        TextBoxComment.Text,
                                        (string)ComboBoxCategoriesTransaction.SelectedValue);
                storage.actualAsset.AddTransactions(nTransaction);
                Button nTransactionButton = new Button();
                nTransactionButton.Content = $"{nTransaction.Date} {nTransaction.Sign}{nTransaction.Amount} {nTransaction.Category}";
                nTransactionButton.Click += CurrentTransaction;
                nTransactionButton.Click += SetTransactionData;
                StackPanelTransactionList.Children.Add(nTransactionButton);
                LabelCurrentAmount.Content = Convert.ToDouble(LabelCurrentAmount.Content) - nTransaction.Amount;
            }
            else
            {
                MessageBox.Show("Сумма операции превышает остаток по выбранному счету");
            }
        }

        private void ButtonIncome_Click(object sender, RoutedEventArgs e)
        {
            Transaction nTransaction = new Transaction("+", Convert.ToDouble(TextBoxAmount.Text),
                                        Convert.ToDateTime(DatePickerTransaction.Text),
                                        TextBoxComment.Text,
                                        (string)ComboBoxCategoriesIncome.SelectedValue);
            storage.actualAsset.AddTransactions(nTransaction);
            Button nTransactionButton = new Button();
            nTransactionButton.Content = $"{nTransaction.Date} {nTransaction.Sign}{nTransaction.Amount} {nTransaction.Category}";
            nTransactionButton.Click += CurrentTransaction;
            nTransactionButton.Click += SetTransactionData;
            StackPanelTransactionList.Children.Add(nTransactionButton);
            LabelCurrentAmount.Content = Convert.ToDouble(LabelCurrentAmount.Content) + nTransaction.Amount;
        }

        private void ButtonAddAsset_Click(object sender, RoutedEventArgs e)
        {
            AddAssetWindow addAssetWindow = new AddAssetWindow(this);
            addAssetWindow.Show();
        }

        private void ButtonDeleteTransaction_Click(object sender, RoutedEventArgs e)
        {
            storage.actualAsset.DeleteTransaction(storage.actualTransaction);
            FillingTransactionsStackPanel(sender,e);
        }

        private void ButtonEditTransaction_Click(object sender, RoutedEventArgs e)      // сделать что-то с доход и расход
        {
            storage.actualTransaction.EditTransaction(Convert.ToDouble(TextBoxAmount.Text), Convert.ToDateTime(DatePickerTransaction.Text), TextBoxComment.Text, ComboBoxCategoriesTransaction.Text);
            FillingTransactionsStackPanel(sender, e);
            LabelCurrentAmount.Content = storage.actualAsset.GetAmount().ToString();
        }

        private void ComboBoxChangeUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            storage.actualUser = storage.GetUserByName(((string)ComboBoxChangeUser.SelectedValue));
            storage.actualAsset = null;  // Так можно?
            ButtonIncome.IsEnabled = false;
            ButtonSpend.IsEnabled = false;
            StackPanelAssetList.Children.Clear();
            StackPanelTransactionList.Children.Clear();
            ComboBoxChangeUser_SelectionDone();
            FillCategories();
            FillCategoriesIncome();
            FillAssetsStackPanel();
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
        }

        private void ButtonDeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            storage.actualUser.CategoriesSpend.Remove((string)ComboBoxCategoriesTransaction.SelectedValue);
            FillCategories();
        }

        private void ButtonAddCategory_Click(object sender, RoutedEventArgs e)
        {
            AddCategories addCategories = new AddCategories(this);
            addCategories.Show();
        }

        

        private void ButtonAddCategoryIncome_Click(object sender, RoutedEventArgs e) 
        {
            AddCategoriesIncome addCategoriesIncome = new AddCategoriesIncome(this);
            addCategoriesIncome.Show();
        }

        private void ButtonDeleteCategoryIncome_Click(object sender, RoutedEventArgs e)
        {
            storage.actualUser.CategoriesIncome.Remove((string)ComboBoxCategoriesIncome.SelectedValue);
            FillCategoriesIncome();
        }

        private void ButtonTransfer_Click(object sender, RoutedEventArgs e)
        {
            if (storage.actualAsset != null)
            {
                if (storage.actualAsset.Amount >= Convert.ToDouble(TextBoxAmount.Text))
                {
                    Asset crntAsset = storage.actualAsset;
                    ButtonSpend_Click(storage.actualAsset, e);
                    storage.actualAsset = storage.actualUser.GetAssetByName(ComboBoxListAsset.Text);
                    ButtonIncome_Click(storage.actualAsset, e);
                    storage.actualAsset = crntAsset;
                    FillingTransactionsStackPanel(sender, e);
                    LabelCurrentAmount.Content = storage.actualAsset.GetAmount();
                }
                else
                {
                    MessageBox.Show("На выбранном счету недостаточно средств для перевода");
                }
            }

        } // работает, но не факт, что правильно
    }
}

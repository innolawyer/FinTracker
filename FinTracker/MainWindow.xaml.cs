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
        public List<User> Users = new List<User>();
        public User actualUser;
        public Asset actualAsset;
        public Transaction actualTransaction;

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
            
            if (actualAsset == null)
            {
                ButtonIncome.IsEnabled = false;
                ButtonSpend.IsEnabled = false;
            }

        }

        public void FillingComboBoxUser()
        {
            ComboBoxChangeUser.Items.Clear();
            foreach (User user in Users)
            {
                ComboBoxChangeUser.Items.Add($"{user.Name}");
            }
        }

        public User GetUserByName(string name)
        {
            foreach (User  user in Users)
            {
                if (user.Name == name)
                {
                    return user;
                }
            }
            return null; // Подумать над этим
        }
       
        public Asset GetAssetByName(string name)
        {
            foreach (Asset asset in actualUser.Assets)
            {
                if (asset.Name == name)
                {
                    return asset;
                }
            }
            return null; // Подумать над этим
        }
        
        public void LabelCurrentAmount_Display(object sender, RoutedEventArgs e)
        {
            LabelCurrentAmount.Content = actualAsset.GetAmount();
        }

        public void CurrentTransaction(object sender, RoutedEventArgs e)
        {
            actualTransaction = actualAsset.Transactions[StackPanelTransactionList.Children.IndexOf((Button)sender)];
        }

        public void FillCategoriesIncome()
        {
            ComboBoxCategoriesIncome.Items.Clear();
            if (actualUser != null)
            {
                foreach (string category in actualUser.CategoriesIncome)
                {
                    ComboBoxCategoriesIncome.Items.Add(category);
                }
            }
        } //не заполняет, только после добавления категории через +

        public void FillAssetListBox ()
        {
            ComboBoxListAsset.Items.Clear();
            if (actualUser != null)
            {
                foreach (Asset asset in actualUser.Assets)
                {
                    ComboBoxListAsset.Items.Add(asset.Name);
                }
            }
        }

        public void FillCategories()
        {
            ComboBoxCategoriesTransaction.Items.Clear();
            if (actualUser != null)
            {
                foreach (string category in actualUser.CategoriesSpend)
                {
                    ComboBoxCategoriesTransaction.Items.Add(category);
                }
            }
        }

        public void FillingTransactionsStackPanel(object sender, RoutedEventArgs e)
        {
            StackPanelTransactionList.Children.Clear();
            foreach (Transaction transaction in actualAsset.Transactions)
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
            DatePickerTransaction.Text = actualTransaction.Date.ToString();
            TextBoxAmount.Text = actualTransaction.Amount.ToString();
            ComboBoxCategoriesTransaction.Text = actualTransaction.Category.ToString();
            TextBoxComment.Text = actualTransaction.Comment.ToString();
        }

        public void SetActualAsset(object sender, RoutedEventArgs e)
        {
            actualAsset = GetAssetByName(Convert.ToString(((Button)sender).Content));
        }

        public void FillAssetsStackPanel()
        {
            if (actualUser != null)
            {
                StackPanelAssetList.Children.Clear();
                foreach (Asset asset in actualUser.Assets)
                {
                    Button buttonAsset = new Button();
                    buttonAsset.Content = asset.Name;
                    buttonAsset.Click += SetActualAsset;
                    buttonAsset.Click += LabelCurrentAmount_Display;
                    buttonAsset.Click += AddTransactionVisibility;
                    buttonAsset.Click += FillingTransactionsStackPanel;

                    StackPanelAssetList.Children.Add(buttonAsset);
                }
            }
        }

        public void AddTransactionVisibility(object sender, RoutedEventArgs e)
        {
            if (actualAsset == null)
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

        private void ButtonCreateNewUser_Click(object sender, RoutedEventArgs e)
        {
            if (IsUniqeUser(TextBoxUserName.Text) == true)
            {
                User user = new User(TextBoxUserName.Text);
                Users.Add(user);
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
                User user = GetUserByName(((string)ComboBoxChangeUser.SelectedValue));
            
                Users.Remove(user);
                //actualUser = null;
                StackPanelAssetList.Children.Clear();
                FillingComboBoxUser();         
        }

        private void ButtonDeleteAsset_Click(object sender, RoutedEventArgs e)
        {
            actualUser.Assets.Remove(actualAsset);
            LabelCurrentAmount.Content = "";
            StackPanelTransactionList.Children.Clear();
            actualAsset = null;
            ButtonIncome.IsEnabled = false;
            ButtonSpend.IsEnabled = false;
            FillAssetsStackPanel();
            FillAssetListBox();
            
        }

        private void ButtonSpend_Click(object sender, RoutedEventArgs e)
        {
            if (actualAsset.Amount >= Convert.ToDouble(TextBoxAmount.Text))
            {
                Transaction nTransaction = new Transaction("-", Convert.ToDouble(TextBoxAmount.Text),
                                        Convert.ToDateTime(DatePickerTransaction.Text),
                                        TextBoxComment.Text,
                                        (string)ComboBoxCategoriesTransaction.SelectedValue);
                actualAsset.AddTransactions(nTransaction);
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
            actualAsset.AddTransactions(nTransaction);
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
            actualAsset.DeleteTransaction(actualTransaction);
            FillingTransactionsStackPanel(sender,e);
        }

        private void ComboBoxChangeUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            actualUser = GetUserByName(((string)ComboBoxChangeUser.SelectedValue));
            actualAsset = null;  // Так можно?
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
            actualUser.CategoriesSpend.Remove((string)ComboBoxCategoriesTransaction.SelectedValue);
            FillCategories();
            //for (int i = 0; i <  StackPanelAssetList.Children.Count; i++)
            //{
            //    if (((Button)StackPanelAssetList.Children[i]).Text == actualAsset.Name);
            //}
            //actualAsset = null;
        }

        private void ButtonAddCategory_Click(object sender, RoutedEventArgs e)
        {
            AddCategories addCategories = new AddCategories(this);
            addCategories.Show();
        }

        public bool IsUniqeUser(string name)
        {
            bool uniq = true;
            foreach(User user in Users)
            {
                if (name == user.Name)
                {
                    uniq = false;
                }
            }
            return uniq;
        }

        private void ButtonAddCategoryIncome_Click(object sender, RoutedEventArgs e) 
        {
            AddCategoriesIncome addCategoriesIncome = new AddCategoriesIncome(this);
            addCategoriesIncome.Show();
        }

        private void ButtonDeleteCategoryIncome_Click(object sender, RoutedEventArgs e)
        {
            actualUser.CategoriesIncome.Remove((string)ComboBoxCategoriesIncome.SelectedValue);
            FillCategoriesIncome();
        }

        private void ButtonTransfer_Click(object sender, RoutedEventArgs e)
        {
            if (actualAsset != null)
            {
                if (actualAsset.Amount >= Convert.ToDouble(TextBoxAmount.Text))
                {
                    Asset crntAsset = actualAsset;
                    ButtonSpend_Click(actualAsset, e);
                    actualAsset = GetAssetByName(ComboBoxListAsset.Text);
                    ButtonIncome_Click(actualAsset, e);
                    actualAsset = crntAsset;
                    FillingTransactionsStackPanel(sender, e);
                    LabelCurrentAmount.Content = actualAsset.GetAmount();
                }
                else
                {
                    MessageBox.Show("На выбранном счету недостаточно средств для перевода");
                }
            }

        } // работает, но не факт, что правильно
    }
}

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
        public User actualUser = new User("admin"); // убрать хардкод
        //public static User user = new User("admin"); // просто проверка, потом убрать
        public Asset actualAsset = new Asset("firstAsset", 0);


        public MainWindow()
        {
            InitializeComponent();

            DatePickerTransaction.SelectedDateFormat = DatePickerFormat.Short;
            DatePickerTransaction.SelectedDate = DateTime.Today;

            foreach (string category in actualUser.Categories)
            {
                ComboBoxCategoriesTransaction.Items.Add(category);
            }
            //Users.Add(user); // проверка, убрать
            FillingComboBoxUser();
        }

        public void FillingComboBoxUser()
        {
            ComboBoxChangeUser.Items.Clear();
            foreach (User user in Users)
            {
                ComboBoxChangeUser.Items.Add($"{user.Name}");
            }
        }

        public void FillingTransactionsStackPanel()
        {
            StackPanelTransactionList.Children.Clear();
            foreach (Transaction transaction in actualAsset.Transactions)
            {
                Button nTransactionButton = new Button();
                nTransactionButton.Content = $"{transaction.Date} {transaction.Sign}{transaction.Amount} {transaction.Category}";
                StackPanelTransactionList.Children.Add(nTransactionButton);

                StackPanelTransactionList.Children.Add(nTransactionButton);
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

        private void ButtonCreateNewUser_Click(object sender, RoutedEventArgs e)
        {
            User user = new User(TextBoxUserName.Text);
            Users.Add(user);
            FillingComboBoxUser();
        }

        private void ButtonSpend_Click(object sender, RoutedEventArgs e)
        {
            Transaction nTransaction = new Transaction("-", Convert.ToDouble(TextBoxAmount.Text),
                                        Convert.ToDateTime(DatePickerTransaction.Text),
                                        TextBoxComment.Text,
                                        (string)ComboBoxCategoriesTransaction.SelectedValue);
            actualAsset.AddTransactions(nTransaction);
            Button nTransactionButton = new Button();
            nTransactionButton.Content = $"{nTransaction.Date} {nTransaction.Sign}{nTransaction.Amount} {nTransaction.Category}";
            StackPanelTransactionList.Children.Add(nTransactionButton);
        }

        private void ButtonIncome_Click(object sender, RoutedEventArgs e) // категории доходов должны быть другие
        {
            Transaction nTransaction = new Transaction("+", Convert.ToDouble(TextBoxAmount.Text),
                                        Convert.ToDateTime(DatePickerTransaction.Text),
                                        TextBoxComment.Text,
                                        (string)ComboBoxCategoriesTransaction.SelectedValue);
            actualAsset.AddTransactions(nTransaction);
            Button nTransactionButton = new Button();
            nTransactionButton.Content = $"{nTransaction.Date} {nTransaction.Sign}{nTransaction.Amount} {nTransaction.Category}";
            StackPanelTransactionList.Children.Add(nTransactionButton);
        }

        private void ButtonAddAsset_Click(object sender, RoutedEventArgs e)
        {
            AddAssetWindow addAssetWindow = new AddAssetWindow(this);
            addAssetWindow.Show();
        }

        private void ComboBoxChangeUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            actualUser = GetUserByName(ComboBoxChangeUser.SelectedItem.ToString());
            actualAsset = null; // Так можно?


        }
    }
}

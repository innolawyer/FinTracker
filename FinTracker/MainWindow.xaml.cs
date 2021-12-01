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
        public static User user = new User("admin"); // просто проверка, потом убрать
        public Asset actualAsset = new Asset("firstAsset", 0);

        public MainWindow()
        {
            InitializeComponent();
            DatePickerTransaction.SelectedDateFormat = DatePickerFormat.Short;
            DatePickerTransaction.SelectedDate = DateTime.Today;
            Users.Add(user); // проверка, убрать
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
                                        "qwe"); //ComboBoxCategoriesTransaction.SelectedValue.ToString()

            actualAsset.AddTransactions(nTransaction);

            Button nTransactionButton = new Button();
            nTransactionButton.Content = $"{nTransaction.Date} {nTransaction.Category} {nTransaction.Amount}";
            StackPanelTransactionList.Children.Add(nTransactionButton);
        }
    }
}

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


        public MainWindow()
        {
            InitializeComponent();

            DatePickerTransaction.SelectedDateFormat = DatePickerFormat.Short;
            DatePickerTransaction.SelectedDate = DateTime.Today;
            
            ComboBoxChangeUser_SelectionDone();
            //AddTransactionVisibility();
            if (actualAsset == null)
            {
                ButtonIncome.IsEnabled = false;
                ButtonSpend.IsEnabled = false;
            }

            FillingComboBoxUser();
            FillCategories();
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

        public void FillCategories()
        {
            ComboBoxCategoriesTransaction.Items.Clear();
            if (actualUser != null)
            {
                foreach (string category in actualUser.Categories)
                {
                    ComboBoxCategoriesTransaction.Items.Add(category);
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
            LabelCurrentAmount.Content = Convert.ToDouble(LabelCurrentAmount.Content) - nTransaction.Amount;
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
            LabelCurrentAmount.Content = Convert.ToDouble(LabelCurrentAmount.Content) + nTransaction.Amount;
        }

        private void ButtonAddAsset_Click(object sender, RoutedEventArgs e)
        {
            AddAssetWindow addAssetWindow = new AddAssetWindow(this);
            addAssetWindow.Show();
        }

        private void ComboBoxChangeUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            actualUser = GetUserByName(((string)ComboBoxChangeUser.SelectedValue));
            actualAsset = null; ; // Так можно?
            ButtonIncome.IsEnabled = false;
            ButtonSpend.IsEnabled = false;
            StackPanelAssetList.Children.Clear();
            StackPanelTransactionList.Children.Clear();
            ComboBoxChangeUser_SelectionDone();
            FillCategories();
        }

        private void ComboBoxChangeUser_SelectionDone()
        {
            if(ComboBoxChangeUser.SelectedIndex == -1)
            {
                TabItemAssets.IsEnabled = false;
                TabItemRegularPayments.IsEnabled = false;
                TabItemAnalytics.IsEnabled = false;
            }
            else
            {
                TabItemAssets.IsEnabled = true;
                TabItemRegularPayments.IsEnabled = true;
                TabItemAnalytics.IsEnabled = true;
            }
        }

        private void ButtonDeleteUser_Copy_Click(object sender, RoutedEventArgs e)
        {           
                User user = GetUserByName(((string)ComboBoxChangeUser.SelectedValue));
                Users.Remove(user);
                FillingComboBoxUser();         
        }

        private void ButtonDeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            actualUser.Categories.Remove((string)ComboBoxCategoriesTransaction.SelectedValue);
            FillCategories();
        }

        private void ButtonAddCategory_Click(object sender, RoutedEventArgs e)
        {
            AddCategories addCategories = new AddCategories(this);
            addCategories.Show();
        }
    }
}

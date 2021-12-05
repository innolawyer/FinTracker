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
    /// Interaction logic for AddCategoriesIncome.xaml
    /// </summary>
    public partial class AddCategoriesIncome : Window
    {
        private Storage _storage = Storage.GetStorage();
        MainWindow _mainWindow;
        public AddCategoriesIncome(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

        }

        private void ButtonSaveCategoryIncome_Click(object sender, RoutedEventArgs e)
        {
            _storage.actualUser.CategoriesIncome.Add(TextBoxNewCategoryIncome.Text);
            _mainWindow.FillCategoriesIncome();
            this.Close();
        }

    }
}

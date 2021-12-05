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
    /// Interaction logic for AddCategories.xaml
    /// </summary>
    public partial class AddCategories : Window
    {
        private Storage storage = Storage.GetStorage();
        MainWindow _mainWindow;
        public AddCategories(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

        }

        private void ButtonSaveCategory_Click(object sender, RoutedEventArgs e)
        {
            storage.actualUser.CategoriesSpend.Add(TextBoxNewCategory.Text);
            _mainWindow.FillCategories();
            this.Close();
        }
    }
}

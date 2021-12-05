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
    /// Interaction logic for AddLoanWindow.xaml
    /// </summary>
    public partial class AddLoanWindow : Window
    {
        private Storage _storage = Storage.GetStorage();
        MainWindow _mainWindow;
        public AddLoanWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            FillingComboboxLoanStatus();
            _mainWindow = mainWindow;

        }

        public void FillingComboboxLoanStatus ()
        {
            ComboBoxLoanStatus.Items.Add("Не выплачен");
            ComboBoxLoanStatus.Items.Add("Выплачен");
            
        }
        

        public void ButtonCreateLoan_Click(object sender, RoutedEventArgs e)
        {
            User user = _storage.actualUser;            
            Loan nLoan = new Loan ((DateTime)Convert.ToDateTime(DatePickerLoanStart.Text), (String)TextBoxLoanCreditorName.Text,
                (Double)Convert.ToDouble(TextBoxLoanPercent.Text), (Double)Convert.ToDouble (TextBoxLoanPeriod.Text),
                (String)Convert.ToString(ComboBoxLoanStatus.SelectedIndex), (Double)Convert.ToDouble (TextBoxLoanPeriod.Text),
                (Double)Convert.ToDouble(TextBoxLoanAmountOfReturned.Text));
                     
        }

        
    }
}

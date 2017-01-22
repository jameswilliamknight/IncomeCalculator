using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IncomeCalculator.Enums;

namespace IncomeCalculator
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // methods
        private void Output(string text)
        {
            OutputText += text + "\n";
        }

        #region Calculate Button

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //Output("testing");
            Calculate();
        }

        private void Calculate()
        {
            var calcOne = true;
            var calcTwo = true;

            if (string.IsNullOrWhiteSpace(tbOne.Text))
            {
                calcOne = false;
                tbOldJobBase.Text = "/";
                tbOldJobMonthly.Text = "-";
                tbOldJobFortnightly.Text = "-";
                tbOldJobWeekly.Text = "-";
            }
                
            if(string.IsNullOrWhiteSpace(tbTwo.Text))
            {
                calcTwo = false;
                tbNewJobBase.Text = "/";
                tbNewJobMonthly.Text = "-";
                tbNewJobFortnightly.Text = "-";
                tbNewJobWeekly.Text = "-";
            }

            if (!calcOne && !calcTwo)
            {
                return;
            }

            if (calcOne) IncomeOne = IncomeFactory.Create(tbOne, cbOne, StudentLoanRepayments);
            if (calcTwo) IncomeTwo = IncomeFactory.Create(tbTwo, cbTwo, StudentLoanRepayments);

            // base salary output.
            if (calcOne) tbOldJobBase.Text = $"{IncomeOne.BaseSalary.ToString("##.")} / {IncomeOne.BaseSalaryTaxedMinusStudentLoanDeductions.ToString("##.")}";
            if (calcTwo) tbNewJobBase.Text = $"{IncomeTwo.BaseSalary.ToString("##.")} / {IncomeTwo.BaseSalaryTaxedMinusStudentLoanDeductions.ToString("##.")}";

            // monthly salary output.
            if (calcOne) tbOldJobMonthly.Text = (IncomeOne.BaseSalaryTaxedMinusStudentLoanDeductions / 12).ToString("##.00");
            if (calcTwo) tbNewJobMonthly.Text = (IncomeTwo.BaseSalaryTaxedMinusStudentLoanDeductions / 12).ToString("##.00");

            // fortnightly salary output.
            if (calcOne) tbOldJobFortnightly.Text = (IncomeOne.BaseSalaryTaxedMinusStudentLoanDeductions / 26).ToString("##.00");
            if (calcTwo) tbNewJobFortnightly.Text = (IncomeTwo.BaseSalaryTaxedMinusStudentLoanDeductions / 26).ToString("##.00");

            // weekly salary output.
            if (calcOne) tbOldJobWeekly.Text = (IncomeOne.BaseSalaryTaxedMinusStudentLoanDeductions / 52).ToString("##.00");
            if (calcTwo) tbNewJobWeekly.Text = (IncomeTwo.BaseSalaryTaxedMinusStudentLoanDeductions / 52).ToString("##.00");
        }

        #endregion

        public Income IncomeOne { get; set; }
        public Income IncomeTwo { get; set; }

        private string jobOneIncome;
        private string jobTwoIncome;

        private IncomePeriod jobOnePeriod;
        private IncomePeriod jobTwoPeriod;

        private bool studentLoanRepay = true;
        public bool StudentLoanRepayments {
            get
            {
                return studentLoanRepay;
            }
            set
            {
                studentLoanRepay = value;
            }
        }

        #region Text Output Area

        private string outputText;

        public string OutputText
        {
            get { return outputText; }
            set
            {
                outputText = value;
                OnPropertyChanged();
            }
        }

        #endregion
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            OutputText = "testing";

            cbOne.ItemsSource = Enum.GetValues(typeof(IncomePeriod)).Cast<IncomePeriod>();
            cbOne.SelectedIndex = 0;

            cbTwo.ItemsSource = Enum.GetValues(typeof(IncomePeriod)).Cast<IncomePeriod>();
            cbTwo.SelectedIndex = 0;

        }
        
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        /// <summary>
        /// Stops non-numeric characters from being entered into the textboxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if ( !char.IsDigit(e.Text, e.Text.Length - 1) && sender is TextBox )
            {
                // Non digit found

                if ( e.Text == "." )
                {
                    // Character is a decimal place.

                    if ( ((TextBox)sender).Text.Contains(".") )
                    {
                        // we already have a decimal place entered.
                        e.Handled = true;
                    }

                    return;
                }
                e.Handled = true;
            }
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Calculate();
        }

        private void StudentLoan_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Calculate();
        }

        private void RangeChanged(object sender, SelectionChangedEventArgs e)
        {
            Calculate();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using IncomeCalculator.Enums;

namespace IncomeCalculator
{
    public class IncomeFactory
    {
        public static Income Create(TextBox tb, ComboBox cb, bool studentLoanRepayments)
        {
            var amount = double.Parse(tb.Text);

            var period = cb.SelectedIndex != -1 ? (IncomePeriod) cb.SelectedIndex : IncomePeriod.Yearly;

            return new Income(amount, period, studentLoanRepayments);
        }
    }
}

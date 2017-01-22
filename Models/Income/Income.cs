using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncomeCalculator.Enums;
using IncomeCalculator.Logic;

namespace IncomeCalculator
{
    public class Income
    {
        private readonly bool _studentLoanRepayments;
        public const double WeeksPerYear = ((365d / 7 * 3) + (366d / 7)) / 4;

        public double BaseSalary;

        public double BaseSalaryTaxed => Tax(BaseSalary);

        public double BaseSalaryTaxedMinusStudentLoanDeductions => Tax(BaseSalary - (_studentLoanRepayments ? DeductStudentLoan() : 0)) ;

        //public double BaseSalaryMinusStudentLoanDeductions => Tax(BaseSalary - (_studentLoanRepayments ? DeductStudentLoan() : 0));

        public Income(double amount, IncomePeriod period, bool studentLoanRepayments = false)
        {
            _studentLoanRepayments = studentLoanRepayments;
            switch (period)
            {
                case IncomePeriod.Weekly:
                    BaseSalary = WeeksPerYear * amount;
                    break;

                case IncomePeriod.Fortnightly:
                    BaseSalary = WeeksPerYear/2 * amount;
                    break;

                case IncomePeriod.Monthly:
                    BaseSalary = 12 * amount;
                    break;

                default:
                    BaseSalary = amount;
                    break;
            }
        }

        public Income(double amount)
        {
            BaseSalary = amount;
        }

        public double Weekly
        {
            get
            {
                return BaseSalaryTaxed / WeeksPerYear;
            }
        }

        private double DeductStudentLoan()
        {
            var thresholdPerWeek = 367;
            //var weeksThisYear = WeeksPerThisYear.GetWeeksInYear(DateTime.Now.Year); // todo delete WeeksPerThisYear
            

            var weekly = BaseSalary/ WeeksPerYear;
            if (weekly <= thresholdPerWeek)
            {
                return 0;
            }

            var remainingPerWeek = weekly - thresholdPerWeek;
            double weeklyStudentLoanRepayment = remainingPerWeek*0.12;

            return weeklyStudentLoanRepayment * WeeksPerYear; // return the amount per year, minus student loan deductions.
        }

        private static double Tax(double amount)
        {
            int[] _brackets = {  0,    14000,    48000,    70000 };
            //double[] _rates = { 10.5,     17.5,     30,       33 };    // excluding Acc Levy
            double[] _rates = { 11.95,    18.95,    31.45,    34.45 }; // including Acc Levy
            //double[] _rates = { 10.5, 17.5, 31.45,  };             // including Acc Levy after 1st bracket.

            var numberOfTaxBrackets = _brackets.Length; // 4

            var remainingIncome = (int)amount; // 56000

            double incomeTally = 0;

            // this contains a series of amounts pertaining to each bracket.
            // e.g. the 3rd item in the list will be the third tax bracket.
            List<int> IncomeByBrackets = new List<int>();

            for (int i = 0; i < numberOfTaxBrackets; i++)
            {
                if ((i + 1) >= numberOfTaxBrackets)
                {
                    // last bracket
                    IncomeByBrackets.Add(remainingIncome);
                    break;
                }
                // we know _brackets[i + 1] exists

                // grab the upper limit to this tax bracket
                // e.g. $0 - $14000 would be '14000'
                var currentBracketCeiling = _brackets[i + 1];

                var taxBracketDelta = currentBracketCeiling - _brackets[i];

                var addAmount = 0; // the amount we will add to the sum of our current bracket.

                if (taxBracketDelta > remainingIncome)
                {
                    addAmount = remainingIncome;
                }
                else
                {
                    addAmount = taxBracketDelta;
                }


                IncomeByBrackets.Add(addAmount); // add the current tax bracket delta
                remainingIncome = remainingIncome - addAmount;
                if (remainingIncome == 0)
                {
                    break;
                }
            }

            for (int i = 0; i < IncomeByBrackets.Count; i++)
            {
                incomeTally += IncomeByBrackets[i]*(1-(_rates[i])/100);
            }
            
            return incomeTally;
        }
    }
}

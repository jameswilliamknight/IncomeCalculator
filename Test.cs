using IncomeCalculator.Logic;
using NUnit.Framework;

namespace IncomeCalculator
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void WeeksThisYear()
        {
            var answer2015 = WeeksPerThisYear.GetWeeksInYear(2015);
            var answer2016 = WeeksPerThisYear.GetWeeksInYear(2016);
        }
    }
}

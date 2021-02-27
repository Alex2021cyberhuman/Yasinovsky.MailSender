using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Yasinovsky.MailSender.Services.Test
{
    public class CustomThreadMathTests
    {
        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 6)]
        [InlineData(4, 24)]
        [InlineData(5, 120)]
        [InlineData(6, 720)]
        [InlineData(7, 5040)]
        [InlineData(8, 40320)]
        [InlineData(9, 362880)]
        [InlineData(10, 3628800)]
        public void GetFactorial_Work(long n, long answer)
        {
            var result = CustomThreadMath.GetFactorialAsync(n);
            Assert.Equal(answer, result);
        }

        [Theory]
        [MemberData(nameof(GetSumOfDigits_Work_Data))]
        public void GetSumOfDigits_Work(int n, long answer)
        {     
            var result = CustomThreadMath.GetSumOfDigits(n);
            Assert.Equal(answer, result);
        }

        [Theory]
        [MemberData(nameof(GetSumOfDigits_Work_Data))]
        public void GetSumOfDigitsParallelRange_Work(int n, long answer)
        {
            var result = CustomThreadMath.GetSumOfDigitsParallelRange(n);
            Assert.Equal(answer, result);
        }

        public static IEnumerable<object[]> GetSumOfDigits_Work_Data =>
            Enumerable.Range(1000, 25).Select(x => new object[] {x, Enumerable.Range(1, x).Sum()});
    }
}
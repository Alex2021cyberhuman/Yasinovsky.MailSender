using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Yasinovsky.MailSender.Services.Test
{
    public class ParallelTest
    {
        [Fact]
        public void Test1()
        {
            var random = new Random();
            var count = 1000;
            var min = -1000;
            var max = 1000;
            var matrixA = new double[count, count];
            var matrixB = new double[count, count];

            InitMatrix(matrixA, random, min, max);
            InitMatrix(matrixB, random, min, max);
            
            //PrintMatrix(matrixA, nameof(matrixA));
            //PrintMatrix(matrixB, nameof(matrixB));

            var manager = new ParallelMatrixMultiplier(matrixA, matrixB, new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            });

            var result1 = manager.Calculate();
            //PrintMatrix(result1, nameof(result1));

            var result = manager.CalculateParallel();
            
            //PrintMatrix(result, nameof(result));
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    Assert.Equal(result[i, j], result1[i, j]);
                }
            }
        }

        private static string PrintMatrix(double[,] matrix, string name)
        {
            var sb = new StringBuilder();
            sb.AppendLine(name);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    sb.Append($"{matrix[i, j],5} ");
                }

                sb.AppendLine();
            }

            var result = sb.ToString();
            sb.Clear();
            Debug.WriteLine(result);
            return result;
        }

        private static void InitMatrix(double[,] matrixA, Random random, int min, int max)
        {
            for (int i = 0; i < matrixA.GetLength(0); i++)
            {
                for (int j = 0; j < matrixA.GetLength(1); j++)
                {
                    matrixA[i, j] = random.Next(min, max);
                }
            }
        }
    }
}
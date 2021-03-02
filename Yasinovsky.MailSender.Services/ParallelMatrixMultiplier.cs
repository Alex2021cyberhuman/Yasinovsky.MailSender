using System;
using System.Linq;
using System.Threading.Tasks;

namespace Yasinovsky.MailSender.Services
{
    public class ParallelMatrixMultiplier
    {
        private readonly double[,] _a;
        private readonly double[,] _b;

        public ParallelMatrixMultiplier(double[,] a, double[,] b)
        {
            if (a.GetLength(0) != b.GetLength(0)
                || a.GetLength(1) != b.GetLength(1))
                throw new ArgumentException("Cannot calculate matrix multiplication");
            _a = a;
            
            _b = b;
        }

        public double[,] Calculate()
        {
            var width = _b.GetLength(1);
            var result = new double[width, width];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    for (int k = 0; k < width; k++)
                    {
                        result[i, j] += _a[i, k] * _b[k, j];
                    }
                }
            }

            return result;
        }

        public double[,] CalculateParallel()
        {
            var width = _b.GetLength(1);
            var result = new double[width, width];
            Parallel.For(0, width, i => _ =
                Parallel.For(0, width, j => 
                    result[i, j]
                        = ParallelEnumerable.Range(0, width)
                            .Select(k => _a[i, k] * _b[k, j])
                            .Sum()));

            return result;
        }
    }
}
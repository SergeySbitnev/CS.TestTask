using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeprofitTechnology.TestTask
{
    class MedianaCalc
    {
        public static double Mediana(double[] arr)
        {
            Array.Sort(arr);
            int size = arr.Length;
            if (arr.Length % 2 == 0)
            {
                return (arr[size / 2] + arr[size / 2 + 1]) * 0.5;
            }
            else
            {
                return arr[size / 2];
            }
        }
    }
}

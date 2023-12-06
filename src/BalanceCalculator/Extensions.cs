using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetirementPlanning.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToUSDollar(this decimal dec)
        {
            return dec.ToString("C2");
        }

        public static string ToPercent(this decimal dec)
        {
            return dec.ToString("P2");
        }
    }

    public static class RandomExtensions
    {
        public static decimal NextInRange(this Random random, Tuple<decimal, decimal> range)
        {
            int scaler = 1000;

            int lowerLimit = (int)(range.Item1 * scaler);
            int upperLimit = (int)(range.Item2 * scaler);

            int randomScaled = random.Next(lowerLimit, upperLimit);
            return (decimal)randomScaled / scaler;
        }
    }
}

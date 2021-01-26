using System;
using System.Collections.Generic;

namespace JimmysUnityUtilities
{
    public static class NumberCollectionExtensions
    {
        public static float GetMean(this IEnumerable<float> numbers)
        {
            float total = 0;
            int count = 0;

            foreach (var number in numbers)
            {
                total += number;
                count++;
            }

            return total / count;
        }
        public static float GetSum(this IEnumerable<float> numbers)
        {
            float total = 0;

            foreach (var number in numbers)
                total += number;

            return total;
        }

        public static double GetMean(this IEnumerable<double> numbers)
        {
            double total = 0;
            int count = 0;

            foreach (var number in numbers)
            {
                total += number;
                count++;
            }

            return total / count;
        }
        public static double GetSum(this IEnumerable<double> numbers)
        {
            double total = 0;

            foreach (var number in numbers)
                total += number;

            return total;
        }

        public static float GetMean(this IEnumerable<int> numbers)
        {
            int total = 0;
            int count = 0;

            foreach (var number in numbers)
            {
                total += number;
                count++;
            }

            return total / (float)count;
        }
        public static double GetMeanPrecise(this IEnumerable<int> numbers)
        {
            int total = 0;
            int count = 0;

            foreach (var number in numbers)
            {
                total += number;
                count++;
            }

            return total / (double)count;
        }
        public static int GetMeanRoundedToNearestWhole(this IEnumerable<int> numbers)
        {
            int total = 0;
            int count = 0;

            foreach (var number in numbers)
            {
                total += number;
                count++;
            }

            return total / count;
        }
        public static int GetSum(this IEnumerable<int> numbers)
        {
            int total = 0;

            foreach (var number in numbers)
                total += number;

            return total;
        }


        public static double GetMean(this IEnumerable<long> numbers)
        {
            long total = 0;
            long count = 0;

            foreach (var number in numbers)
            {
                total += number;
                count++;
            }

            return total / (double)count;
        }
        public static long GetMeanRoundedToNearestWhole(this IEnumerable<long> numbers)
        {
            long total = 0;
            long count = 0;

            foreach (var number in numbers)
            {
                total += number;
                count++;
            }

            return total / count;
        }
        public static long GetSum(this IEnumerable<long> numbers)
        {
            long total = 0;

            foreach (var number in numbers)
                total += number;

            return total;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace HoursRegisterGenerator
{
    public static class Utils
    {
        private static readonly Random random = new Random();

        public static int[] GenerateIndicatedAmountOfRandomNumbersWithSpecifiedSum(int amountOfNumbers, int sum)
        {
            int[] numbers = new int[amountOfNumbers];

            for (int i = 0; i < sum; i++)
            {
                lock (random)
                {
                    numbers[random.Next(0, amountOfNumbers)]++;
                }
            }
            return numbers;
        }

        public static int GetRandomNumber(int from, int to)
        {
            lock (random)
            {
                return random.Next(from, to);
            }
        }

        public static List<int> NormalizeRandomWorkHours(List<int> numbers, int max, int min)
        {
            int controlSum = numbers.Sum();
            for (int iter = 0; iter < numbers.Count; iter++)
            {
                if (numbers[iter] > max)
                {
                    int rnd = GetRandomNumber(1, 5);
                    int indexOfMinValue = numbers.IndexOf(numbers.Min());
                    numbers[iter] -= rnd;
                    numbers[indexOfMinValue] += rnd;
                }
                if (numbers[iter] < min)
                {
                    int rnd = GetRandomNumber(1, 5);
                    int indexOfMaxValue = numbers.IndexOf(numbers.Max());
                    numbers[iter] += rnd;
                    numbers[indexOfMaxValue] -= rnd;
                }
            }

            if (numbers.Sum() != controlSum)
                throw new Exception("numbers.Sum() != controlSum");
            return numbers;
        }
    }
}

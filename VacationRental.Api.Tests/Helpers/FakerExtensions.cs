using System;

namespace VacationRental.Api.Tests.Helpers
{
    public static class FakerExtensions
    {
        private static readonly Random random = new Random();

        public static int GenerateRandomNumber(short digits)
        {
            int minValue = Convert.ToInt32(Math.Pow(10.0, digits - 1));
            int maxValue = Convert.ToInt32(Math.Pow(10.0, digits) - 1.0);
            return random.Next(minValue, maxValue);
        }

    }
}

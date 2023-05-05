using System;

namespace CMS_Common
{
    public class RandomID
    {
        private static readonly Random random = new Random();

        public static int GenerateRandomNumber()
        {
            return random.Next(10000000, 99999999);
        }
    }
}
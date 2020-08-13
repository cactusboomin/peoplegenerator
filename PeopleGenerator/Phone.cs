using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleGenerator
{
    public class Phone
    {
        private static string[] phoneCodes = new string[]
        {
            "29",
            "33",
            "44"
        };
        private Random rnd = new Random();

        public string PhoneNumber()
        {
            return $"+375 ({phoneCodes[rnd.Next(0, phoneCodes.Length)]}) {rnd.Next(10, 99)}-{rnd.Next(10, 99)}-{rnd.Next(100, 999)}";
        }
    }
}

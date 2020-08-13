using Bogus;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleGenerator
{
    public static class Program
    {
        private static string region;
        private static int records;
        private static double errors;
        private static StringBuilder symbolsUS = new StringBuilder("qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890");
        private static StringBuilder symbolsRU = new StringBuilder("йцукенгшщзхъфывапролджэячсмитьбюЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ1234567890");

        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            try
            {
                TryParseArgs(args);

                ParseArgs(args, out region, out records, out errors);

                if (region.Equals("be_by", StringComparison.InvariantCultureIgnoreCase))
                {
                    GenerateBelarus(records, errors);
                }
                else
                {
                    Generate(region, records, errors);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void TryParseArgs(string[] args)
        {
            if (args.Length < 2)
            {
                throw new ArgumentOutOfRangeException($"{nameof(args)} must have at least 2 parameters. ");
            }

            var languages = new List<string>() { "en_us", "ru_ru", "be_by" };

            if (!languages.Contains(args[0].Trim().ToLower()))
            {
                throw new ArgumentOutOfRangeException($"Unknown region and language. ");
            }

            int recordsCount;

            if (!int.TryParse(args[1], out recordsCount))
            {
                throw new InvalidCastException("Invalid number of records. ");
            }

            if (recordsCount < 1)
            {
                throw new ArgumentOutOfRangeException("The count of records must be more than zero. ");
            }

            if (args.Length > 2)
            {
                double errorsCount;

                if (!double.TryParse(args[2], out errorsCount))
                {
                    throw new InvalidCastException("Invalid number of errors. ");
                }

                if (errorsCount < 0)
                {
                    throw new ArgumentOutOfRangeException("The count of errors must be more or equals zero. ");
                }
            }
        }

        private static void ParseArgs(string[] args, out string region, out int recordsCount, out double errorsCount)
        {
            region = args[0];
            recordsCount = int.Parse(args[1]);
            
            if (args.Length < 3)
            {
                errorsCount = 0;
            }
            else
            {
                errorsCount = double.Parse(args[2]);
            }
        }

        private static void Generate(string region, int records, double errors)
        {
            Random rnd = new Random();
            int errorCounter = 1;
            string country;
            NewFaker faker = new NewFaker();

            if (region.Equals("en_US", StringComparison.InvariantCultureIgnoreCase))
            {
                faker = new NewFaker("en");
                country = "USA";
            }
            else
            { 
                faker = new NewFaker("ru");
                country = "Россия";
            }

            for (int i = 0; i < records; i++)
            {
                var name = new StringBuilder(faker.NewName.FullName());

                var address = new StringBuilder(country + ", "
                            + faker.Address.City() + ", "
                            + faker.Address.ZipCode() + ", "
                            + faker.Address.StreetName() + ", "
                            + faker.Address.BuildingNumber());

                var number = new StringBuilder(faker.Phone.PhoneNumber());

                if (errors != 0)
                {
                    int percent = 1;
                    StringBuilder info = GetInformation(name, address, number, ref percent);

                    if (errors >= 1)
                    {
                        while (errorCounter <= errors)
                        {
                            info = MakeError(info);
                            GiveInformation(name, address, number, percent, info);
                            info = GetInformation(name, address, number, ref percent);
                            errorCounter++;
                        }

                        errorCounter = 1;
                    }
                    else
                    {
                        if (errorCounter < (int)(1 / errors))
                        {
                            errorCounter++;
                        }
                        else
                        {
                            errorCounter = 1;
                            info = MakeError(info);
                            GiveInformation(name, address, number, percent, info);
                        }
                    }
                }
                
                Console.WriteLine($"{i + 1}) {name}; {address}; {number}");
            }
        }

        private static void GenerateBelarus(int records, double errors)
        {
            Random rnd = new Random();
            int errorCounter = 1;
            string country = "Беларусь";
            BogusBelarus faker = new BogusBelarus();

            for (int i = 0; i < records; i++)
            {
                var name = new StringBuilder(faker.NewName.FullName());

                var address = new StringBuilder(country + ", "
                            + faker.Address.City() + ", "
                            + faker.Address.ZipCode() + ", "
                            + faker.Address.StreetName() + ", "
                            + faker.Address.BuildingNumber());

                var number = new StringBuilder(faker.Phone.PhoneNumber());

                if (errors != 0)
                {
                    int percent = 1;
                    StringBuilder info = GetInformation(name, address, number, ref percent);

                    if (errors >= 1)
                    {
                        while (errorCounter <= errors)
                        {
                            info = MakeError(info);
                            GiveInformation(name, address, number, percent, info);
                            info = GetInformation(name, address, number, ref percent);
                            errorCounter++;
                        }

                        errorCounter = 1;
                    }
                    else
                    {
                        if (errorCounter < (int)(1 / errors))
                        {
                            errorCounter++;
                        }
                        else
                        {
                            errorCounter = 1;
                            info = MakeError(info);
                            GiveInformation(name, address, number, percent, info);
                        }
                    }
                }

                Console.WriteLine($"{i + 1}) {name}; {address}; {number}");
            }
        }

        private static StringBuilder GetInformation(StringBuilder name, StringBuilder address, StringBuilder number, ref int percent)
        {
            Random rnd = new Random();
            percent = rnd.Next(1, 4);

            if (percent == 1)
            {
                return name;
            }
            else if (percent == 2)
            {
                return address;
            }
            else
            {
                return number;
            }
        }

        private static void GiveInformation(StringBuilder name, StringBuilder address, StringBuilder number, int percent, StringBuilder info)
        {
            if (percent == 1)
            {
                name = info;
            }
            else if (percent == 2)
            {
                address = info;
            }
            else
            {
                number = info;
            }
        }

        private static StringBuilder MakeError(StringBuilder s)
        {
            Random rnd = new Random();
            int percent = rnd.Next(1, 4);
            int position = rnd.Next(0, s.Length - 1);

            if (percent == 1 && s.Length > 1)
            {
                s.Remove(position, 1);
            }
            else if (percent == 2 && s.Length > 1)
            {
                if (position == 0)
                {
                    s.Insert(position, s[position + 1]);
                    s.Remove(position + 2, 1);
                }
                else
                {
                    s.Insert(position - 1, s[position]);
                    s.Remove(position + 1, 1);
                }
            }
            else
            {
                if (region.Equals("ru_ru", StringComparison.InvariantCultureIgnoreCase))
                {
                    s.Insert(position, symbolsRU[rnd.Next(0, symbolsRU.Length)]);
                }
                else
                {
                    s.Insert(position, symbolsUS[rnd.Next(0, symbolsUS.Length)]);
                }
            }


            return s;
        }
    }
}

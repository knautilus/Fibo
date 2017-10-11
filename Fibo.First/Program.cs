using System;
using System.Collections.Generic;
using Fibo.Utils;
using Newtonsoft.Json;
using StructureMap;

namespace Fibo.First
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = Container.For<DependencyConfig>();

            var app = container.GetInstance<Application>();
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new BigIntegerConverter() }
            };

            Console.WriteLine("Fibonacci Numbers Generator");
            Console.WriteLine("Enter number of calculations (1 to 1000):");

            int number;

            do
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out number))
                {
                    if (number >= 1 && number <= 1000)
                    {
                        break;
                    }
                }
            }
            while (true);

            app.RunAsync(number);

            Console.WriteLine("Please find results in logs");
            Console.WriteLine("Processing...");
            Console.WriteLine("Press Enter to stop");
            Console.ReadLine();

            container.Dispose();
        }
    }
}

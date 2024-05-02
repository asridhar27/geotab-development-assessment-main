using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {
        static string[] results = new string[50];
        static char key;
        static ConsolePrinter printer = new ConsolePrinter();

        static void Main(string[] args)
        {
            printer.Value("Press ? to get instructions.").ToString();
            key = Console.ReadKey().KeyChar;
            while (true)
            {
                printer.Value("Press c to get categories").ToString();
                printer.Value("Press r to get random jokes").ToString();
                key = Console.ReadKey().KeyChar;
                if (key == 'c')
                {
                    getCategories();
                    PrintResults();
                }
                if (key == 'r')
                {
                    printer.Value("Want to specify a category? y/n").ToString();
                    key = Console.ReadKey().KeyChar;
                    if (key == 'y')
                    {
                        printer.Value("How many jokes do you want? (1-9)").ToString();
                        int n = Int32.Parse(Console.ReadLine());
                        printer.Value("Enter a category;").ToString();
                        GetRandomJokes(Console.ReadLine(), n);
                        PrintResults();
                    }
                    else
                    {
                        printer.Value("How many jokes do you want? (1-9)").ToString();
                        int n = Int32.Parse(Console.ReadLine());
                        GetRandomJokes(null, n);
                        PrintResults();
                    }
                }
            }
        }

        private static void PrintResults()
        {
            printer.Value("[" + string.Join(",", results) + "]").ToString();
        }

        private static void GetRandomJokes(string category, int number)
        {
            new JsonFeed("https://us-central1-geotab-interviews.cloudfunctions.net/joke", number);
            results = JsonFeed.GetRandomJokes(category);
        }

        private static void getCategories()
        {
            new JsonFeed("https://us-central1-geotab-interviews.cloudfunctions.net/joke_category", 0);
            results = JsonFeed.GetCategories();
        }
    }
}

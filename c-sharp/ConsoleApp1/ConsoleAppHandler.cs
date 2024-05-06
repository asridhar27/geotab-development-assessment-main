using System;
using System.Linq;
using System.Threading.Tasks;

public class ConsoleAppHandler
{
    private readonly IJokeService _jokeService;

    public ConsoleAppHandler(IJokeService jokeService)
    {
        _jokeService = jokeService;
    }

    public async Task RunAsync()
    {
        Console.WriteLine("Press ? to get instructions.");
        while (true)
        {
            string key = Console.ReadLine();
            if (key == "?")
            {
                Console.WriteLine("Press c to get categories");
                Console.WriteLine("Press r to get random jokes");
            }
            else if (key == "c")
            {
                await GetCategoriesAsync();
            }
            else if (key == "r")
            {
                await GetRandomJokesAsync();
            }
            else if (key == "e" || key == "exit")
            {
                Console.WriteLine("EXITING THE APP....");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Invalid input. Press ? to get instructions.");
            }
        }
    }

    public async Task GetCategoriesAsync()
    {
        string[] categories = await _jokeService.GetCategories();
        if(!categories.Any())
        {
            Console.WriteLine("No categories available");
        }
        else 
        {
            Console.WriteLine($"{Environment.NewLine}Categories: {string.Join(", ", categories)}");
        }
        Console.WriteLine("Press c or r to continue or Press e to terminate the app ");
    }

    public async Task GetRandomJokesAsync()
    {
        Console.WriteLine("Want to specify a category? y/n");
        char key = Console.ReadKey().KeyChar;
        string category = null;
        if (key == 'y')
        {
            string[] validCategories = await _jokeService.GetCategories();
            Console.WriteLine($"{Environment.NewLine}Enter a category:");
            category = Console.ReadLine();
            while(!validCategories.Contains(category))
            {
                Console.WriteLine("Invalid Category. Choose from the following list of valid categories:");
                Console.WriteLine($"Categories: {string.Join(", ", validCategories)}");
                category = Console.ReadLine();
            }
        }
        int numberOfJokes = ValidateNumberOfJokes();
        if (numberOfJokes == 0)
        {
            Console.WriteLine("Exiting the App....");
            Environment.Exit(0);
        }
        else
        {
            string[] jokes = await _jokeService.GetRandomJokes(category, numberOfJokes);
            if(!jokes.Any())
            {
                Console.WriteLine("No categories available");
            }
            else 
            {
                for (int i = 0; i < jokes.Length; i++)
                {
                    Console.WriteLine($"- {jokes[i]}");
                }
            }
            Console.WriteLine("Press c or r to continue or Press e or exit to terminate the app");
        }
    }


    private int ValidateNumberOfJokes()
    {
        while (true)
        {
            Console.WriteLine($"{Environment.NewLine}How many jokes do you want? (1-9)");
            string input = Console.ReadLine();
    
            if (input.Equals("e", StringComparison.OrdinalIgnoreCase)
                || input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                return 0;
            }
            if (int.TryParse(input, out int result) && result >= 1 && result <= 9)
            {
                return result;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 9.");
            }
        }
    }
}

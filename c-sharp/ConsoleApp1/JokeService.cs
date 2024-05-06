using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    public class JokeService : IJokeService
    {
		private readonly HttpClient _httpClient;
		private static readonly string categoryAPI = "https://us-central1-geotab-interviews.cloudfunctions.net/joke_category";
		private static readonly string JOKES_API = "https://us-central1-geotab-interviews.cloudfunctions.net/joke";
        
		public JokeService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task<string[]> GetRandomJokes(string category, int numberOfJokes)
		{
			HashSet<string> uniqueJokes = new HashSet<string>();
			try
			{
				if (!string.IsNullOrEmpty(category) && !Regex.IsMatch(category, @"^[a-zA-Z]+$"))
				{
					Console.WriteLine("Category must contain only letters.");
				}
				else
				{
					UriBuilder _uriBuilder = new UriBuilder(JOKES_API)
					{
						Query = category != null ? $"category={category}" : null
					};

					string url = _uriBuilder.Uri.ToString();
					int maxRetries = 20;
					int retries = 0;
					while (uniqueJokes.Count < numberOfJokes && retries < maxRetries)
					{
						string uniqueJoke = await _httpClient.GetStringAsync(url);
						string jokeValue = JsonConvert.DeserializeObject<JokeResponse>(uniqueJoke).Value;

						if (!uniqueJokes.Contains(jokeValue))
						{
							uniqueJokes.Add(jokeValue);
						}
						retries++;
					}
					if (retries >= maxRetries)
					{
						Console.WriteLine("Could not find specified number of unique jokes in this category. Fetching the maximum unique jokes available in this category.");
					}
				}
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"An error occurred while fetching jokes: {ex.Message}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An unexpected error occurred: {ex.Message}");
			}
			return uniqueJokes.ToArray();
		}

		public async Task<string[]> GetCategories()
		{
			try
			{
				var response = await _httpClient.GetAsync(new UriBuilder(categoryAPI).ToString());
				if (response.IsSuccessStatusCode)
				{
					string jokeCategories = await response.Content.ReadAsStringAsync();
					return JsonConvert.DeserializeObject<string[]>(jokeCategories);
				}
				else
				{
					Console.WriteLine($"Failed to fetch categories. Status code: {response.StatusCode}");
				}
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"An error occurred while fetching categories: {ex.Message}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An unexpected error occurred: {ex.Message}");
			}

			return Array.Empty<string>();
		}
	}
}

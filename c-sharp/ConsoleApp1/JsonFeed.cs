using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class JsonFeed
    {
        static string _url = "";

        public JsonFeed() { }
        public JsonFeed(string endpoint, int results)
        {
            _url = endpoint;
        }
        
		public static string[] GetRandomJokes(string category)
		{
			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri(_url);
			string url = string.Empty;
			if (category != null)
			{
				url += "?";
				url += "category=";
				url += category;
			}

            string joke = Task.FromResult(client.GetStringAsync(url).Result).Result;

            return new string[] { JsonConvert.DeserializeObject<dynamic>(joke).value };
        }

		public static string[] GetCategories()
		{
			HttpClient client = new HttpClient();

			return new string[] { Task.FromResult(client.GetStringAsync(new Uri(_url)).Result).Result };
		}
    }
}

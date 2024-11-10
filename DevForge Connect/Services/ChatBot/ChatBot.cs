using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace DevForge_Connect.Services.ChatBot
{
    public class ChatBot
    {
		public async Task<string> GetGeminiResponse(string description)
		{
			using (HttpClient client = new HttpClient())
			{
				var uri = new Uri($"http://127.0.0.1:8000/genResponse?text={description}");

				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage response = await client.GetAsync(uri);
				response.EnsureSuccessStatusCode();

				var responseString = await response.Content.ReadAsStringAsync();

				return responseString;
			}
		}


	}
}

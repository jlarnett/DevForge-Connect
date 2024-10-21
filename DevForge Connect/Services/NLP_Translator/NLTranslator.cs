using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DevForge_Connect.Services.NLP_Translator
{
    public class NLTranslator : ITranslator
    {
        /// <summary>
        /// Calls the NLP Endpoint with the passed in description
        /// Concatenates the tag list into a singular string that can be later split 
        /// </summary>
        /// <param name="description">project submission description</param>
        /// <returns></returns>
        public async Task<string> GetNlpTags(string[] description) 
        {
			using (HttpClient client = new HttpClient())
			{
				var uri = new Uri("http://127.0.0.1:8000/textPrediction");

				// Convert the array to a JSON string
				var jsonContent = JsonConvert.SerializeObject(new { text = description });

				// Create the HTTP content with the JSON payload
				var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

				// Send the POST request
				HttpResponseMessage response = await client.PostAsync(uri, content);
				response.EnsureSuccessStatusCode();

				// Read and return the response content as a string
				var responseString = await response.Content.ReadAsStringAsync();
				return responseString;
			}
		}

        /// <summary>
        /// Parses JArray into 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string ConcatNlpTags(JArray obj)
        {
            var values = obj.Values().ToList();
            StringBuilder builder = new StringBuilder();

            int counter = 0;

            foreach(var value in values)
            {
                if(!counter.Equals(0)) builder.Append("|");
                builder.Append(value.Value<string>());
                counter++;
            }

            return builder.ToString();
        }

        public List<string> DecatNlpTags(string tagString)
        {
            var tags = tagString.Split('|');
            return tags.ToList();
        }
    }
}

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

        public string GrabTop3Tags(string nlpClasses)
        {
			

			// Deserialize the JSON string into a List of Lists
			var technologyLists = JsonConvert.DeserializeObject<List<List<string>>>(nlpClasses);

			// Flatten the list and count occurrences of each technology
			var technologyCounts = technologyLists
				.SelectMany(x => x) // Flatten into a single list of technologies
				.GroupBy(x => x)    // Group by each technology name
				.ToDictionary(g => g.Key, g => g.Count()); // Create a dictionary with counts

			// Check if there are any technologies with more than one occurrence
			var hasRepeats = technologyCounts.Values.Any(count => count > 1);

			// If there are repeats, get the top 3 based on frequency, otherwise get unique technologies
			string result;
			if (hasRepeats)
			{
				result = technologyCounts
					.OrderByDescending(kvp => kvp.Value) // Order by frequency
					.Take(3)                             // Take the top 3
					.Select(kvp => kvp.Key)              // Select only the technology name
					.Aggregate((current, next) => $"{current}|{next}"); // Join with "|"
			}
			else
			{
				result = string.Join("|", technologyCounts.Keys); // Join all unique technologies with "|"
			}
			return result;
		}
    }
}

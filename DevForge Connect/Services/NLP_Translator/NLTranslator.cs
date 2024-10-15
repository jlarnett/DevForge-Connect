using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
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
        public async Task<string> GetNlpTags(string description) 
        {
            HttpClient client = new HttpClient();

            var uri = new Uri($"http://localhost:8000/textPrediction?text='{description}'");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            client.Dispose();
            return ConcatNlpTags(JArray.Parse(responseString));
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

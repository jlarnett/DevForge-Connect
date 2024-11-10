using DevForge_Connect.Services.NLP_Translator;

using DevForge_Connect.Services.ChatBot;
using DevForge_Connect.Entities;
using System.Text.RegularExpressions;

namespace DevForge.Tests
{
    [TestClass]
    public class NLPTests
    {
        [TestMethod]
        public void VerifyNlpDeconcatenation()
        {
            ITranslator translator = new NLTranslator();
            string tagString = "STORAGE|AI|WEB_DEVELOPMENT";

            var tags = translator.DecatNlpTags(tagString);

            Assert.AreEqual(tags.Count, 3);
            Assert.AreEqual(tags[0], "STORAGE");
            Assert.AreEqual(tags[1], "AI");
            Assert.AreEqual(tags[2], "WEB_DEVELOPMENT");
        }

		[TestMethod]
		public async Task VerifyLLMOuputAsync()
		{
			ITranslator translator = new NLTranslator();
			
			string nonFormatedString = "I want to create a storage app on mobile devices. I want to build an ai gaming engine.  I want to communicate with customers";
			string[] input = Regex.Split(nonFormatedString, @"(?<=[.!?])\s+");



			var response = await translator.GetNlpTags(input);

			Console.WriteLine(response);

			Assert.IsNotNull(response, "Response from LLM is not Null");
			
		}

		[TestMethod]
		public async Task GetGeminiResponse_ReturnsResponseFromApi()
		{
			// Arrange
			var chatBot = new ChatBot();
			var description = "";

			// Act
			var actualResponse = await chatBot.GetGeminiResponse(description);

			Console.WriteLine($"API Response: {actualResponse}");

			// Assert
			Assert.IsNotNull(actualResponse, "The response from the API should not be null.");
			Assert.IsTrue(actualResponse.Length > 0, "The response should not be empty.");
		}
	}
}

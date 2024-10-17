using DevForge_Connect.Services.NLP_Translator;

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
    }
}
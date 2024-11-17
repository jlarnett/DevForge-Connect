using Newtonsoft.Json.Linq;

namespace DevForge_Connect.Services.NLP_Translator
{
    public interface ITranslator
    {
        Task<string> GetNlpTags(string[] description);
        string ConcatNlpTags(JArray obj);
        List<string> DecatNlpTags(string tagString);
        string GrabTop3Tags(string nlpClasses);

	}
}

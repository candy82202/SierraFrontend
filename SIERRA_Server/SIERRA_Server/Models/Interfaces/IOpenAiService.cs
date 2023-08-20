namespace SIERRA_Server.Models.Interfaces
{
    public interface IOpenAiService
    {
        Task<string> CompleteSentence(string text);
        Task<string> CompleteSentenceAdvance(string text);
        Task<string> CheckProgramingLanguage(string language);
        Task<string> AskDessertQuestion(string text);
    }
}

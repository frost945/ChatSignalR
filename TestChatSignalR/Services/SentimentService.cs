using Azure;
using Azure.AI.TextAnalytics;

namespace TestChatSignalR.Services
{
    public interface ISentimentService
    {
        Task<string> AnalyzeSentimentAsync(string message);
    }

    public class SentimentService : ISentimentService
    {

        private readonly TextAnalyticsClient client;

        public SentimentService(string endpoint, string key)
        {
            var credentials = new AzureKeyCredential(key);
            client = new TextAnalyticsClient(new Uri(endpoint), credentials);
        }

        public async Task<string> AnalyzeSentimentAsync(string message)
        {
            DocumentSentiment result = await client.AnalyzeSentimentAsync(message);
            return result.Sentiment.ToString(); // Возвращает: Positive, Neutral, или Negative
        }
    }

    //класс заглушка на случай если SentimentService не будет работать
    public class SentimentServiceStub : ISentimentService
    {
        public Task<string> AnalyzeSentimentAsync(string message)
        {
            return Task.FromResult("Neutral"); // по умолчанию
        }
    }
}

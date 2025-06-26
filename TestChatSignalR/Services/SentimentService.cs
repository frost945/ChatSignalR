using Azure;
using Azure.AI.TextAnalytics;

namespace TestChatSignalR.Services
{
    public class SentimentService
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
}

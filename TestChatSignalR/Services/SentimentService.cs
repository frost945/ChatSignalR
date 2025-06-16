using Azure;
using Azure.AI.TextAnalytics;

namespace TestChatSignalR.Services
{
    public class SentimentService
    {

        private readonly TextAnalyticsClient _client;

        public SentimentService(string endpoint, string key)
        {
            var credentials = new AzureKeyCredential(key);
            _client = new TextAnalyticsClient(new Uri(endpoint), credentials);
        }

        public async Task<string> AnalyzeSentimentAsync(string message)
        {
            DocumentSentiment result = await _client.AnalyzeSentimentAsync(message);
            return result.Sentiment.ToString(); // Возвращает: Positive, Neutral, или Negative
        }
    }
}

using Microsoft.AspNetCore.SignalR;
using TestChatSignalR.Domain;
using TestChatSignalR.Models;
using TestChatSignalR.Services;

namespace TestChatSignalR.Hubs
{
   
    public class ChatHub : Hub
    {
        private readonly ChatDbContext _db;
        private readonly ChatRepository _chatRepository;
        private readonly SentimentService _sentimentService;

         public ChatHub(ChatDbContext db, ChatRepository chatRepository, SentimentService sentimentService)
         {
             _db = db;
             _chatRepository = chatRepository;
             _sentimentService = sentimentService;
         }

        
        //функция в процессе доработки
        /*public async Task LoadChatHistory(string chat, int skip = 0)
        {
            Console.WriteLine("LoadChatHistory on");
            List<ChatMessage> messages = await _chatRepository.GetMessagesAsync(chat, 50, skip);

            // Отправляем клиенту список сообщений
            Console.WriteLine($"Sending {messages.Count} messages to client...");

            await Clients.Caller.SendAsync("ReceiveHistory", chat, messages);
        }*/





    }
}

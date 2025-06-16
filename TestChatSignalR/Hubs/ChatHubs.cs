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

        public ChatHub(SentimentService sentimentService, ChatDbContext db, ChatRepository chatRepository)
        {
            _sentimentService = sentimentService;
            _db = db;
            _chatRepository = chatRepository;
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

        public async Task SendMessage(string user, string chat, string message)
        {
             ChatMessage chatMessage = new ChatMessage
             {
                 UserName = user,
                 ChatName = chat,
                 Message = message
             };

             _db.ChatMessages.Add(chatMessage);
            await _db.SaveChangesAsync();

            string sentiment = await _sentimentService.AnalyzeSentimentAsync(message);

            await Clients.Group(chat).SendAsync("ReceiveMessage", user, chat, message, sentiment);
        }


        public async Task joinChat(UserConnection userConnection)
        {
            Console.WriteLine("joinChat on");
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.ChatName);

            await Clients.Group(userConnection.ChatName).SendAsync("ReceiveMessage", "admin", userConnection.ChatName, $"{userConnection.UserName} присоединился к чату");    
        }
    }
}

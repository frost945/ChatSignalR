using Microsoft.AspNetCore.SignalR;
using TestChatSignalR.Domain;
using TestChatSignalR.Models;
using TestChatSignalR.Services;

namespace TestChatSignalR.Hubs
{
    public class ChatHub : Hub
    {
        private readonly SentimentService _sentimentService;
        private readonly ChatDbContext _db;
        private readonly ChatRepository _chatRepository;

        public ChatHub(ChatDbContext db, ChatRepository chatRepository, SentimentService sentimentService)
        {
            _db = db;
            _chatRepository = chatRepository;
            _sentimentService = sentimentService;
        }

        public async Task joinChat(UserConnection userConnection)
        {
            Console.WriteLine("joinChat on");
            Console.WriteLine($"{userConnection.UserName} {userConnection.ChatName}");
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.ChatName);

            await Clients.Group(userConnection.ChatName).SendAsync("ReceiveMessage", "admin", userConnection.ChatName, $"{userConnection.UserName} присоединился к чату");
        }

        public async Task SendMessage(UserConnection userConnection, string message)
        {
            Console.WriteLine("sendMessage on");
            Console.WriteLine($"{userConnection.UserName} {userConnection.ChatName} {message}");

            ChatMessage chatMessage = new ChatMessage
            {
                UserName = userConnection.UserName,
                ChatName = userConnection.ChatName,
                Message = message
            };

            _db.ChatMessages.Add(chatMessage);
            await _db.SaveChangesAsync();

            string sentiment = await _sentimentService.AnalyzeSentimentAsync(message);

            await Clients.Group(userConnection.ChatName).SendAsync("ReceiveMessage", userConnection.UserName, userConnection.ChatName, message, sentiment);
        }

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

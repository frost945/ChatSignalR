using Microsoft.AspNetCore.SignalR;
using TestChatSignalR.Domain;
using TestChatSignalR.Models;
using TestChatSignalR.Services;

namespace TestChatSignalR.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ISentimentService iSentimentService;
        private readonly ChatDbContext db;
        private readonly ChatRepository chatRepository;

        public ChatHub(ChatDbContext Db, ChatRepository ChatRepository, ISentimentService ISentimentService)
        {
            db = Db;
            chatRepository = ChatRepository;
            iSentimentService = ISentimentService;
        }

        public async Task joinChat(UserConnection userConnection)
        {
            Console.WriteLine("joinChat on");
            Console.WriteLine($"{userConnection.userName} {userConnection.chatName}");
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.chatName);

            await Clients.Group(userConnection.chatName).SendAsync("ReceiveMessage", "admin", userConnection.chatName, $"{userConnection.userName} присоединился к чату");
        }

        public async Task SendMessage(UserConnection userConnection, string Message)
        {
            Console.WriteLine("sendMessage on");
            Console.WriteLine($"{userConnection.userName} {userConnection.chatName} {Message}");

            ChatMessage chatMessage = new ChatMessage
            {
                userName = userConnection.userName,
                chatName = userConnection.chatName,
                message = Message
            };

            db.chatMessages.Add(chatMessage);// добавляем в БД сообщение
            await db.SaveChangesAsync();

            string sentiment = await iSentimentService.AnalyzeSentimentAsync(Message);

            await Clients.Group(userConnection.chatName).SendAsync("ReceiveMessage", userConnection.userName, userConnection.chatName, Message, sentiment);
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

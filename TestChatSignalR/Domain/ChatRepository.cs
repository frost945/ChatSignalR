using TestChatSignalR.Models;
using Microsoft.EntityFrameworkCore;


namespace TestChatSignalR.Domain
{
    public class ChatRepository
    {
        private readonly ChatDbContext context;

        public ChatRepository(ChatDbContext Context)
        {
            context = Context;
        }

        //в процессе доработки
        /*public async Task<List<ChatMessage>> GetMessagesAsync(string chatName, int count = 50, int skip = 0)
        {
            return await _context.ChatMessages
                .Where(m => m.ChatName == chatName)
                .OrderByDescending(m => m.SentAt)
                .Skip(skip)
                .Take(count)
                .ToListAsync();
        }*/
    }
}

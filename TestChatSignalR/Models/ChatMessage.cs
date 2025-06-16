﻿namespace TestChatSignalR.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ChatName { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now;
    }
}

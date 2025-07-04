﻿using Microsoft.EntityFrameworkCore;
using TestChatSignalR.Domain;
using TestChatSignalR.Hubs;
using TestChatSignalR.Services;

namespace TestChatSignalR
{
    public class Program
    {
        public static async Task Main()
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder();

            builder.Services.AddDbContext<ChatDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //хранилище сообщений
            builder.Services.AddScoped<ChatRepository>();

            builder.Services.AddSignalR();

            //подключаю анализатор настроений
            builder.Services.AddSingleton<ISentimentService>(provider =>
            {
                IConfiguration config = provider.GetRequiredService<IConfiguration>();
                string? endpoint = config["AzureCognitiveServices:Endpoint"];
                string? key = config["AzureCognitiveServices:Key"];

                if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(key))
                {
                    Console.WriteLine("SentimentService отключён: отсутствует конфигурация");
                    return new SentimentServiceStub(); // заглушка
                }
                return new SentimentService(endpoint, key);
            });


            WebApplication app = builder.Build();           

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapHub<ChatHub>("/chat");

            await app.RunAsync();
        }
    }
}

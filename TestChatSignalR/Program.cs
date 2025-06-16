using Microsoft.EntityFrameworkCore;
using TestChatSignalR.Domain;
using TestChatSignalR.Hubs;
using TestChatSignalR.Services;


namespace TestChatSignalR
{
    public class Program
    {
        public static async Task Main()
        {
            //"DefaultConnection": "Server=localhost;Database=DBChatSignalR;User Id=sa;Password=StrongPass123!;TrustServerCertificate=True;"
            //Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");


            var builder = WebApplication.CreateBuilder();

           // builder.Services.AddDbContext<ChatDbContext>(options =>
            //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //builder.Services.AddScoped<ChatRepository>();


            builder.Services.AddSignalR();

            builder.Services.AddSingleton(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                var endpoint = config["AzureCognitiveServices:Endpoint"];
                var key = config["AzureCognitiveServices:Key"];
                return new SentimentService(endpoint, key);
            });


            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();
           

            app.UseRouting();

            app.MapHub<ChatHub>("/chat");

            await app.RunAsync();
        }
    }
}

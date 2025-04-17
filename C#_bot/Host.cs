using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace C__bot
{
    internal class Host
    {
        public Action<ITelegramBotClient, Update>? OnMessage;
        TelegramBotClient client;

        public Host(string token)
        {
            client = new TelegramBotClient(token);

        }

        public void Start()
        {
            client.StartReceiving(Update, Error);
            Console.WriteLine("bot has been started");
        }

        private async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            Console.WriteLine(update.Message?.Text ?? "[not a text]");
            OnMessage?.Invoke(botClient, update);
            await Task.CompletedTask;
        }

        private async Task Error(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
        {
            throw new NotImplementedException();
            await Task.CompletedTask;
        }
    }
}

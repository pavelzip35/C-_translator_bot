using System;
using System.Net.Http; 
using System.Threading.Tasks; 
using Telegram.Bot; 
using Telegram.Bot.Args; 
using Newtonsoft.Json;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics.Contracts;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;

namespace C__bot
{
    class Program
    {
        private static ITelegramBotClient botClient;//Крч, это тебе не надо
        public static string UsersLanguage = "en";
        public static string TargetLanguage = "it";

        static void Main(string[] args)
        {

            Host botClient = new Host("6444476122:AAGokkXQn6UixLVw6hXR9fl9d1DeFKnpNiM");

            botClient.Start();
            botClient.OnMessage += BotOnMessage;
            Console.ReadLine();
        }



        // Обработчик события получения сообщения
        private static async void BotOnMessage(ITelegramBotClient botClient, Update e)
        {
            switch (e.Type)
            {
               case UpdateType.Message:
                    // Это тоже не надо
                    if (e.Message.Text == "/start")
                    {
                        await botClient.SendMessage(e.Message.Chat.Id, "Hello, this bot can translate your messages");

                        var replyKeyboard = new ReplyKeyboardMarkup(
                            new List<KeyboardButton[]>()
                            {
                         new KeyboardButton[]
                         {
                              new KeyboardButton("/translate")
                         },
                         new KeyboardButton[]
                         {
                              new KeyboardButton("/help")
                         },
                         new KeyboardButton[]
                         {
                              new KeyboardButton("/selectyourlang"),
                              new KeyboardButton("/selecttargetlang")
                         }
                            })
                        {

                            ResizeKeyboard = true,
                        };

                        await botClient.SendMessage(
                            e.Message.Chat.Id,
                            "Here are my functions!",
                            replyMarkup: replyKeyboard);

                        return;
                    }
                    // А вот здесь добавлены кнопки для Выбора языка пользователя
                    if (e.Message.Text == "/selectyourlang")
                    {
                        var inlineKeyboard = new InlineKeyboardMarkup(
                                            new List<InlineKeyboardButton[]>()
                                            {


                                        new InlineKeyboardButton[]
                                        {
                                            InlineKeyboardButton.WithCallbackData("English", "button_english"),// 1 аргумент - отображаемый текст в самом тг, 2 - можно сказать тэг кнопки
                                            InlineKeyboardButton.WithCallbackData("Russian", "button_russian"),// со строчки 157 мы к ним обращаемся
                                        },
                                        new InlineKeyboardButton[]
                                        {
                                            InlineKeyboardButton.WithCallbackData("German", "button_german"),
                                            InlineKeyboardButton.WithCallbackData("Spanish", "button_spanish"),
                                        },
                                            });

                        await botClient.SendMessage(
                            e.Message.Chat.Id,
                            "Select your language!",
                            replyMarkup: inlineKeyboard);

                        return;

                    }
                    //Здесь тоже есть кнопки
                    if (e.Message.Text == "/selecttargetlang")
                    {
                        var inlineKeyboard = new InlineKeyboardMarkup(
                                            new List<InlineKeyboardButton[]>()
                                            {


                                        new InlineKeyboardButton[] // тут создаем массив кнопок
                                        {
                                            InlineKeyboardButton.WithCallbackData("English", "button_english_target"),
                                            InlineKeyboardButton.WithCallbackData("Russian", "button_russian_target"),
                                        },
                                        new InlineKeyboardButton[]
                                        {
                                            InlineKeyboardButton.WithCallbackData("German", "button_german_target"),
                                            InlineKeyboardButton.WithCallbackData("Spanish", "button_spanish_target"),
                                        },
                                            });

                        await botClient.SendMessage(
                            e.Message.Chat.Id,
                            "Select target language!",
                            replyMarkup: inlineKeyboard);

                        return;

                    }
                    //Это тебе тоже не надо
                    if (e.Message.Text == "/translate")
                    {
                        await botClient.SendMessage(e.Message.Chat.Id, "Enter the text");
                        
                    }
                    else
                    {
                        if (e.Message.Text != null)
                        {
                            string translatedText = await TranslateText(e.Message.Text, TargetLanguage);
                            Console.WriteLine(translatedText);

                            await botClient.SendMessage(e.Message.Chat.Id, translatedText);
                        }
                    }

                    break;
                    // Во здесь мы их нажатия обрабатываем, код для этого я на хабре взял
                case UpdateType.CallbackQuery:
                    var callbackQuery = e.CallbackQuery;

                    var user = callbackQuery.From;

                    Console.WriteLine($"{user.FirstName} ({user.Id}) нажал на кнопку: {callbackQuery.Data}");

                    var chat = callbackQuery.Message.Chat;

                    switch (callbackQuery.Data)
                    {
                        // Data - это придуманный нами id кнопки, мы его указывали в параметре
                        // callbackData при создании кнопок. 

                        case "button_english":
                            {
                                await botClient.AnswerCallbackQuery(callbackQuery.Id);

                                UsersLanguage = "en";
                                await botClient.SendMessage(e.Message.Chat.Id, "English selected");
                                return;
                            }

                        case "button_russian":
                            {
                                await botClient.AnswerCallbackQuery(callbackQuery.Id);

                                UsersLanguage = "ru";
                                await botClient.SendMessage(e.Message.Chat.Id, "Russian selected");
                                return;
                            }

                        case "button_german":
                            {
                                await botClient.AnswerCallbackQuery(callbackQuery.Id);

                                UsersLanguage = "ger";
                                await botClient.SendMessage(e.Message.Chat.Id, "German selected");
                                return;
                            }
                        case "button_spanish":
                            {
                                await botClient.AnswerCallbackQuery(callbackQuery.Id);

                                UsersLanguage = "sp";
                                await botClient.SendMessage(e.Message.Chat.Id, "Spanish selected");
                                return;
                            }


                        case "button_english_target":
                            {
                                await botClient.AnswerCallbackQuery(callbackQuery.Id);

                                TargetLanguage = "en";
                                await botClient.SendMessage(e.Message.Chat.Id, "English selected");
                                return;
                            }

                        case "button_russian_target":
                            {
                                await botClient.AnswerCallbackQuery(callbackQuery.Id);

                                TargetLanguage = "ru";
                                await botClient.SendMessage(e.Message.Chat.Id, "Russian selected");
                                return;
                            }

                        case "button_german_target":
                            {
                                await botClient.AnswerCallbackQuery(callbackQuery.Id);

                                TargetLanguage = "ger";
                                await botClient.SendMessage(e.Message.Chat.Id, "German selected");
                                return;
                            }
                        case "button_spanish_target":
                            {
 
                                await botClient.AnswerCallbackQuery(callbackQuery.Id);
                                // Для того, чтобы отправить телеграмму запрос, что мы нажали на кнопку

                                TargetLanguage = "sp";
                                await botClient.SendMessage(e.Message.Chat.Id, "Spanish selected");
                                return;
                            }



                    }

                    return;

                    
            }
            
        }

        // Метод для перевода текста, сюда тоже лезть не надо поидее
        private static async Task<string> TranslateText(string text, string targetLanguage)
        {
            using (var httpClient = new HttpClient())
            {
                string url = $"https://api.mymemory.translated.net/get?q={text}&langpair={UsersLanguage}|{targetLanguage}";

                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode) 
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync(); // Чтение ответа как строки
                    dynamic result = JsonConvert.DeserializeObject(jsonResponse); // Десериализация JSON-ответа
                    return result.responseData.translatedText; // Возврат переведенного текста
                }
                else
                {
                    return "Translation Error";
                }
            }
        }
    }
}
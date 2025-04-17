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
        private static ITelegramBotClient botClient;//���, ��� ���� �� ����
        public static string UsersLanguage = "en";
        public static string TargetLanguage = "it";

        static void Main(string[] args)
        {

            Host botClient = new Host("6444476122:AAGokkXQn6UixLVw6hXR9fl9d1DeFKnpNiM");

            botClient.Start();
            botClient.OnMessage += BotOnMessage;
            Console.ReadLine();
        }



        // ���������� ������� ��������� ���������
        private static async void BotOnMessage(ITelegramBotClient botClient, Update e)
        {
            switch (e.Type)
            {
               case UpdateType.Message:
                    // ��� ���� �� ����
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
                    // � ��� ����� ��������� ������ ��� ������ ����� ������������
                    if (e.Message.Text == "/selectyourlang")
                    {
                        var inlineKeyboard = new InlineKeyboardMarkup(
                                            new List<InlineKeyboardButton[]>()
                                            {


                                        new InlineKeyboardButton[]
                                        {
                                            InlineKeyboardButton.WithCallbackData("English", "button_english"),// 1 �������� - ������������ ����� � ����� ��, 2 - ����� ������� ��� ������
                                            InlineKeyboardButton.WithCallbackData("Russian", "button_russian"),// �� ������� 157 �� � ��� ����������
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
                    //����� ���� ���� ������
                    if (e.Message.Text == "/selecttargetlang")
                    {
                        var inlineKeyboard = new InlineKeyboardMarkup(
                                            new List<InlineKeyboardButton[]>()
                                            {


                                        new InlineKeyboardButton[] // ��� ������� ������ ������
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
                    //��� ���� ���� �� ����
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
                    // �� ����� �� �� ������� ������������, ��� ��� ����� � �� ����� ����
                case UpdateType.CallbackQuery:
                    var callbackQuery = e.CallbackQuery;

                    var user = callbackQuery.From;

                    Console.WriteLine($"{user.FirstName} ({user.Id}) ����� �� ������: {callbackQuery.Data}");

                    var chat = callbackQuery.Message.Chat;

                    switch (callbackQuery.Data)
                    {
                        // Data - ��� ����������� ���� id ������, �� ��� ��������� � ���������
                        // callbackData ��� �������� ������. 

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
                                // ��� ����, ����� ��������� ���������� ������, ��� �� ������ �� ������

                                TargetLanguage = "sp";
                                await botClient.SendMessage(e.Message.Chat.Id, "Spanish selected");
                                return;
                            }



                    }

                    return;

                    
            }
            
        }

        // ����� ��� �������� ������, ���� ���� ����� �� ���� ������
        private static async Task<string> TranslateText(string text, string targetLanguage)
        {
            using (var httpClient = new HttpClient())
            {
                string url = $"https://api.mymemory.translated.net/get?q={text}&langpair={UsersLanguage}|{targetLanguage}";

                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode) 
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync(); // ������ ������ ��� ������
                    dynamic result = JsonConvert.DeserializeObject(jsonResponse); // �������������� JSON-������
                    return result.responseData.translatedText; // ������� ������������� ������
                }
                else
                {
                    return "Translation Error";
                }
            }
        }
    }
}
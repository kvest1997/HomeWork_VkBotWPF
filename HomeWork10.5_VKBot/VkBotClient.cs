using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HomeWork10._5_VKBot
{
    public class VkBotClient
    {
        private readonly MainWindow window;
        public ObservableCollection<MessageLog> BotMessageLog { get; set; }
        public List<Users> user;
        private readonly VkApi vkApi;

        private DateTime DateTimeFromUnixTime(long unixTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).LocalDateTime;
        }

        private void MessageListener()
        {
            foreach (dynamic conversation in vkApi.MessagesGetConversations())
            {
                string userId = conversation.conversation.peer.id;
                string firstName = "";
                string lastName = "";
                string fromId = "";
                long date;
                string messageId = "";

                foreach (dynamic history in vkApi.MessagesGetHistory(userId))
                {
                    dynamic userName = vkApi.UsersGet(userId);
                    firstName = $"{userName[0].first_name}";
                    lastName = $"{userName[0].last_name}";
                    fromId = history.from_id;
                    date = history.date;
                    messageId = history.conversation_message_id;
                    string textTemp = history.text;

                    window.Dispatcher.Invoke(() =>
                    {
                        foreach (var item in user)
                        {
                            if (item.Id == userId)
                            {
                                BotMessageLog.Add(new MessageLog(Convert.ToString(DateTimeFromUnixTime(date)),
                                                    userId,
                                                    messageId,
                                                    fromId,
                                                    textTemp,
                                                    item
                                                    ));

                            }
                        }
                    });
                }
            }
        }

        public void MessageListener(int userIndex)
        {
                this.BotMessageLog = new ObservableCollection<MessageLog>();

                string userId = user[userIndex].Id;
                dynamic userName = vkApi.UsersGet(userId);

                int botCount = BotMessageLog.Count;

                foreach (dynamic item in vkApi.MessagesGetHistory(userId))
                {
                    int msgId = item.conversation_message_id;
                    string text = item.text;
                    string firstName = userName[0].first_name;
                    string lastName = userName[0].last_name;
                    string fromId = item.from_id;
                    long date = item.date;

                    foreach (var usr in user)
                    {
                        if (usr.Id == userId)
                        {
                            window.Dispatcher.Invoke(() =>
                            {
                                Debug.WriteLine($"{date}, {userId}, {msgId}, {fromId}, {text}, {user[userIndex]}");
                                Debug.WriteLine("-----------------------------");
                                BotMessageLog.Add(new MessageLog(Convert.ToString(DateTimeFromUnixTime(date)),
                                    userId,
                                    Convert.ToString(msgId),
                                    fromId,
                                    text,
                                    usr
                                    ));
                            });
                        }
                    }
                }

        }

        private void GetUser()
        {
            user = new List<Users>();
            VkApi vkApi = new VkApi();

            foreach (dynamic conversation in vkApi.MessagesGetConversations())
            {
                string userId = conversation.conversation.peer.id;
                dynamic userName = vkApi.UsersGet(userId);

                string firstName = $"{userName[0].first_name}";
                string lastName = $"{userName[0].last_name}";

                user.Add(new Users(userId, firstName, lastName));
            }
        }

        public void SendMessage(string text, string Id)
        {
            vkApi.MessagesSend(Id, text);
        }

        public VkBotClient(MainWindow W)
        {
            this.BotMessageLog = new ObservableCollection<MessageLog>();
            this.window = W;

            vkApi = new VkApi();

            GetUser();
            MessageListener();
        }

        public void SaveToJson(Users user)
        {
            JObject userObj = new JObject();
            JArray mainTree = new JArray();

            foreach (var msg in BotMessageLog)
            {
                if (user.Id == msg.Id)
                {
                    userObj["Id"] = $"{msg.User.Id}";
                    userObj["FirstName"] = $"{msg.User.FirstName}";
                    userObj["LastName"] = $"{msg.User.LastName}";
                    userObj["Msg"] = $"{msg.Msg}";
                    userObj["Time"] = $"{msg.Time}";
                    mainTree.Add(userObj);
                }
            }

            File.WriteAllText($"{user.FirstName}_{user.LastName}.json", mainTree.ToString());
        }
    }
}

   
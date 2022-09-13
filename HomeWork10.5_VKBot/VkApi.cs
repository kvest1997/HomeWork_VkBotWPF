using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeWork10._5_VKBot
{
    class VkApi
    {
        private readonly WebClient wc;

        private Uri uri;
        private string url;

        private readonly string startUrl;
        private string methodName;
        private readonly string v;

        public string Token { get; set; }

        public VkApi()
        {
            wc = new WebClient() { Encoding = Encoding.UTF8 };

            startUrl = $@"https://api.vk.com/method/";
            Token = File.ReadAllText(@"token.txt");
            v = "5.131";
        }

        /// <summary>
        /// Возвращает список бесед
        /// </summary>
        /// <returns></returns>
        public JToken[] MessagesGetConversations()
        {
            methodName = "messages.getConversations";
            url = $"{startUrl}{methodName}?access_token={Token}&v={v}";

            uri = new Uri(url);

            var r = wc.DownloadString(uri);

            var conversation = JObject.Parse(r)["response"]["items"].ToArray();

            return conversation;
        }

        /// <summary>
        /// Возвращает историю сообщений конкретного пользоватя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Id пользователя у которого надо прочитать сообщение</returns>
        public JToken[] MessagesGetHistory(string userId)
        {
            methodName = "messages.getHistory";
            url = $"{startUrl}{methodName}?user_id={userId}&access_token={Token}&v={v}";

            uri = new Uri(url);

            var r = wc.DownloadString(uri);

            var msgs = JObject.Parse(r)["response"]["items"].ToArray();

            return msgs;
        }

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sendText"></param>
        public void MessagesSend(string userId, string sendText)
        {
            methodName = "messages.send";

            url = $"{startUrl}{methodName}?user_id={userId}&random_id=0&message={sendText}&access_token={Token}&v={v}";
            uri = new Uri(url);
            wc.DownloadString(uri);

            Thread.Sleep(500);
        }

        /// <summary>
        /// Возвращает информацию об пользователе
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Id возвращаемого пользователя</returns>
        public JToken[] UsersGet(string userId)
        {
            methodName = "users.get";

            url = $"{startUrl}{methodName}?user_id={userId}&fields=about, sex, bdate, city, country, domain&access_token={Token}&v={v}";
            uri = new Uri(url);

            var r = wc.DownloadString(uri);

            var msgs = JObject.Parse(r)["response"].ToArray();

            return msgs;
        }
    }
}

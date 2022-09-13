using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork10._5_VKBot
{
    public class MessageLog
    {
        public string Time { get; set; }
        public string Id { get; set; }
        public string ConversationMessageId { get; set; }
        public string FromId { get; set; }
        public string Msg { get; set; }
        public Users User { get; set; }

        public MessageLog(string time, string id,string conversationMessageId,string fromId, string msg, Users user)
        {
            this.FromId = fromId;
            this.Id = id;
            this.Time = time;
            this.Msg = msg;

            if (fromId== "-215361802")
                this.User = new Users(fromId, "VK","Bot");
            else
                this.User = user;

            this.ConversationMessageId = conversationMessageId;
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SlackPurge
{
    public class Message
    {
        public string type { get; set; }
        public string subtype { get; set; }
        public string text { get; set; }
        public string user { get; set; }
        public string ts { get; set; }
        public string client_msg_id { get; set; }
    }

    public class RootObject
    {
        public bool ok { get; set; }
        public List<Message> messages { get; set; }
        public bool has_more { get; set; }
        public int pin_count { get; set; }
    }
}

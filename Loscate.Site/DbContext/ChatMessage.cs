using System;
using System.Collections.Generic;

#nullable disable

namespace Loscate.Site.DbContext
{
    public partial class ChatMessage
    {
        public int Id { get; set; }
        public int SendUserId { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public int DialogId { get; set; }

        public virtual Dialog Dialog { get; set; }
        public virtual FirebaseUser SendUser { get; set; }
    }
}

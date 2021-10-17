using System;
using System.Collections.Generic;

#nullable disable

namespace Loscate.Site.DbContext
{
    public partial class Dialog
    {
        public Dialog()
        {
            ChatMessages = new HashSet<ChatMessage>();
        }

        public int Id { get; set; }
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }

        public virtual FirebaseUser UserId1Navigation { get; set; }
        public virtual FirebaseUser UserId2Navigation { get; set; }
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
    }
}

using System;
using Loscate.DTO.Firebase;

namespace Loscate.DTO.Social
{
    public class Message
    {
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public FirebaseUser SendUser { get; set; }
    }
}
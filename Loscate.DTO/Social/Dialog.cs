using Loscate.DTO.Firebase;

namespace Loscate.DTO.Social
{
    public class Dialog
    {
        public FirebaseUser Companion { get; set; }
        public Message LastMessage { get; set; }
    }
}
namespace Loscate.DTO.Firebase
{
    public class FirebaseUser
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
        public string PictureUrl { get; set; }
        public bool IsRegistered { get; set; }

        public FirebaseUser() {}

        public FirebaseUser(string uid, string name, string eMail, string pictureUrl, bool isRegistered)
        {
            UID = uid;
            Name = name;
            EMail = eMail;
            PictureUrl = pictureUrl;
            IsRegistered = isRegistered;
        }
    }
}

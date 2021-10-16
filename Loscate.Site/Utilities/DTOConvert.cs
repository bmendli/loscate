using Loscate.DTO.Firebase;

namespace Loscate.Site.Utilities
{
    public static class DTOConvert
    {
        public static DbContext.FirebaseUser ConvertToDb(this DTO.Firebase.FirebaseUser user)
        {
            return new DbContext.FirebaseUser()
            {
                Uid = user.UID,
                Email = user.EMail,
                Name = user.Name,
                PictureUrl = user.PictureUrl
            };
        }
        
        public static DTO.Firebase.FirebaseUser ConvertToDto(this DbContext.FirebaseUser user)
        {
            return new DTO.Firebase.FirebaseUser()
            {
                UID = user.Uid,
                EMail = user.Email,
                Name = user.Name,
                PictureUrl = user.PictureUrl
            };
        }
        
        public static DTO.Social.Message ConvertToDto(this DbContext.ChatMessage message)
        {
            return new DTO.Social.Message()
            {
               Text = message.Text,
               Time = message.Time,
               SendUser = message.SendUser.ConvertToDto()
            };
        }
    }
}
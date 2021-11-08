using Loscate.DTO.Firebase;
using Loscate.DTO.Map;
using Loscate.Site.DbContext;
using Xamarin.Forms.Maps;

namespace Loscate.Site.Utilities
{
    public static class DTOConvert
    {
        public static DTO.Map.Pin ConvertToDto(this DbContext.Pin pin)
        {
            return new DTO.Map.Pin()
            {
                Latitude = pin.Latitude,
                Longitude = pin.Longitude,
                ShortName = pin.ShortName,
                FullName = pin.FullName,
                PhotoBase64 = pin.Photo,
                Id = pin.Id
            };
        }
        
        public static DbContext.Pin ConvertToDto(this DTO.Map.Pin pin, DbContext.FirebaseUser user)
        {
            return new DbContext.Pin()
            {
                Latitude = pin.Latitude,
                Longitude = pin.Longitude,
                ShortName = pin.ShortName,
                FullName = pin.FullName,
                Photo = pin.PhotoBase64,
                UserId = user.Id
            };
        }
        
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
            if (message == null) return null;
            return new DTO.Social.Message()
            {
               Text = message.Text,
               Time = message.Time,
               SendUser = message.SendUser.ConvertToDto()
            };
        }
    }
}
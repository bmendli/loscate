using System.Collections.Generic;
using System.Threading.Tasks;
using Loscate.App.Map;
using Loscate.App.Services;
using Loscate.App.Utilities;
using Loscate.DTO.Map;
using System.Text.Json;

namespace Loscate.App.ApiRequests.Map
{
    public static class MapRequests
    {
        public static async Task<List<DTO.Map.Pin>> GetPins(IFirebaseAuthenticator firebaseAuthenticator)
        {
            return await ApiRequest.MakeRequest<List<DTO.Map.Pin>>(firebaseAuthenticator, "api/map/getPins");
        }
        
        public static async Task<string> AddPin(IFirebaseAuthenticator firebaseAuthenticator, Pin pin)
        {
            var values = new Dictionary<string, string>
            {
                { "fullName",  pin.FullName},
                { "shortName",  pin.ShortName},
                { "latitude",  pin.Latitude.ToString()},
                { "longitude",  pin.Longitude.ToString()},
                { "photo",  pin.PhotoBase64},
            };

            return await ApiRequest.MakePostRequest<string>(firebaseAuthenticator, $"api/map/addPin", values);
        }
    }
}
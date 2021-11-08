using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Loscate.Site.DbContext;
using Loscate.Site.Services;
using Loscate.Site.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loscate.Site.Controllers.Map
{
    [Authorize]
    [Route("api/map/[controller]")]
    public class AddPinController : Controller
    {
        private readonly LoscateDbContext loscateDbContext;
        private readonly UserService userService;

        public AddPinController(LoscateDbContext loscateDbContext, UserService userService)
        {
            this.loscateDbContext = loscateDbContext;
            this.userService = userService;
        }

        [HttpPost]
        public string Post(string fullName, string shortName, string latitude, string longitude, string photo)
        {
            try
            {
                var dbUser = userService.GetDbUser(User);
                var dbPin = new Pin()
                {
                    Latitude = double.Parse(latitude, CultureInfo.InvariantCulture),
                    Longitude = double.Parse(longitude, CultureInfo.InvariantCulture),
                    UserId = dbUser.Id,
                    Photo = photo,
                    FullName = fullName,
                    ShortName = shortName
                };
                
                loscateDbContext.Pins.Add(dbPin);
                loscateDbContext.SaveChanges();
                
                return "OK";
            }
            catch (Exception e)
            {
                return e.ToString() + "::" + latitude;
            }
        }
    }
}
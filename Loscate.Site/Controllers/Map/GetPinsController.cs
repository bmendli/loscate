using System.Collections.Generic;
using System.Linq;
using Loscate.Site.DbContext;
using Loscate.Site.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pin = Loscate.DTO.Map.Pin;

namespace Loscate.Site.Controllers.Map
{
    [Authorize]
    [Route("api/map/[controller]")]
    public class GetPinsController: Controller
    {
        private readonly LoscateDbContext loscateDbContext;

        public GetPinsController(LoscateDbContext loscateDbContext)
        {
            this.loscateDbContext = loscateDbContext;
        }

        [HttpGet]
        public List<Pin> Get()
        {
            return loscateDbContext.Pins.Include(p=>p.User).ToList().Select(pin => pin.ConvertToDto()).ToList();
        }
    }
}
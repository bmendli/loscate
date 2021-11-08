using System;
using System.Collections.Generic;
using System.Linq;
using Loscate.Site.DbContext;
using Loscate.Site.Services;
using Loscate.Site.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Loscate.Site.Controllers.Social.Message
{
    [Authorize]
    [Route("api/social/message/[controller]")]
    public class GetDialogMessagesController : Controller
    {
        private readonly LoscateDbContext loscateDbContext;
        private readonly UserService userService;

        public GetDialogMessagesController(LoscateDbContext loscateDbContext, UserService userService)
        {
            this.loscateDbContext = loscateDbContext;
            this.userService = userService;
        }

        [HttpGet]
        public List<DTO.Social.Message> Get(string pairUid)
        {
            var dbUser = userService.GetDbUser(User);
            var companionUser = loscateDbContext.FirebaseUsers.SingleOrDefault(u => u.Uid == pairUid);

            if (companionUser == null)
            {
                return null;
            }

            var dialog = loscateDbContext.Dialogs.SingleOrDefault((d =>
                (d.UserId1 == dbUser.Id && d.UserId2 == companionUser.Id) ||
                (d.UserId2 == dbUser.Id && d.UserId1 == companionUser.Id)));

            if (dialog == null)
            {
                return null;
            }

            var dbMessages = loscateDbContext.ChatMessages.Where(m => m.DialogId == dialog.Id).Include(d => d.SendUser);


            return dbMessages.Select(dbMessage => dbMessage.ConvertToDto()).ToList();
        }
    }
}
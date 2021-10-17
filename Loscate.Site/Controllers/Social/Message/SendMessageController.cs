using System;
using System.Linq;
using Loscate.Site.DbContext;
using Loscate.Site.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loscate.Site.Controllers.Social.Message
{
    [Authorize]
    [Route("api/social/message/[controller]")]
    public class SendMessageController : Controller
    {
        private readonly LoscateDbContext loscateDbContext;
        private readonly UserService userService;
        
        public SendMessageController(LoscateDbContext loscateDbContext, UserService userService)
        {
            this.loscateDbContext = loscateDbContext;
            this.userService = userService;
        }

        [HttpGet]
        public string Get(string pairUid, string msg)
        {
            var dbUser = userService.GetDbUser(User);
            var toUser = loscateDbContext.FirebaseUsers.SingleOrDefault(u => u.Uid == pairUid);
            
            if (toUser == null)
            {
                return "nullToUser";
            }
            
            var dialog = loscateDbContext.Dialogs.SingleOrDefault((d =>
                (d.UserId1 == dbUser.Id && d.UserId2 == toUser.Id) ||
                (d.UserId2 == dbUser.Id && d.UserId1 == toUser.Id)));
            
            
            
            if (dialog == null)
            {
                dialog = new DbContext.Dialog()
                {
                    UserId1Navigation = dbUser,
                    UserId2Navigation = toUser
                };
                loscateDbContext.Dialogs.Add(dialog);
                loscateDbContext.SaveChanges();
            }


            try
            {
                var newMessage = new ChatMessage()
                {
                    DialogId = dialog.Id,
                    Text = msg,
                    Time = DateTime.Now,
                    SendUserId = dbUser.Id
                };
                loscateDbContext.ChatMessages.Add(newMessage);
                loscateDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            
          

            return "OK";
        }
    }
}
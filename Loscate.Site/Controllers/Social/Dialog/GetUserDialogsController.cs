using System;
using System.Collections.Generic;
using System.Linq;
using Loscate.Site.DbContext;
using Loscate.Site.Services;
using Loscate.Site.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Loscate.Site.Controllers.Social.Dialog
{
    [Authorize]
    [Route("api/social/dialog/[controller]")]
    public class GetUserDialogsController : Controller
    {
        private readonly LoscateDbContext loscateDbContext;
        private readonly UserService userService;

        public GetUserDialogsController(LoscateDbContext loscateDbContext, UserService userService)
        {
            this.loscateDbContext = loscateDbContext;
            this.userService = userService;
        }

        [HttpGet]
        public List<DTO.Social.Dialog> Get()
        {
            var dtoDialogs = new List<DTO.Social.Dialog>();
            var dbUser = userService.GetDbUser(User);

            var dbDialogs = loscateDbContext.Dialogs.Where(d => d.UserId1 == dbUser.Id || d.UserId2 == dbUser.Id)
                .Include(u1 => u1.UserId1Navigation).Include(u2 => u2.UserId2Navigation);

            foreach (var dbDialog in dbDialogs.ToList())
            {
                var companion = dbDialog.UserId1 == dbUser.Id
                    ? dbDialog.UserId2Navigation.ConvertToDto()
                    : dbDialog.UserId1Navigation.ConvertToDto();

                var lastMessage = loscateDbContext.ChatMessages.
                    Where(c => c.DialogId == dbDialog.Id).
                    Include(c=>c.SendUser).
                    OrderByDescending(c=>c.Time).
                    FirstOrDefault();

                if (lastMessage == null)
                {
                    lastMessage = new ChatMessage()
                    {
                        Text = "Нет сообщений",
                        Time = DateTime.Now,
                        SendUser = dbUser
                    };
                }

                
                dtoDialogs.Add(new DTO.Social.Dialog()
                {
                    Companion = companion,
                    LastMessage = lastMessage.ConvertToDto()
                });
                
            }


            return dtoDialogs;
        }
    }
}
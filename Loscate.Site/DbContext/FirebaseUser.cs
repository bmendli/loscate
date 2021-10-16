﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Loscate.Site.DbContext
{
    public partial class FirebaseUser
    {
        public FirebaseUser()
        {
            DialogUserId1Navigations = new HashSet<Dialog>();
            DialogUserId2Navigations = new HashSet<Dialog>();
        }

        public int Id { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PictureUrl { get; set; }

        public virtual ICollection<Dialog> DialogUserId1Navigations { get; set; }
        public virtual ICollection<Dialog> DialogUserId2Navigations { get; set; }
    }
}

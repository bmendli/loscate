using System;
using System.Collections.Generic;

#nullable disable

namespace Loscate.Site.DbContext
{
    public partial class Pin
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int UserId { get; set; }
        public string Photo { get; set; }

        public virtual FirebaseUser User { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnouncementWebAPI.Models
{
    public class UpdateAnnounceModel
    {
        public string ItemID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ExpireDate { get; set; }
        public string PublishDate { get; set; }
        public string RePublish { get; set; }
        public string ReExpireDate { get; set; }
        public string RePublishDate { get; set; }
        public string Top { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public string Important { get; set; }
    }
}
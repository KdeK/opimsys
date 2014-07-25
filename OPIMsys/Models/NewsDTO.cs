using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPIMsys.Models
{
    public class NewsDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PubDate { get; set; }
        public string PDFLink { get; set; }
        public string Link { get; set; }
        public string Content { get; set; }
    }

    public class NewsDTOAPI : NewsDTO
    {
        public string Id { get; set; }
    }
}
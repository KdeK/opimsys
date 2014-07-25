using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPIMsys.Models
{
    public class Vimeo
    {
        public string generated_in  { get; set; }
        public string stat  { get; set; }
        public vidGroup videos { get; set; }
    }
    public class vidGroup
    {
        public string on_this_page { get; set; }
        public string page { get; set; }
        public string perpage { get; set; }
        public string total { get; set; }
        public Video[] video { get; set; }
    }
    public class Video
    {
        public string allow_adds { get; set; }
        public string embed_privacy { get; set; }
        public string id { get; set; }
        public string is_hd { get; set; }
        public string is_transcoding { get; set; }
        public string is_watchlater { get; set; }
        public string license { get; set; }
        public string privacy { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string upload_date { get; set; }
        public string modified_date { get; set; }
        public string number_of_likes { get; set; }
        public string number_of_plays { get; set; }
        public string number_of_comments { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string duration { get; set; }
        public vidOwner owner {get; set; }
        public vidCast cast { get; set; }
      public vidUrls urls { get; set; }
        public vidThumbnails thumbnails { get; set; }
    }
    public class vidOwner 
    {
        public string display_name { get; set; }
        public string id { get; set; }
        public string is_plus { get; set; }
        public string is_pro { get; set; }
        public string is_staff { get; set; }
        public string profileurl { get; set; }
        public string realname { get; set; }
        public string username { get; set; }
        public string videosurl { get; set; }
        public vidPortraits portraits { get; set; }
        

    }
    public class vidPortraits
    {
        public vidPortrait[] portrait { get; set; }
    }
    public class vidPortrait
    {
        public string height { get; set; }
        public string width  { get; set; }
        public string _content { get; set; }
    }
    public class vidCast
    {
        public vidMember member { get; set; }
    }
    public class vidMember
    {
        public string display_name { get; set; }
        public string id { get; set; }
        public string role { get; set; }
        public string username { get; set; }
    }
    public class vidUrls 
    {
        public vidUrl[] url  { get; set; }
    }
    public class vidUrl 
    {
        public string type  { get; set; }
        public string _content {get; set; }
    }
    public class vidThumbnails
    {
        public vidThumbnail[] thumbnail { get; set; }
    }
    public class vidThumbnail
    {
        public string height { get; set; }
        public string width { get; set; }
        public string _content { get; set; }
    }
}
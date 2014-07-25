using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;


namespace OPIMsys.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        [Required]
        public string Name { get; set; }
        public string LogoURL { get; set; }
        public int Revision { get; set; }
       
        // Navigation properties
        public virtual ICollection<StockSymbol> StockSymbols { get; set; }
        public virtual ICollection<People> People { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<NewsSource> NewsSources { get; set; }
        public virtual ICollection<CompanyInformation> CompanyInformation { get; set; }
        public virtual ICollection<CompanyLink> CompanyLinks { get; set; }
        public virtual ICollection<CompanyFollower> CompanyFollowers { get; set; }
        public virtual ICollection<CompanyPage> CompanyPages { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<EventSource> EventSources { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
    public class CompanyInformation
    {
        [Key]
        public int CompanyInformationId { get; set; }

        [Required]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [Required]
        public int LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }

        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Slogan { get; set; }
        public string Strategy { get; set; }
        public string Letter { get; set; }
        public string Contact { get; set; }
        
    }
    public class CompanyLink
    {
        [Key]
        public int CompanyLinkId { get; set; }

        [Required]
        public int CompanyLinkTypeId { get; set; }
        [ForeignKey("CompanyLinkTypeId")]
        public virtual CompanyLinkType CompanyLinkType { get; set; }

        [Required]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [Required]
        public int LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }

        [Required]
        public string Link { get; set; }

    }
    public class CompanyPage
    {
        [Key]
        public int CompanyPageId { get; set; }

        [Required]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        public int PageId { get; set; }

        [Required]
        public string PageName { get; set; }

        [Required]
        public int Revision { get; set; }

        [Required]
        public int LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }

        [Required]
        public string UserName { get; set; }

        public DateTime PublishDate { get; set; }
        
        [Required]
        public DateTime EditDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        [Required]
        public bool Enabled { get; set; }
    }

    public class CompanyLinkType
    {
        [Key]
        public int CompanyLinkTypeId { get; set; }

        [Required]
        public string Title { get; set; }
    }
    public class CompanyMarketComparison
    {
        [Key]
        public int CompanyMarketComparisonId { get; set; }

        [Required]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [Required]
        public int StockSymbolId { get; set; }
        [ForeignKey("StockSymbolId")]
        public virtual StockSymbol StockSymbol { get; set; }
    }
    public class CompanyPeer
    {
        [Key]
        public int CompanyPeerId { get; set; }

        [Required]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [Required]
        public int PeerId { get; set; }
     }
    public class CompanyGroup
    {
        [Key]
        public int CompanyGroupId { get; set; }

        [Required]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [Required]
        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
    }
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        
        [Required]
        public string Name { get; set; }
    }
    public class CompanyFollower
    {
        [Key]
        public int CompanyFollowerId { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string CompanyName { get; set; }

        [Required]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
    }

}
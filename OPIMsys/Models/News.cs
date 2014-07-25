using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;


namespace OPIMsys.Models
{
    public class News
    {
        [Key]
        public int NewsId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime PubDate { get; set; }

        public string PDFLink { get; set; }

        [Required]
        public string Link { get; set; }

        public string Content { get; set; }

        [Required]
        public int NewsTypeId { get; set; }
        [ForeignKey("NewsTypeId")]
        public virtual NewsType NewsType { get; set; }

        [Required]
        public int NewsSourceId { get; set; }
        [ForeignKey("NewsSourceId")]
        public virtual NewsSource NewsSource { get; set; }

        [Required]
        public string NewsSourceIdentifier { get; set; }
    }

    public class NewsType
    {
        [Key]
        public int NewsTypeId { get; set; }

        [Required]
        public string Title { get; set; }
    }
    public class NewsSource
    {
        [Key]
        public int NewsSourceId { get; set; }

        [Required]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [Required]
        public string Link { get; set; }

        [Required]
        public int NewsSourceTypeId { get; set; }
        [ForeignKey("NewsSourceTypeId")]
        public virtual NewsSourceType NewsSourceType { get; set; }

        [Required]
        public int LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }

    }

    public class NewsSourceType
    {
        [Key]
        public int NewsSourceTypeId { get; set; }

        [Required]
        public string Title { get; set; }
        
    }

}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace OPIMsys.Models
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }

        public string SourceId { get; set; }

        [Required]
        public string Link { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string ThumbnailLink { get; set; }

        [Required]
        public DateTime PubDate { get; set; }

        [Required]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [Required]
        public int LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }

        [Required]
        public int DocumentTypeId { get; set; }
        [ForeignKey("DocumentTypeId")]
        public virtual DocumentType DocumentType { get; set; }

    }
    public class DocumentType
    {
        [Key]
        public int DocumentTypeId { get; set; }

        [Required]
        public string Title { get; set; }

    }
    public class DocumentApi
    {
        
        public string Link { get; set; }

        public string Title { get; set; }

        public DateTime PubDate { get; set; }

        public string DocumentType { get; set; }

        public string Language { get; set; }


    }
}
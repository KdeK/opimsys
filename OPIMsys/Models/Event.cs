using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;


namespace OPIMsys.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [Required]
        public int EventCategoryId { get; set; }
        [ForeignKey("EventCategoryId")]
        public virtual EventCategory EventCategory { get; set; }

        [Required]
        public int EventSourceId { get; set; }
        [ForeignKey("EventSourceId")]
        public virtual EventSource EventSource { get; set; }

        [Required]
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string SourceId { get; set; }

        public virtual ICollection<EventDetail> EventDetails { get; set; }
        
        
    }
    public class EventDetail
    {
        [Key]
        public int EventDetailId { get; set; }

        [Required]
        public int EventId { get; set; }
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        [Required]
        public string Summary { get; set; }
        public string Description { get; set; }

        [Required]
        public int LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }
    }

    public class EventCategory
    {
        [Key]
        public int EventCategoryId { get; set; }
        [Required]
        public string Title { get; set; }
    }
    public class EventSource
    {
        [Key]
        public int EventSourceId { get; set; }
        [Required]
        public string Link { get; set; }

        [Required]
        public int EventSourceTypeId { get; set; }
        [ForeignKey("EventSourceTypeId")]
        public virtual EventSourceType EventSourceType { get; set; }

        [Required]
        public int LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }

        [Required]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
    }
    public class EventSourceType
    {
        [Key]
        public int EventSourceTypeId { get; set; }
        [Required]
        public string Title { get; set; }
    }
    public class EventApi
    {
        public string Language { get; set; }
        public string Category { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationM { get; set; }
    }

}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace OPIMsys.Models
{
    public class Language
    {
        [Key]
        public int LanguageId { get; set; }

        [Required]
        public string Culture { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
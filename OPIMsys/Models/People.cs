using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;


namespace OPIMsys.Models
{
    public class People
    {
        [Key]
        [DatabaseGeneratedAttribute(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int PeopleId { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageURL { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public string CompanyName { get; set; }

        [Required]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        public virtual ICollection<PeopleType> PeopleTypes { get; set; }

        public virtual ICollection<PeopleInformation> PeopleInformation { get; set; }

    }
    public class PeopleInformation
    {
        [Key]
        public int PeopleInformationId { get; set; }

        [Required]
        public int PeopleId { get; set; }
        [ForeignKey("PeopleId")]
        public virtual People People { get; set; }

        [Required]
        public int LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }

        public string Title { get; set; }
        public string Bio { get; set; }
        
        
    }

    public class PeopleType
    {
        [Key]
        public int PeopleTypeId { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual ICollection<People> People { get; set; }
    }
}
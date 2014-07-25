using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace OPIMsys.Models
{
    public class CompanyData
    {
        [Key]
        public int CompanyDataId { get; set; }

        [Required]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [Required]
        public int CompanyDataVariableId { get; set; }
        [ForeignKey("CompanyDataVariableId")]
        public virtual CompanyDataVariable CompanyDataVariable { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public DateTime publishDate { get; set; }

    }
    public class CompanyDataVariable
    {
        [Key]
        public int CompanyDataVariableId { get; set; }

        [Required]
        public int CompanyDataTypeId { get; set; }
        [ForeignKey("CompanyDataTypeId")]
        public virtual CompanyDataType CompanyDataType { get; set; }

        [Required]
        public string Title { get; set; }
    }
    public class CompanyDataType
    {
        [Key]
        public int CompanyDataTypeId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<CompanyDataVariable> Variables { get; set; }
        public virtual ICollection<CompanyDataTypeCompany> Companies { get; set; }
    }
    public class CompanyDataTypeCompany
    {
        [Key]
        public int CompanyDataTypeCompanyId { get; set; }
        
        [Required]
        public int CompanyDataTypeId { get; set; }
        [ForeignKey("CompanyDataTypeId")]
        public virtual CompanyDataType CompanyDataType { get; set; }

        [Required]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
    }

    public class CompanyDataChartDTO
    {
        public string Title { get; set; }
        public string Notes { get; set; }
        public CompanyDataDTO[] Data { get; set; }
    }
    public class CompanyDataDTO
    {
        public string Company { get; set; }
        public CompanyDataVaribleDTO[] Variables { get; set; }
        public DateTime[] Dates { get; set; }
    }
    public class CompanyDataVaribleDTO
    {
        public string VariableName { get; set; }
        public decimal[] Values { get; set; }
    }
}
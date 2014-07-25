using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPIMsys.Models
{
    public class CompanyDTO
    {
        public string Name { get; set; }
        public string Slogan { get; set; }
        public string LogoURL { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Letter { get; set; }
        public string Strategy { get; set; }
        public string Contact { get; set; }
        public int Revision { get; set; }
        public CompanyDTOSite[] Links { get; set; }
        public CompanyDTOStock[] Stocks { get; set; }
        public CompanyDTOPeople[] People { get; set; }
        public CompanyDTOEvent[] Events { get; set; }
        public CompanyDTODocuments[] Documents { get; set; }
        public CompanyDTOPage[] Pages { get; set; }
    }
    public class CompanyDTOSite
    {
        public string Type { get; set; }
        public string Link { get; set; }
    }
    public class CompanyDTOStock
    {
        public string Market { get; set; }
        public string Symbol { get; set; }
        public CompanyDTODividend[] Dividends { get; set; }
    }
    public class CompanyDTODividend
    {
        public DateTime RecordDate { get; set; }
        public DateTime ExDividendDate { get; set; }
        public DateTime PayableDate { get; set; }
        public Double Dividend { get; set; }
        public string Notes { get; set; }
    }
    public class CompanyDTOPeople
    {
        public string Name { get; set; }
        public CompanyDTOPerson[] Persons { get; set; }
    }
    public class CompanyDTOPerson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string Bio { get; set; }
        public string Company { get; set; }
        public string ImageURL { get; set; }
    }
    public class CompanyDTOEvent
    {
        public string Category { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
    }
    public class CompanyDTODocuments
    {
        public string Name { get; set; }
        public CompanyDTODocument[] Documents { get; set; }
    }
    public class CompanyDTODocument
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string ThumbnailLink { get; set; }
        public DateTime PubDate { get; set; }
    }
    public class CompanyDTOPage
    {
        public string PageName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PubDate { get; set; }
    }
    public class CompanyDTOMarketComparison
    {
        public int Months { get; set; }
        public CompanyDTOMarketComparisonStock[] Stocks { get; set; }
    }
    public class CompanyDTOMarketComparisonStock
    {
        public int StockSymbolId { get; set; }
        public string Symbol { get; set; }
        public string Market { get; set; }
        public CompanyDTOMarketComparisonData[] Data { get; set; }
    }
    public class CompanyDTOMarketComparisonData
    {
        public DateTime date { get; set; }
        public Decimal value { get; set; }
    }
}
using System.Data.Entity;

namespace OPIMsys.Models
{
    public class OPIMsysContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<OPIMsys.Models.OPIMsysContext>());

        public OPIMsysContext() : base("name=OPIMsysContext")
        {
        }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<AccountApiKey> AccountApiKeys { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyInformation> CompanyInformation { get; set; }
        public DbSet<CompanyLink> CompanyLink { get; set; }
        public DbSet<CompanyLinkType> CompanyLinkType { get; set; }
        public DbSet<CompanyPeer> CompanyPeers { get; set; }
        public DbSet<CompanyPage> CompanyPages { get; set; }
        public DbSet<CompanyGroup> CompanyGroups { get; set; }
        public DbSet<CompanyMarketComparison> CompanyMarketComparisons { get; set; }
        public DbSet<CompanyFollower> CompanyFollowers { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventSource> EventSources { get; set; }
        public DbSet<EventSourceType> EventSourceTypes { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<EventDetail> EventDetail { get; set; }

        public DbSet<StockHistory> StockHistories { get; set; }
        public DbSet<StockQuote> StockQuotes { get; set; }
        public DbSet<StockSymbol> StockSymbols { get; set; }
        public DbSet<StockDividend> StockDividends { get; set; }
        public DbSet<Share> Shares { get; set; }

        public DbSet<Market> Markets { get; set; }
        public DbSet<Language> Language { get; set; }

        public DbSet<People> Peoples { get; set; }
        public DbSet<PeopleType> PeopleTypes { get; set; }
        public DbSet<PeopleInformation> PeopleInformation { get; set; }

        public DbSet<CompanyData> CompanyData { get; set; }
        public DbSet<CompanyDataVariable> CompanyDataVariables { get; set; }
        public DbSet<CompanyDataTypeCompany> CompanyDataTypeCompanies { get; set; }
        public DbSet<CompanyDataType> CompanyDataTypes { get; set; }
        
        public DbSet<News> News { get; set; }
        public DbSet<NewsType> NewsType { get; set; }
        public DbSet<NewsSource> NewsSources { get; set; }
        public DbSet<NewsSourceType> NewsSourceTypes { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<People>()
                .HasMany(p => p.PeopleTypes)
                .WithMany(t => t.People)
                .Map(mc =>
                {
                    mc.ToTable("PeopleJoinPeopleType");
                    mc.MapLeftKey("PeopleId");
                    mc.MapRightKey("PeopleTypeId");
                });
            
            base.OnModelCreating(modelBuilder);


        }
    }
}

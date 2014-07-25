namespace OPIMsys.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Web.Security;
    using System.Linq;

    using OPIMsys.Models;
    using WebMatrix.WebData;
using System.Text;
    using System.Security.Cryptography;

    internal sealed class Configuration : DbMigrationsConfiguration<OPIMsys.Models.OPIMsysContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OPIMsys.Models.OPIMsysContext context)
        {
  /*          if (!WebSecurity.Initialized)
                WebSecurity.InitializeDatabaseConnection("OPIMsysContext", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            if (!Roles.RoleExists("Administrator"))
                Roles.CreateRole("Administrator");
            if (!Roles.RoleExists("BackEndUser"))
                Roles.CreateRole("BackEndUser");
            if (!Roles.RoleExists("ApiReadUser"))
                Roles.CreateRole("ApiReadUser");

            if (!WebSecurity.UserExists("webmaster"))
                WebSecurity.CreateUserAndAccount("webmaster", "icmsw3bm", new { FullName = "Administrator", Email = "webmaster@bmir.com" });
            if (!WebSecurity.UserExists("testuser"))
                WebSecurity.CreateUserAndAccount("testuser", "testuser", new { FullName = "Test User", Email = "webmaster@bmir.com" });
            if(!WebSecurity.UserExists("stockapi"))
                WebSecurity.CreateUserAndAccount("stockapi", "nava7EcH", new { FullName = "Atock API", Email = "webmaster@bmir.com" });
            if (!Roles.GetRolesForUser("webmaster").Contains("Administrator"))
                Roles.AddUserToRole("webmaster", "Administrator");
            if (!Roles.GetRolesForUser("webmaster").Contains("BackEndUser"))
                Roles.AddUserToRole("webmaster", "BackEndUser");

            if (!Roles.GetRolesForUser("testuser").Contains("BackEndUser"))
                Roles.AddUserToRole("testuser", "BackEndUser");
            if (!Roles.GetRolesForUser("stockapi").Contains("ApiReadUser"))
                Roles.AddUserToRole("stockapi", "ApiReadUser");

            string key = "1234567890123456";
            string convert = "stockapi:nava7EcH";
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(convert);
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            int stockId = context.UserProfiles.Where(a => a.UserName=="stockapi").Single().UserId;
            
            byte[] buffer = Encoding.ASCII.GetBytes(convert);

            int companyId = context.Companies.Where(a => a.Name == "Eagle Energy Trust").Single().CompanyId;
            context.AccountApiKeys.AddOrUpdate(
                p => p.UserId,
                new AccountApiKey { UserId = stockId, ApiKey = "dec39crafrU9axaw2nahuq8663EwruRa", LoginToken = Convert.ToBase64String(resultArray, 0, resultArray.Length), CompanyId=companyId}
                );

            context.NewsSourceTypes.AddOrUpdate(
                p => p.Title,
                new NewsSourceType { Title = "MarketWireOilGas" },
                new NewsSourceType { Title = "GoogleMarketWire" },
                new NewsSourceType { Title = "MarketWire" },
                new NewsSourceType { Title = "MarketWiredImpress" },
                new NewsSourceType { Title = "CNW" },
                new NewsSourceType { Title = "API" }
                );
            context.Language.AddOrUpdate(
                p => p.Culture,
                new Language { Culture ="en", Title = "English" }
                );
            context.EventSourceTypes.AddOrUpdate(a => a.Title, new EventSourceType { Title = "API" });

            int newsSourceTypeID = context.NewsSourceTypes.Where(a => a.Title == "API").Single().NewsSourceTypeId;
            int eventSourceTypeID = context.EventSourceTypes.Where(a => a.Title == "API").Single().EventSourceTypeId;
            int LanguageId = context.Language.Where(a => a.Culture == "en").Single().LanguageId;
            context.NewsSources.AddOrUpdate(
                p => p.CompanyId,
                new NewsSource { CompanyId = companyId, LanguageId = LanguageId, NewsSourceTypeId = newsSourceTypeID, Link = "API" }
                );
            context.EventSources.AddOrUpdate(
                p => p.CompanyId,
                new EventSource { CompanyId = companyId, LanguageId = LanguageId, EventSourceTypeId = eventSourceTypeID, Link = "API" }
                );
   */         
        }
    }
}

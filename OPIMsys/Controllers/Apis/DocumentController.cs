using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using OPIMsys.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace OPIMsys.Controllers.Apis
{
    public class DocumentController : ApiController
    {
        private OPIMsysContext db = new OPIMsysContext();

        // PUT api/News/5
        public HttpResponseMessage PutDocument(string id, DocumentApi newapi)
        {
            AccountApiKey apiUser = GetAccount();
            Document document = new Document();
            DocumentType docType = new DocumentType();
            Language lang = new Language();
            try
            {
                document = db.Documents.Where(a => a.CompanyId == apiUser.CompanyId).Where(a => a.SourceId == id).Single();
                docType = db.DocumentTypes.Where(a => a.Title == newapi.DocumentType).Single();
                lang = db.Language.Where(a => a.Culture == newapi.Language).Single();
            }
            catch (Exception err)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            document.DocumentTypeId = docType.DocumentTypeId;
            document.Link = newapi.Link;
            document.PubDate = newapi.PubDate;
            document.Title = newapi.Title;
            document.LanguageId = lang.LanguageId;
            

            if (ModelState.IsValid)
            {
                //db.Entry(news).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                if (document.Link.Substring(document.Link.Length - 3) == "pdf")
                {
                    if (document.Link.StartsWith("http://"))
                        document.Link = document.Link.Substring(7);
                    else if (document.Link.StartsWith("https://"))
                        document.Link = document.Link.Substring(8);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://bmirphpdev2.com/pdfapi/test.php?url=http://" + document.Link);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream receiveStream = response.GetResponseStream();
                    // read the stream


                    var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);
                    var blobStorage = storageAccount.CreateCloudBlobClient();

                    CloudBlobContainer container = blobStorage.GetContainerReference("images");
                    if (container.CreateIfNotExist())
                    {
                        // configure container for public access
                        var permissions = container.GetPermissions();
                        permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                        container.SetPermissions(permissions);
                    }

                    //Azure
                    string uniqueBlobName = string.Format("images/docthumb-{0}.png", document.DocumentId.ToString());
                    CloudBlockBlob blob = blobStorage.GetBlockBlobReference(uniqueBlobName);
                    blob.Properties.ContentType = "image/png";
                    blob.UploadFromStream(receiveStream);
                    document.ThumbnailLink = blob.Uri.ToString();

                    receiveStream.Close();
                    response.Close();

                    db.Entry(document).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/News
        public HttpResponseMessage PostDocument(string id, DocumentApi newapi)
        {
            AccountApiKey apiUser = GetAccount();
            Document document = new Document();
            DocumentType docType = new DocumentType();
            Language lang = new Language();
            try
            {
                docType = db.DocumentTypes.Where(a => a.Title == newapi.DocumentType).Single();
                lang = db.Language.Where(a => a.Culture == newapi.Language).Single();
            }
            catch (Exception err)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            document = new Document
            {
                CompanyId = apiUser.CompanyId,
                DocumentTypeId = docType.DocumentTypeId,
                LanguageId = lang.LanguageId,
                Link = newapi.Link,
                PubDate = newapi.PubDate,
                Title = newapi.Title,
                SourceId = id,
                ThumbnailLink = "test"
            };
            
            if (ModelState.IsValid)
            {
                db.Documents.Add(document);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/News/5
        public HttpResponseMessage DeleteDividend(string id)
        {
            AccountApiKey apiUser = GetAccount();
            Document document = new Document();
            try
            {
                document = db.Documents.Where(c => c.CompanyId == apiUser.CompanyId).Where(a => a.SourceId == id).Single();    
            }
            catch (Exception err)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            db.Documents.Remove(document);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private AccountApiKey GetAccount()
        {
            IEnumerable<string> apiKeyHeaderValues = null;
            if (Request.Headers.TryGetValues("X-ApiKey", out apiKeyHeaderValues))
            {
                var apiKeyHeaderValue = apiKeyHeaderValues.First();
                var webapis = db.AccountApiKeys.Where(a => a.ApiKey == apiKeyHeaderValue);
                if (webapis.Count() == 1)
                    return webapis.Single();
            }
            throw new Exception("No Company associated with that API key");
        }
    }
}

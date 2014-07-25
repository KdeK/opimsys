using OPIMsys.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

using DotNetOpenAuth;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using System.Text;
using System.Security.Cryptography;
using System.Web.Helpers;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;


namespace OPIMsys.Controllers.Apis
{
    public class VideoController : ApiController
    {
        private OPIMsysContext db = new OPIMsysContext();
        public vidGroup Get(int id, string apikey = "")
        {
            if (User.Identity.Name != null)
                if (!OPIMsys.Filters.ApiKeyHandler.ApiKeyToUser(apikey, Request))
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
            if (!Roles.IsUserInRole("ReportAPI") && !Roles.IsUserInRole("ApiReadUser"))
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
            AccountApiKey apiUser = OPIMsys.Filters.ApiKeyHandler.KeyToAccount(apikey, Request);
            if(apiUser.CompanyId != id)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
            if(apiUser.CompanyId == 14 || apiUser.CompanyId == 1019)
            {

                string oauth_consumer_key = "012d2e7c1375a0a4cc1ec429cb977ca0a3395bfc";
                string oauth_consumer_secret = "9722a9d93aa3e1207a13876b63b0962994f68724";
                string oauth_token = "0a2c0f86933bb231ae778bc51e8a871e";
                string oauth_token_secret = "10dbed9f65799c21334e59a77eb7521f39ccdbad";
                
                string url = "https://vimeo.com/api/rest/v2";
                
                OAuthBase Obase = new OAuthBase();
                string oauth_timestamp = Obase.GenerateTimeStamp();
                string oauth_nonce = Obase.GenerateNonce();
                string nurl = "";
                string paramsOut = "";
                string paramsIn = "format=json&method=vimeo.videos.getAll&sort=newest&full_response=1";
                string oauth_signature = Obase.GenerateSignature(new Uri(url+"?"+paramsIn), oauth_consumer_key, oauth_consumer_secret, oauth_token, oauth_token_secret, "POST", oauth_timestamp, oauth_nonce,out nurl,out paramsOut);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(nurl + "?"+ paramsOut+"&oauth_signature="+oauth_signature);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
      
               
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string returnVal = "";
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    returnVal = reader.ReadToEnd();
                }
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                Vimeo vimeo = (Vimeo)json_serializer.Deserialize(returnVal, typeof(Vimeo));

                return vimeo.videos;
            }
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
        }
   
    }
}

using OPIMsys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Security;

namespace OPIMsys.Filters
{
    public class ApiKeyHandler : DelegatingHandler
    {
        /* CORS stuff */
        const string Origin = "Origin";
        const string AccessControlRequestMethod = "Access-Control-Request-Method";
        const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
        const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
        const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";

        

        public ApiKeyHandler(HttpConfiguration httpConfiguration)
        {
            InnerHandler = new HttpControllerDispatcher(httpConfiguration); 
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //IEnumerable<string> apiKeyHeaderValues = null;
            if (!ApiKeyToUser("", request))
            {
                return base.SendAsync(request, cancellationToken);
            }
     /*       if (request.Headers.TryGetValues("X-ApiKey", out apiKeyHeaderValues))
            {
                var apiKeyHeaderValue = apiKeyHeaderValues.First();

                

            

            }
       */
            //Non CORS
            return base.SendAsync(request, cancellationToken);

           // return request.Headers.Contains(Origin) ?
           //     ProcessCorsRequest(request, ref cancellationToken) :
           //     base.SendAsync(request, cancellationToken);
        }
        public static AccountApiKey KeyToAccount(string apikey, HttpRequestMessage request)
        {
            using (OPIMsysContext db = new OPIMsysContext())
            {
                IEnumerable<string> apiKeyHeaderValues = null;
                if (request.Headers.TryGetValues("X-ApiKey", out apiKeyHeaderValues))
                {
                    var apiKeyHeaderValue = apiKeyHeaderValues.First();
                    var webapis = db.AccountApiKeys.Where(a => a.ApiKey == apiKeyHeaderValue);
                    if (webapis.Count() == 1)
                        return webapis.Single();
                }
                if (apikey != "")
                {
                    var webapis = db.AccountApiKeys.Where(a => a.ApiKey == apikey);
                    if (webapis.Count() == 1)
                        return webapis.Single();
                }
            }
            return null;
            //throw new Exception("No Company associated with that API key");
        }
        public static bool ApiKeyToUser(string apikey, HttpRequestMessage request)
        {
            using (OPIMsysContext db = new OPIMsysContext())
            {
               
                var webapi = KeyToAccount(apikey, request);
                if (webapi == null)
                    return false;
                string key = "1234567890123456";
                byte[] toEncryptArray = Convert.FromBase64String(webapi.LoginToken);
                byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();

                string userPass = UTF8Encoding.UTF8.GetString(resultArray);

                var parts = userPass.Split(":".ToCharArray());
                var username = parts[0];
                var password = parts[1];

                if (!Membership.ValidateUser(username, password))
                {
                    return false;
                }

                var identity = new GenericIdentity(username, "Basic");
                string[] roles = Roles.Provider.GetRolesForUser(username);
                var principal = new GenericPrincipal(identity, roles);
                Thread.CurrentPrincipal = principal;
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = principal;
                }
                return true;
            }
        }
        private Task<HttpResponseMessage> ProcessCorsRequest(HttpRequestMessage request, ref CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Options)
            {
                return Task.Factory.StartNew<HttpResponseMessage>(() =>
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    AddCorsResponseHeaders(request, response);
                    return response;
                }, cancellationToken);
            }
            else
            {
                return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>(task =>
                {
                    HttpResponseMessage resp = task.Result;
                    resp.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
                    return resp;
                });
            }
        }
        private static void AddCorsResponseHeaders(HttpRequestMessage request, HttpResponseMessage response)
        {
            response.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());

            string accessControlRequestMethod = request.Headers.GetValues(AccessControlRequestMethod).FirstOrDefault();
            if (accessControlRequestMethod != null)
            {
                response.Headers.Add(AccessControlAllowMethods, accessControlRequestMethod);
            }

            string requestedHeaders = string.Join(", ", request.Headers.GetValues(AccessControlRequestHeaders));
            if (!string.IsNullOrEmpty(requestedHeaders))
            {
                response.Headers.Add(AccessControlAllowHeaders, requestedHeaders);
            }
        } 

    }
}
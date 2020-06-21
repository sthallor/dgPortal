using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Helpers;

namespace DataGumboInterface.Service
{
    public abstract class StructuredDataService<T>
    {
        static StructuredDataService()
        {
            ServerName = ConfigurationManager.AppSettings["GumboStructuredServerName"];
            ApiKey = ConfigurationManager.AppSettings["GumboStructuredApiKey"];
            AccessKey = ConfigurationManager.AppSettings["GumboStructuredAccessKey"];

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
        }

        public static string ServerName { get; set; }
        public static string ApiKey { get; set; }
        public static string AccessKey { get; set; }

        public abstract string GumboTypeName { get; }

        public virtual T GetById(int id)
        {
            string url = string.Format("https://{0}/api/{1}/{2}?apikey={3}&accesskey={4}", ServerName, GumboTypeName, id, ApiKey, AccessKey);
            url = url.Replace(" ", "%20");
            url = url.Replace("$", "%24");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (WebResponse webResponse = request.GetResponse())
            {
                using (Stream webStream = webResponse.GetResponseStream())
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        string response = responseReader.ReadToEnd();
                        responseReader.Close();
                        return Json.Decode<T>(response);
                    }
                }
            }
        }

        public virtual void Delete(int id)
        {
            Delete(id, GetById(id));
        }

        public virtual void Delete(int id, T t)
        {
            string url = string.Format("https://{0}/api/{1}/{2}?apikey={3}&accesskey={4}", ServerName, GumboTypeName, id, ApiKey, AccessKey);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            string gumboPayload = Json.Encode(t);

            request.Method = "DELETE";
            request.ContentType = "application/json";
            request.ContentLength = gumboPayload.Length;
            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(gumboPayload);
            requestWriter.Close();

            using (WebResponse webResponse = request.GetResponse())
            {
                using (Stream webStream = webResponse.GetResponseStream())
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        string response = responseReader.ReadToEnd();
                        responseReader.Close();
                    }
                }
            }
        }

        public virtual void Update(int id, T t)
        {
            string url = string.Format("https://{0}/api/{1}/{2}?apikey={3}&accesskey={4}", ServerName, GumboTypeName, id, ApiKey, AccessKey);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            string gumboPayload = Json.Encode(t);

            request.Method = "PUT";
            request.ContentType = "application/json";
            request.ContentLength = gumboPayload.Length;
            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(gumboPayload);
            requestWriter.Close();

            using (WebResponse webResponse = request.GetResponse())
            {
                using (Stream webStream = webResponse.GetResponseStream())
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        string response = responseReader.ReadToEnd();
                        responseReader.Close();
                    }
                }
            }
        }

        public virtual IEnumerable<T> GetAll()
        {
            string url = string.Format("https://{0}/api/{1}?apikey={2}&accesskey={3}", ServerName, GumboTypeName, ApiKey, AccessKey);
            url = url.Replace(" ", "%20");
            url = url.Replace("$", "%24");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (WebResponse webResponse = request.GetResponse())
            {
                using (Stream webStream = webResponse.GetResponseStream())
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        string response = responseReader.ReadToEnd();
                        responseReader.Close();
                        return Json.Decode<T[]>(response);
                    }
                }
            }
        }

    }
}

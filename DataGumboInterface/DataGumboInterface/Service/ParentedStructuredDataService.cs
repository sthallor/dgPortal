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
    public abstract class ParentedStructuredDataService<T> : StructuredDataService<T>
    {
        public abstract string GumboParentIdName { get; }

        public virtual IEnumerable<T> GetByParentId(int parentId)
        {
            string url = string.Format("https://{0}/api/{1}?{2}={3}&apikey={4}&accesskey={5}", ServerName, GumboTypeName, GumboParentIdName, parentId, ApiKey, AccessKey);
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

using Admin.Portal.API.Core.Const;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Admin.Portal.API.Filters
{
    public class Context
    {
        private protected object Respond { get; set; }

        public Context(object respond)
        {
            Respond = respond;
        }

        public ContentResult ToContextResult(int statusCode = (int)HttpStatusCode.OK)
        {
            if (!Respond.GetType().Namespace.StartsWith("System") || Respond.GetType().FullName.Contains("DataTable") || Respond.GetType().FullName.Contains("Collections") || Respond.GetType().FullName.Contains("DataSet"))
                return new ContentResult { StatusCode = statusCode, ContentType = Config.CONTENT_TYPE_APPLICATION_JSON, Content = JsonConvert.SerializeObject(Respond) };
            else
                return new ContentResult { StatusCode = statusCode, ContentType = Config.CONTENT_TYPE_APPLICATION_TEXT, Content = Respond.ToString() };
        }
    }
}

using System;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace SamuraiServer.Models
{
    public class DynamicJsonBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (!controllerContext.HttpContext.Request.ContentType.StartsWith
                ("application/json", StringComparison.OrdinalIgnoreCase))
            {
                // not JSON request
                return null;
            }

            var inpStream = controllerContext.HttpContext.Request.InputStream;
            inpStream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(controllerContext.HttpContext.Request.InputStream);
            var bodyText = reader.ReadToEnd();
            reader.Close();

            if (String.IsNullOrEmpty(bodyText))
            {
                // no JSON data
                return null;
            }

            var fullJSON = JsonConvert.DeserializeObject<dynamic>(bodyText);

            try
            {
                return fullJSON[bindingContext.ModelName];
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace SamuraiServer.Tests.API
{
    public static class Extensions
    {
        public static dynamic AsDynamic(this object obj)
        {
            //would be nice to restrict to anonymous types - but alas no.
            IDictionary<string, object> toReturn = new ExpandoObject();

            foreach (var prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead))
            {
                toReturn[prop.Name] = prop.GetValue(obj, null);
            }

            return toReturn;
        }

        public static string SerializeModel(this JsonResult result)
        {
            var serializer = new JsonSerializer();
            var builder = new StringBuilder();
            TextWriter writer = new StringWriter(builder);
            var textWriter = new JsonTextWriter(writer);
            serializer.Serialize(textWriter, result.Data);

            var jsonString = builder.ToString();
            return jsonString;
        }
    }
}
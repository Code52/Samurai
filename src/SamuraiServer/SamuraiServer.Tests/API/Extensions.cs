using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

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
    }
}
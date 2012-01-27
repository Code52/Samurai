using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    
    public static class Extensions
    {
        private static Random _random = new Random();

        public static T RandomElement<T>(this IEnumerable<T> source)
        {
            var count = source.Count();
            return source.ElementAt(_random.Next(count));
        }
    }
}

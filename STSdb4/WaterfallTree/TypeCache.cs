using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace STSdb4.WaterfallTree
{
    public class TypeCache
    {
        private static readonly ConcurrentDictionary<string, Type> cache = new ConcurrentDictionary<string, Type>();

        public static Type GetType(string fullName)
        {
            var type = Type.GetType(fullName, false);
            if (type != null)
                return type;

            return cache.GetOrAdd(fullName, (x) =>
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = assembly.GetType(fullName);
                    if (type != null)
                        return type;
                }

                return null; //once return null - always return null
            });
        }
    }
}

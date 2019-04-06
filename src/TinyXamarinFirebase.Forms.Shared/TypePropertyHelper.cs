using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace TinyXamarinFirebase.Froms
{

    public static class TypePropertyHelper
    {
        public static Dictionary<Type, Dictionary<string, (PropertyInfo, FirebaseAttribute)>> cache = new Dictionary<Type, Dictionary<string, (PropertyInfo, FirebaseAttribute)>>();

        public static Dictionary<string, (PropertyInfo, FirebaseAttribute)> GetFirebaseProperties<T>()
        {
            return GetFirebaseProperties(typeof(T));
        }

        public static Dictionary<string, (PropertyInfo, FirebaseAttribute)> GetFirebaseProperties(Type type)
        {

            if (!cache.ContainsKey(type))
            {
                var ret = new Dictionary<string, (PropertyInfo, FirebaseAttribute)>();
                foreach (var prp in type.GetProperties().Where(d => d.CanWrite))
                {
                    if (prp.GetCustomAttribute(typeof(FirebaseAttribute)) is FirebaseAttribute attr)
                    {
                        ret.Add(attr.Name, (prp, attr));
                    }
                }
                cache.Add(type, ret);
                return ret;
            }

            return cache[type];


        }
    }
}

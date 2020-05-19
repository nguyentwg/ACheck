using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage.File;

namespace ACheckAPI.Models
{
    public static class Extensions
    {
        public static IDictionary<TKey, TValue> NullIfEmpty<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null || !dictionary.Any())
            {
                return null;
            }
            return dictionary;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
            {
                action(element);
            }
            return source;
        }

        public static void CopyPropertiesTo<T>(this T source, T dest)
        {
            var plist = from prop in typeof(T).GetProperties() where prop.CanRead && prop.CanWrite select prop;

            foreach (PropertyInfo prop in plist)
            {
                //dest.GetType().GetProperty(prop.Name).SetValue(dest, prop.GetValue(source));
                prop.SetValue(dest, prop.GetValue(source, null), null);
            }
        }

        private static object CloneObject(object o)
        {
            Type t = o.GetType();
            PropertyInfo[] properties = t.GetProperties();

            Object p = t.InvokeMember("", System.Reflection.BindingFlags.CreateInstance,
                null, o, null);

            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    pi.SetValue(p, pi.GetValue(o, null), null);
                }
            }

            return p;
        }


        public static string DetailedCompare<T>(this T oldObject, T newObj)
        {
            dynamic _oldObject = JsonConvert.DeserializeObject(oldObject.ToString());
            dynamic _newObject = JsonConvert.DeserializeObject(newObj.ToString());
            Dictionary<string, string> oldValue = new Dictionary<string, string>();
            Dictionary<string, string> newValue = new Dictionary<string, string>();
            FieldInfo[] fi = oldObject.GetType().GetFields();
            var s = _newObject.ToObject<Dictionary<string, object>>();
            foreach (var property in _oldObject.ToObject<Dictionary<string, object>>())
            {
                if (property.Value != null && s[property.Key]!= null && !property.Value.Equals(s[property.Key]))
                {
                    oldValue.Add(property.Key, property.Value.ToString());
                    newValue.Add(property.Key, s[property.Key].ToString());
                }
                if(s[property.Key] == null || property.Value == null)
                {
                    string _new = s[property.Key] == null ? "null" : s[property.Key];
                    string _old = property.Value == null ? "null" : property.Value;
                    oldValue.Add(property.Key, _old);
                    newValue.Add(property.Key, _new);
                }
            }
            string json = JsonConvert.SerializeObject(new
            {
                oldValue = oldValue,
                newValue = newValue
            });
            return json;
        }
        
        
    }
    public class Variance
    {
        public string Prop { get; set; }
        public object Value { get; set; }
    }
    
    
}

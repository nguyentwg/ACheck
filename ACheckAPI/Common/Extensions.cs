using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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
                prop.SetValue(dest, prop.GetValue(source, null), null);
            }
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

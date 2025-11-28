using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace PerfumeShop.Helpers
{
    public static class SessionExtensions
    {

        public static void SetObject(this ISession session, string key, object value)
                => session.SetString(key, JsonConvert.SerializeObject(value)); //luu roi chuyen ep tu string ve json
        public static T GetObject<T>(this ISession session , string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Outparts2.Helpers
{
    public static class SessionHelper
    {
        //SetObjectAsJson method sets the key and value for a session object as a Json string. 
        //On the other hand the helper method GetObjectFromJson retrieves the object for a key after deserializing the Json string. 

        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}

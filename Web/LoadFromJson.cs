using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Web
{
    public static class LoadFromJson
    {
        public static string Foo()
        {
            JObject o = new JObject();
            o["Links"] = File.ReadAllText("appsettings.json");
            Console.WriteLine(o.ToString());
            return o.ToString();
        }
    }
}

using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MvcRDMG.Tests
{
    public class Helpers
    {
        public static List<T> DeseraliazerJSON<T>(string fileName)
        {
            var json = File.ReadAllText("Data/" + fileName);
            var jsonList = JsonConvert.DeserializeObject<List<T>>(json);
            return jsonList;
        }
    }
}
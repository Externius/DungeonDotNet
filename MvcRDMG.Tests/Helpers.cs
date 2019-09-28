using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MvcRDMG.Tests
{
    public class Helpers
    {
        private static readonly Helpers _instance = new Helpers();
        public static Helpers Instance => _instance;
        private Helpers()
        {

        }
        public List<T> DeseraliazerJSON<T>(string fileName)
        {
            string json = File.ReadAllText("Data/" + fileName);
            var jsonList = JsonConvert.DeserializeObject<List<T>>(json);
            return jsonList;
        }
    }
}
using Newtonsoft.Json;
using System.Reflection;

namespace Fiber_Optic_System_Designer.ReadJSON
{
    // I USED THIS https://www.newtonsoft.com/json/help/html/ReadingWritingJSON.htm
    public class ReadJSONFile
    {
        public static string ReadFile(string url)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            StreamReader reader = new StreamReader(assembly.GetManifestResourceStream($"Fiber_Optic_System_Designer.{url}"));
            return reader.ReadToEnd();
        }

        public static List<List<Tuple<string, string>>> Read_JSON_File(string fileURL)
        {
            string values = ReadFile(fileURL);
            List<List<Tuple<string, string>>> result = new List<List<Tuple<string, string>>>();
            JsonTextReader reader = new JsonTextReader(new StringReader(values));

            List<Tuple<string, string>> tmp = new List<Tuple<string, string>>();
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    string name = reader.Value.ToString().Trim();
                    reader.Read();
                    string value = reader.Value.ToString().Trim();
                    tmp.Add(new Tuple<string, string>(name, value));
                }
                else if (tmp.Count() > 0)
                {
                    result.Add(tmp);
                    tmp = new List<Tuple<string, string>>();
                }
            }
            return result;
        }


    }
}
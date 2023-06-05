using Newtonsoft.Json;
using System.Reflection;

namespace Fiber_Optic_System_Designer.ReadJSON
{
    // I USED THIS https://www.newtonsoft.com/json/help/html/ReadingWritingJSON.htm
    public class ReadJSONFile
    {
        public static String ReadFile(String url)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            StreamReader reader = new StreamReader(assembly.GetManifestResourceStream($"Fiber_Optic_System_Designer.{url}"));
            return reader.ReadToEnd();
        }

        public static List<List<Tuple<String, String>>> Read_JSON_File(String fileURL)
        {
            String values = ReadFile(fileURL);
            List<List<Tuple<String, String>>> result = new List<List<Tuple<String, String>>>();
            JsonTextReader reader = new JsonTextReader(new StringReader(values));

            List<Tuple<String, String>> tmp = new List<Tuple<String, String>>();
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    String name = reader.Value.ToString().Trim();
                    reader.Read();
                    String value = reader.Value.ToString().Trim();
                    tmp.Add(new Tuple<String, String>(name, value));
                }
                else if (tmp.Count() > 0)
                {
                    result.Add(tmp);
                    tmp = new List<Tuple<String, String>>();
                }
            }
            return result;
        }


    }
}
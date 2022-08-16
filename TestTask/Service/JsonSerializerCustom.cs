using System.Text.RegularExpressions;

namespace TestTask.Service
{
    public class JsonSerializerCustom<T>
    {
        public Dictionary<string, string> ConvertJsonToDictionary(string jsonString)
        {

            var rez = new Dictionary<string, string>();
            var pattern = @"\s+";
            jsonString = Regex.Replace(jsonString, pattern, " ").Trim().Replace("{{", "{ {").Replace("}}", "} }").TrimStart('{').TrimEnd('}');

            int complextObjectAmount = jsonString.Where(x => x == '{').Count();

            List<string> templist = new List<string>();
            for (int i = 0; i < complextObjectAmount; i++)
            {
                var startIndex = jsonString.IndexOf('{');
                var endIndex = jsonString.IndexOf('}') + 1;

                string subString = jsonString.Substring(startIndex, endIndex - startIndex);
                templist.Add(subString);

                jsonString = jsonString.Replace(subString, "\"subString" + i + "\"");
            }

            List<string> listStrin = jsonString.Replace(", \"", "~").Split(new char[] { '~' }).ToList();

            foreach (var item in listStrin)
            {
                string[] str = item.Replace(@"""", " ").Split(new char[] { ':' });
                string val = str[1].Trim();
                if (val.Contains("subString"))
                {
                    int ind = int.Parse(val[val.Length - 1].ToString());
                    rez.Add(str[0].Trim().ToLower(), templist[ind]);
                }
                else
                {
                    rez.Add(str[0].Trim().ToLower(), str[1].Trim());
                }
            }
            return rez;
        }

        public string ConvertObjectToJson(Object obj)
        {
            string jsonString = "{";

            var propItem = obj.GetType().GetProperties();
            foreach (var prop in propItem)
            {
                if (prop.PropertyType == typeof(string))
                {
                    jsonString += $"\"{prop.Name}\": \"{prop.GetValue(obj)}\", ";
                }
                else if (prop.PropertyType == typeof(long))
                {
                    jsonString += $"\"{prop.Name}\": {prop.GetValue(obj)}, ";
                }
                else if (!prop.PropertyType.FullName.StartsWith("System"))
                {
                    jsonString += $"\"{prop.Name}\":";
                    jsonString += ConvertObjectToJson(prop.GetValue(obj));
                    jsonString += ", ";
                }
            }
            jsonString = jsonString.TrimEnd().TrimEnd(',');
            jsonString += "}";

            return jsonString;
        }

        public string ConvertListObjectToJson(List<T> listObj)
        {
            string jsonString = "[";

            foreach (var item in listObj)
            {
                jsonString += ConvertObjectToJson(item);
                jsonString += ", ";
            }
            jsonString = jsonString.TrimEnd().TrimEnd(',');
            jsonString += "]";

            return jsonString;
        }

        public Object ConvertFromJsonToObject(String jsonString, Type type)
        {
            Dictionary<string, string> dictJson = ConvertJsonToDictionary(jsonString);

            var propItem = type.GetProperties();

            var newObj = Activator.CreateInstance(type);

            foreach (var prop in propItem)
            {
                if (prop.PropertyType == typeof(string))
                {
                    try
                    {
                        prop.SetValue(newObj, dictJson[prop.Name.ToLower()]);
                    }
                    catch
                    {
                        prop.SetValue(newObj, "");
                    }

                }
                else if (prop.PropertyType == typeof(long))
                {
                    try
                    {
                        prop.SetValue(newObj, Convert.ToInt64(dictJson[prop.Name.ToLower()]));
                    }
                    catch
                    {
                        prop.SetValue(newObj, 0);
                    }

                }
                else if (!prop.PropertyType.FullName.StartsWith("System"))
                {
                    try
                    {
                        Object obj = ConvertFromJsonToObject(dictJson[prop.Name.ToLower()], prop.PropertyType);
                        prop.SetValue(newObj, obj);
                    }
                    catch
                    {
                        prop.SetValue(newObj, null);
                    }
                }
            }
            return newObj;
        }
    }
}

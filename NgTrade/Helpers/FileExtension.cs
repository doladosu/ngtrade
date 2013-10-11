using System.Collections.Generic;
using System.Text;

namespace NgTrade.Helpers
{
    public static class FileExtension
    {
        public static string GetCsv<T>(this List<T> list)
        {
            var sb = new StringBuilder();

            //Get the properties for type T for the headers
            var propInfos = typeof(T).GetProperties();
            for (var i = 0; i <= propInfos.Length - 1; i++)
            {
                sb.Append(propInfos[i].Name);

                if (i < propInfos.Length - 1)
                {
                    sb.Append(",");
                }
            }

            sb.AppendLine();

            //Loop through the collection, then the properties and add the values
            for (var i = 0; i <= list.Count - 1; i++)
            {
                T item = list[i];
                for (var j = 0; j <= propInfos.Length - 1; j++)
                {
                    var o = item.GetType().GetProperty(propInfos[j].Name).GetValue(item, null);
                    if (o != null)
                    {
                        var value = o.ToString();

                        //Check if the value contans a comma and place it in quotes if so
                        if (value.Contains(","))
                        {
                            value = string.Concat("\"", value, "\"");
                        }
                        if (value.Contains("12:00:00"))
                        {
                            value = value.Replace("12:00:00", "");
                        }
                        if (value.Contains("  AM"))
                        {
                            value = value.Replace("  AM", "");
                        }
                        if (value.Contains("/"))
                        {
                            value = value.Replace("/", "-");
                        }
                        //Replace any \r or \n special characters from a new line with a space
                        if (value.Contains("\r"))
                        {
                            value = value.Replace("\r", " ");
                        }
                        if (value.Contains("\n"))
                        {
                            value = value.Replace("\n", " ");
                        }

                        sb.Append(value);
                    }

                    if (j < propInfos.Length - 1)
                    {
                        sb.Append(",");
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

    }
}
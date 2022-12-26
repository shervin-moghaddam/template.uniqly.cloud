using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace robotmanden.Code
{
    public static class JSonHelperClass
    {
        public static DataTable JsonToDataTable(string jsonString)
        {
            var jsonLinq = JObject.Parse(jsonString);

            // Find the first array using Linq  
            JToken srcArray = jsonLinq.Descendants().First(d => d is JArray);
            var trgArray = new JArray();

            foreach (JObject row in srcArray.Children<JObject>())
            {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties())
                {

                    // Only include JValue types  
                    if (column.Value is JValue)
                    {
                        cleanRow.Add(column.Name, column.Value);
                    }
                }
                trgArray.Add(cleanRow);
            }

            return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
        }

        public static string DataTableToJSON(DataTable table)
        {
            var JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StatusCode">0: ok, 1: warning, 2: error</param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public static JsonObject CreateJsonErrorReturn(int StatusCode, string ErrorMessage)
        {
            JsonObject JsonResponse = new JsonObject
            {
                { "statusCode", StatusCode },
                { "errorMessage", ErrorMessage }
            };
            return JsonResponse;
        }
    }
}

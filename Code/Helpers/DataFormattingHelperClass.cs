using System;
using static robotmanden.Code.DataTypeConversionHelperClass;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Html;

namespace robotmanden.Code
{
    public class DataFormattingHelperClass
    {
        /// <summary>
        /// Formats an element by returning class and data.
        /// Fixed names:
        /// @class = will be replaced with class
        /// @data =  will be reeplace with data from column
        /// </summary>
        /// <param name="html"></param>
        /// <param name="dc"></param>
        /// <returns></returns>
        public static HtmlString FormatElement(string html, object data)
        {
            string DataType = data.GetType().FullName.ToLower();
            if (DataType == "system.data.datacolumn") DataType = ((DataColumn)data).DataType.FullName.ToLower();

            switch (DataType)
            {
                case "system.data.int32":
                case "system.int32":
                    if (html.IndexOf("@class", StringComparison.OrdinalIgnoreCase) >= 0)
                        html = Regex.Replace(html, "@class", "textalign-right", RegexOptions.IgnoreCase);

                    if (html.IndexOf("@data", StringComparison.OrdinalIgnoreCase) >= 0)
                        html = html.Replace("@data", cStr(data));
                    break;
                case "system.data.datetime":
                case "system.datetime":
                    if (html.IndexOf("@class", StringComparison.OrdinalIgnoreCase) >= 0)
                        html = Regex.Replace(html, "@class", "textalign-right", RegexOptions.IgnoreCase);

                    if (html.IndexOf("@data", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // format date (only DK)
                        DateTime dt = cDate(data);
                        string strDateTime;
                        if (dt.Hour != 0 && dt.Minute != 0) // Date and time
                        {
                            strDateTime = dt.ToString("dd-MM-yyyy HH:mm");
                        }
                        else // Date only
                        {
                            strDateTime = dt.ToString("dd-MM-yyyy");
                        }
                        html = html.Replace("@data", strDateTime);
                    }
                    break;
                case "system.data.decimal":
                case "system.decimal":
                    if (html.IndexOf("@class", StringComparison.OrdinalIgnoreCase) >= 0)
                        html = Regex.Replace(html, "@class", "textalign-right", RegexOptions.IgnoreCase);

                    if (html.IndexOf("@data", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // format currency (only DK)
                        string strDecimal = cDec(data).ToString("N", CultureInfo.CreateSpecificCulture("da-DK"));
                        html = html.Replace("@data", strDecimal);
                    }
                    break;
                default:
                    if (html.IndexOf("@class", StringComparison.OrdinalIgnoreCase) >= 0)
                        html = Regex.Replace(html, "@class", "textalign-left", RegexOptions.IgnoreCase);
                    if (html.IndexOf("@data", StringComparison.OrdinalIgnoreCase) >= 0)
                        html = html.Replace("@data", cStr(data));
                    break;
            }
            return new HtmlString(html) ;
        }
        
        
    }
}

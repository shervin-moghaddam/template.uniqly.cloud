using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace template.Code
{
    public class DataTypeConversionHelperClass
    {
        /// <summary>
        /// Converts and trim object to string
        /// </summary>
        public static string cStr(object o)
        {
            try
            {
                if (o == null | o is DBNull)
                {
                    return "";
                }

                string c = Convert.ToString(o).Trim();
                if (string.IsNullOrEmpty(c))
                {
                    c = "";
                }

                return c;
            }
            catch
            {
                //GeneralLog("System", "CONVERSION ERROR: " + ex.Message, "Conversion Error (cStr)", 2, false);
                return "";
            }
        }

        /// <summary>
        /// Converts to int32 
        /// </summary>
        public static int cInt(object o)
        {
            try
            {
                if (o == null | o is DBNull)
                {
                    return 0;
                }

                int c = Convert.ToInt32(o);
                return c;
            }
            catch
            {
                //GeneralLog("System", "CONVERSION ERROR: " + ex.Message, "Conversion Error (cInt)", 2, false);
                return 0;
            }
        }

        /// <summary>
        /// Converts to Decimal 
        /// </summary>
        public static decimal cDec(object o)
        {
            try
            {
                if (o == null | o is DBNull | o.ToString() == "")
                {
                    return 0;
                }

                decimal c = Convert.ToDecimal(o);
                return c;
            }
            catch
            {
                //GeneralLog("System", "CONVERSION ERROR: " + ex.Message, "Conversion Error (cDec)", 2, false);
                return 0;
            }
        }

        /// <summary>
        /// Returns a boolean 
        /// </summary>
        public static bool cBool(object o)
        {
            try
            {
                if (o == null | o is DBNull)
                {
                    return false;
                }

                bool c = Convert.ToBoolean(o);
                return c;
            }
            catch
            {
                //GeneralLog("System", "CONVERSION ERROR: " + ex.Message, "Conversion Error (cBool)", 2, false);
                return false;
            }
        }

        /// <summary>
        /// Returns whether a value in string can be converted to int
        /// </summary>
        /// <returns></returns>
        public static bool IsInt(string s)
        {
            try
            {
                float output;
                return float.TryParse(s, out output);
            }
            catch
            {
                //GeneralLog("System", "CONVERSION ERROR: " + ex.Message, "Conversion Error (Is Int)", 2, false);
                return false;
            }
        }

        /// <summary>
        /// Returns a decimal value as string with decimals in correct regional format
        /// </summary>
        /// <returns></returns>
        public static string cMoneyStr(decimal o)
        {
            try
            {
                // Creates a CultureInfo for Danish in Denmark.
                CultureInfo dk = new CultureInfo("da-DK");

                string c;
                if (o == 0) c = o.ToString("0.00", dk);
                else if (Math.Abs(o) < 1) c = o.ToString("0.##");
                else
                {
                    Math.Round(o, 2);
                    c = o.ToString("0,0.00", dk);
                }

                return c;
            }
            catch
            {
                //GeneralLog("System", "CONVERSION ERROR: " + ex.Message, "Conversion Error (cMoneyStr)", 2, false);
                return "";
            }
        }

        public static string cMoneyAccountingStr(decimal o)
        {
            try
            {
                // Creates a CultureInfo for Danish in Denmark.
                CultureInfo dk = new CultureInfo("da-DK");

                string c;
                if (o == 0) c = o.ToString("0.0,00", dk);
                else if (Math.Abs(o) < 1) c = o.ToString("0.##");
                else
                {
                    Math.Round(o, 2);
                    c = o.ToString("0,0.00", dk);
                    c += " DKK";
                }

                return c;
            }
            catch
            {
                //GeneralLog("System", "CONVERSION ERROR: " + ex.Message, "Conversion Error (cMoneyStr)", 2, false);
                return "";
            }
        }

        /// <summary>
        /// Returns a datetime object.
        /// </summary>
        public static DateTime cDate(object o)
        {
            try
            {
                DateTime d = DateTime.MinValue;
                if (o != null && o.ToString() != "") d = Convert.ToDateTime(o);
                return d;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DATE CONVERSION ERROR: " + ex.Message);
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Returns a string object of datetime.
        /// </summary>
        public static string cDateStr(object o)
        {
            try
            {
                string DateStr = "";
                if (o != null && o.ToString() != "")
                    if (Convert.ToDateTime(o).Year > 1)
                        DateStr = Convert.ToDateTime(o).ToString("d");
                return DateStr;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("cDateStr ERROR: " + ex.Message);
                return "";
            }
        }

        /// <summary>
        /// Trims a string to at desired length.
        /// </summary>
        /// <param name="str">The string.</param>
        public static string TrimStr(string str, int len)
        {
            try
            {
                if (str.Length > len)
                {
                    str = str.Substring(0, str.Length - 1);
                    return str;
                }

                return str;
            }
            catch
            {
                //GeneralLog("System", "CONVERSION ERROR: " + ex.Message, "Conversion Error (TrimStr)", 2, false);
                return "";
            }
        }

        /// <summary>
        /// Converts integer to Byte
        /// </summary>
        /// <param name="i">Integer.</param>
        /// <returns></returns>
        public static byte cByte(int i)
        {
            try
            {
                byte b = Convert.ToByte(i);
                return b;
            }
            catch
            {
                //GeneralLog("System", "CONVERSION ERROR: " + ex.Message, "Conversion Error (cByte, int)", 2, false);
                return byte.MinValue;
            }
        }

        /// <summary>
        /// Returns a byte array
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static byte[] cByte(object o)
        {
            Byte[] bArray = new Byte[0];
            bArray = (Byte[])(o);
            MemoryStream mem = new MemoryStream(bArray);
            return bArray;
        }

        /// <summary>
        /// Converts an object to GUID.
        /// If the conversion is NOT succesful it will return guid.empty and log it.
        /// </summary>
        public static Guid cGUID(object o)
        {
            try
            {
                Guid ReturnGUID;
                Guid.TryParse(cStr(o), out ReturnGUID);
                return ReturnGUID;
            }
            catch
            {
                //GeneralLog("System", "CONVERSION ERROR: " + ex.Message, "Conversion Error (cGUID)", 2, false);
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Converts an image from bytearray to a given format
        /// </summary>
        /// <param name="bArray"></param>
        /// <returns></returns>
        public static Byte[] ConvertImage(Byte[] bArray, string FileType)
        {
            MemoryStream mem = new MemoryStream(bArray);
            MemoryStream returnms = new MemoryStream();

            Image imageStream = Image.FromStream(mem);
            switch (FileType.ToLower())
            {
                case "jpg":
                case "jpeg":
                    imageStream.Save(returnms, ImageFormat.Jpeg);
                    break;
                case "bmp":
                    imageStream.Save(returnms, ImageFormat.Bmp);
                    break;
                case "tif":
                    imageStream.Save(returnms, ImageFormat.Tiff);
                    break;
            }

            return returnms.ToArray();
        }

        /// <summary>
        /// Return an object list, converted from DataTable.
        /// This function is primarily made for converting DataTables into models.
        /// NOTE: The order of datatable columns and modul has to match 100%
        /// </summary>
        /// <param name="dt"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> ConvertToList<T>(DataTable dt)
        {
            List<string> columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            PropertyInfo[] properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        pro.SetValue(objT, row[pro.Name]);
                    }
                }

                return objT;
            }).ToList();
        }

        /// <summary>
        /// Takes a string with numbers and non-numeric characters, and return only numbers.
        /// This is useful when dealing with user typed numerics, to remove any culture formatting.
        /// Ex: "19.220,00 DKK" -> 1922000
        /// </summary>
        /// <param name="StringInput"></param>
        /// <param name="ForCurrency">Will set ending zeros so the result can always be divided by 100</param>
        /// <returns></returns>
        public static int GetIntFromString(string StringInput, bool ForCurrency = false)
        {
            if (ForCurrency)
            {
                // Check the last 3 chars of the string to find if the user put in a decimal seperator
                // and in that case, how may decimals were present after decimal seperator.
                // Ex: 14,9 has to become 1490 and not 149 when converted to int.
                char last = StringInput[^1];
                char secondlast;

                // In a case when the user inputs only two chars, and one is a seperator (which is unlikely to happen)
                if (StringInput.Length == 2)
                {
                    secondlast = StringInput[^2];
                    
                    // This would occurr only if inputString is ex: 14,9
                    if (!Char.IsDigit(secondlast) && (Char.IsDigit(last))) StringInput += "0";
                    else // No decimal seperator detected, add two zeros at the end
                        StringInput += "00";
                }
                else if (StringInput.Length > 2)
                {
                    secondlast = StringInput[^2];
                    char thirdlast = StringInput[^3];
                    
                    // If the thirdlast char is a non-digit, the input is assumably ending with ",90" which is fine
                    if (!Char.IsDigit(secondlast))
                        // This would occurr only if inputString is ex: 14,9
                        StringInput += "0";
                    else if
                        // The last char is a decimal seperator, add two zeros so 15, will be 15,00 - OR there is no decimal seperator at all
                        (!Char.IsDigit(last) || Char.IsDigit(thirdlast)) 
                        StringInput += "00";
                }
            }
            
            // Create a new string from only the digit chars, and return as int
            return cInt(new string(StringInput.Where(c => char.IsDigit(c)).ToArray()));
        }
    }
}
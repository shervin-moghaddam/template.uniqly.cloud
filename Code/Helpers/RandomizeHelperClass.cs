using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace template.Code
{
    public class RandomizeHelperClass
    {
        private static Random random = new Random();

        /// <summary>
        /// Returns a string with random set of characters
        /// </summary>
        /// <param name="length">How many characters?</param>
        /// <param name="IncludeNumbers"></param>
        /// <param name="OnlyUppercase"></param>
        /// <param name="IncludeSymbols"></param>
        /// <param name="NonNumericFirstCharacter">For randomized function names</param>
        /// <returns></returns>
        public static string GetRandomString(int length, bool IncludeNumbers = true, bool OnlyUppercase = false,
            bool IncludeSymbols = false, bool NonNumericFirstCharacter = true)
        {
            string ListOfChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (IncludeNumbers) ListOfChars += "1234567890";
            if (IncludeSymbols) ListOfChars += "!#%&()-@$*_:;?+";
            if (!OnlyUppercase) ListOfChars += "abcdefghijklmnopqrstuvxyz";

            string chars = ListOfChars;
            string ReturnString = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            if (NonNumericFirstCharacter)
                // if (Regex.IsMatch(ReturnString.Substring(0, 1), @"[,/]"))
                if (int.TryParse(ReturnString.Substring(0, 1), out int i))
                    ReturnString = "Q" + ReturnString;
            return ReturnString;
        }
    }
}
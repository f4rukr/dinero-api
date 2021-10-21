using System.Text.RegularExpressions;

namespace Klika.Dinero.Model.Extensions
{
    public static class BaseTypeExtensions
    {
        public static string Replace(this string input, Regex regex, string replaceValue)
        {
            return regex.Replace(input, replaceValue);
        }
    }
}
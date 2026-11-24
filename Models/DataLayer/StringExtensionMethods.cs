using System.Text;

namespace MedicaidEmploymentVerificationApplication.Models.DataLayer
{
    public static class StringExtensionMethods
    {
        public static string Slug(this string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str)
            {
                if (c == '&')
                {
                    sb.Append("and");
                    continue;
                }

                if (!char.IsPunctuation(c) || c == '-')
                {
                    sb.Append(c);
                }
            }

            var formatted = sb.ToString().Replace(' ', '-').ToLower();

            return formatted;
        }

        public static bool EqualsNoCase(this string str, string tocompare) => str?.ToLower() == tocompare?.ToLower();

        public static int ToInt(this string str)
        {
            if (int.TryParse(str, out int result))
            {
                return result;
            }
            return 0;
        }

        public static string Capitalize(this string str) => str?.Substring(0, 1).ToUpper() + str?.Substring(1).ToLower();
    }
}

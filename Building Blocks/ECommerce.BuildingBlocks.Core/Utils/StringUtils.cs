using System.Linq;

namespace ECommerce.Core.Utils
{
    public static class StringUtils
    {
        public static string ApenasNumeros(this string str, string cpf)
        {
            return new string(str.Where(char.IsDigit).ToArray());
        }
    }
}

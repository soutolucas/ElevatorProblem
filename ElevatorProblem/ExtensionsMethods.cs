using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ElevatorProblem
{
    public static class ExtensionsMethods
    {
        public static T Pop<T>(this List<T> list)
        {
            T result = default;

            if (list != null && list.Any())
            {
                result = (T)list.Take(1);
                list.Remove(result);
            }

            return result;
        }
    }
}

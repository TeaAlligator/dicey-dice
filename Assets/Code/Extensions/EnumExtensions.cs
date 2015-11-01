using System;
using System.Linq;

namespace Assets.Code.Extensions
{
    static class EnumExtensions
    {
        // based on http://stackoverflow.com/a/203389
        public static int BoundValue<T>(int value) where T : IComparable, IConvertible, IFormattable
        {
            var type = typeof(T);

            if (!type.IsSubclassOf(typeof(Enum)))
                throw new
                    InvalidCastException
                        ("Cannot cast '" + type.FullName + "' to System.Enum.");

            var enumValues = Enum.GetValues(type).Cast<int>();

            if (value > enumValues.Max())
                return enumValues.Min();

            if (value < enumValues.Min())
                return enumValues.Max();

            return value;
        }
    }
}

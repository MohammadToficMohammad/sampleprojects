
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NioStream.Pipe
{
    public static class Extensions
    {
        public static byte[] TrimEnd(this byte[] array)
        {
            int lastIndex = Array.FindLastIndex(array, b => b != 0);

            Array.Resize(ref array, lastIndex + 1);

            return array;
        }
        public static ArraySegment<T> GetSegment<T>(this T[] arr, int offset, int? count = null)
        {
            if (count == null) { count = arr.Length - offset; }
            return new ArraySegment<T>(arr, offset, count.Value);
        }

      

        public static void CopyPropertiesTo(this object fromObject, object toObject)
        {
            PropertyInfo[] toObjectProperties = toObject.GetType().GetProperties();
            foreach (PropertyInfo propTo in toObjectProperties)
            {
                PropertyInfo propFrom = fromObject.GetType().GetProperty(propTo.Name);
                if (propFrom != null && propFrom.CanWrite)
                    propTo.SetValue(toObject, propFrom.GetValue(fromObject, null), null);
            }
        }

        public static void CopyPropertiesExpecptIdTo<T>(this T fromObject, T toObject, string IdName = null) where T : class
        {
            PropertyInfo[] toObjectProperties = toObject.GetType().GetProperties();
            foreach (PropertyInfo propTo in toObjectProperties)
            {
                PropertyInfo propFrom = fromObject.GetType().GetProperty(propTo.Name);
                if (IdName != null)
                {
                    if (propFrom != null && propFrom.CanWrite && !propFrom.Name.Equals(IdName))
                        propTo.SetValue(toObject, propFrom.GetValue(fromObject, null), null);
                }
                else
                {
                    if (propFrom != null && propFrom.CanWrite)
                        propTo.SetValue(toObject, propFrom.GetValue(fromObject, null), null);
                }

            }
        }

       

    }
}

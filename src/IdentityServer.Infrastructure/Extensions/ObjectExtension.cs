using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 当前对象转换成特定类型，如果转换失败或者对象为null，返回defaultValue。
        /// 如果传入的是可空类型：会试着转换成其真正类型后返回。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue">转换出错或者对象为空的时候的返回值</param>
        /// <returns></returns>
        public static T ToSimpleT<T>(this object value, T defaultValue)
        {
            if (value == null)
            {
                return defaultValue;
            }
            else if (value is T)
            {
                return (T)value;
            }
            try
            {
                if (typeof(T).BaseType == typeof(Enum))
                {
                    object objValue = Enum.Parse(typeof(T), value.ToString());
                    return (T)objValue;
                }
                Type typ = typeof(T);
                if (typ.BaseType == typeof(ValueType) && typ.IsGenericType)//可空泛型
                {
                    Type[] typs = typ.GetGenericArguments();
                    return (T)Convert.ChangeType(value, typs[0]);
                }
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 更加安全的调用对象的ToString方法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SafeToString(this object value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            return value.ToString();
        }

        /// <summary>
        /// 安全获取值，当值为null时，不会抛出异常
        /// </summary>
        /// <param name="value">可空值</param>
        public static T SafeValue<T>(this T? value) where T : struct
        {
            return value ?? default(T);
        }
    }
}

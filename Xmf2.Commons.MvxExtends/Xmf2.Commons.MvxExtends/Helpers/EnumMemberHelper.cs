using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Xmf2.Commons.MvxExtends.Helpers
{
	public static class EnumMemberHelper
    {
        public static string ToEnumString<T>(T type)
        {
            Type enumType = typeof(T);
            string name = Enum.GetName(enumType, type);
            EnumMemberAttribute enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            return enumMemberAttribute.Value;
        }

        public static T ToEnum<T>(string str)
        {
            Type enumType = typeof(T);
            foreach (string name in Enum.GetNames(enumType))
            {
                EnumMemberAttribute enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                if (enumMemberAttribute.Value == str)
                {
	                return (T)Enum.Parse(enumType, name);
                }
            }
            //throw exception or whatever handling you want or
            throw new ArgumentException();
            //return default(T);
        }
    }
}

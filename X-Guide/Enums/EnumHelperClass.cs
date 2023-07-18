using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace X_Guide.Enums
{
    public static class EnumHelperClass
    {
        public static string GetEnumDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static string GetEnumDescription<T>(int value) where T : Enum
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new ArgumentException($"{nameof(T)} must be an enum type");
            }

            var enumValue = (T)Enum.ToObject(type, value);
            var descriptionAttribute = enumValue.GetType()
            .GetMember(enumValue.ToString())[0]
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .FirstOrDefault() as DescriptionAttribute;

            return descriptionAttribute?.Description ?? enumValue.ToString();
        }

        public static IEnumerable<ValueDescription> GetAllValuesAndDescriptions(Type t)
        {
            if (!t.IsEnum)
                throw new ArgumentException($"{nameof(t)} must be an enum type");

            return Enum.GetValues(t).Cast<Enum>().Select((e) => new ValueDescription() { Value = e, Description = e.GetEnumDescription() }).ToList();
        }

        public static IEnumerable<ValueDescription> GetAllIntAndDescriptions(Type t)
        {
            if (!t.IsEnum)
                throw new ArgumentException($"{nameof(t)} must be an enum type");

            return Enum.GetValues(t).Cast<Enum>().Select((e) => new ValueDescription() { Value = (int)Enum.Parse(t, e.ToString()), Description = e.GetEnumDescription() }).ToList();
        }
    }
}
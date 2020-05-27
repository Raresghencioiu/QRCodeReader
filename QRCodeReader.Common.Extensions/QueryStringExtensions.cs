using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace QRCodeReader.Common.Extensions
{
    public static class QueryStringExtensions
    {
        public static T FromQueryToObject<T>(this IQueryCollection query) where T : new()
        {
            var obj = new T();
            var properties = typeof(T).GetProperties();
            var dataMembers = new Dictionary<string, string>();

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<DataMemberAttribute>();
                dataMembers.Add(property.Name, attribute?.Name ?? property.Name);
            }

            foreach (var property in properties)
            {
                var queryParamName = dataMembers[property.Name];

                var valueAsString = query[queryParamName];
                var value = Parse(property.PropertyType, valueAsString);

                if (value == null)
                    continue;

                property.SetValue(obj, value);
            }
            return obj;
        }

        private static object Parse(Type dataType, string valueToConvert)
        {
            if (string.IsNullOrEmpty(valueToConvert)) 
            {
                return null;
            }

            var typeConverter = TypeDescriptor.GetConverter(dataType);

            return typeConverter.ConvertFromInvariantString(valueToConvert);
        }
    }
}

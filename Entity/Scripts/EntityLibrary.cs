using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sentience
{
    public static class EntityLibrary
    {
        public static IEnumerable<Type> FindDerivedTypes(Assembly assembly, Type baseType)
        {
            return assembly.GetTypes().Where(t => t != baseType && baseType.IsAssignableFrom(t));
        }

        public static List<string> GetDerivedTypeNames(Assembly assembly, Type baseType, string replace)
        {
            List<string> names = new List<string>();
            var types = FindDerivedTypes(assembly, baseType);
            foreach (var type in types) names.Add(type.Name.Replace(replace, ""));
            return names;
        }
    }
}
using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorWasmApp.Infra.DI
{
    public static class DIResolver
    {
        internal const string assemblyPrefix = "BlazorWasmApp";

        public static void Resolve(IServiceCollection collection)
        {
            var assemblies = GetAssemblies();
            var DIAttrs = assemblies.SelectMany(assembly =>
                assembly.DefinedTypes.Where(ca => ca.BaseType == typeof(DIAttribute))).ToArray();

            var attributedTypes = assemblies.SelectMany(assembly =>
                assembly.DefinedTypes
                    .Where(type => HasDIAttrs(type.CustomAttributes.Select(ca => ca.AttributeType).ToArray(), DIAttrs)))
                .ToArray();

            foreach (var attributedType in attributedTypes)
            {
                var attribute =
                    attributedType.CustomAttributes.FirstOrDefault(ca => DIAttrs.Contains(ca.AttributeType));
                var resolveType = (Type) attribute.ConstructorArguments.FirstOrDefault().Value;

                if (attribute.AttributeType == typeof(InjectAsSingletonAttribute))
                {
                    collection.AddSingleton(resolveType, attributedType);
                    continue;
                }

                if (attribute.AttributeType == typeof(InjectAsTransientAttribute))
                {
                    collection.AddTransient(resolveType, attributedType);
                    continue;
                }

                if (attribute.AttributeType == typeof(InjectAsScopedAttribute))
                {
                    collection.AddScoped(resolveType, attributedType);
                    continue;
                }

                throw new NotImplementedException($"{resolveType.FullName} can't be resolver");
            }
        }

        public static Assembly[] GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assembly.FullName.StartsWith(assemblyPrefix)).ToArray();
        }

        private static bool HasDIAttrs(Type[] types, Type[] diTypes)
        {
            return types.Any(diTypes.Contains);
        }
    }
}

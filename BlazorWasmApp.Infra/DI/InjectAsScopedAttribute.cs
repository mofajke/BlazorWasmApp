using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorWasmApp.Infra.DI
{
    public class InjectAsScopedAttribute : DIAttribute
    {
        private readonly Type resolveType;

        public InjectAsScopedAttribute(Type resolveType)
        {
            this.resolveType = resolveType;
        }
    }
}

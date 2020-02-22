using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorWasmApp.Infra.DI
{
    public class InjectAsSingletonAttribute : DIAttribute
    {
        private readonly Type resolveType;

        public InjectAsSingletonAttribute(Type resolveType)
        {
            this.resolveType = resolveType;
        }
    }
}

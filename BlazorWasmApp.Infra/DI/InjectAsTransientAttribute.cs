using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorWasmApp.Infra.DI
{
    public class InjectAsTransientAttribute : DIAttribute
    {
        private readonly Type resolveType;

        public InjectAsTransientAttribute(Type resolveType)
        {
            this.resolveType = resolveType;
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace z.Core.Socket
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CoreSocketAttribute: Attribute
    {
        public PathString Path { get; }

        public CoreSocketAttribute() { }

        public CoreSocketAttribute(PathString path)
        {
            this.Path = $"/{path.Value.TrimStart('/')}";
        }

    }
}

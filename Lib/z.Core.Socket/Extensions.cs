using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace z.Core.Socket
{
    public static class Extensions
    {

        public static IApplicationBuilder UseCoreSocket(this IApplicationBuilder app,
                                                              PathString path,
                                                              ICoreSocket handler)
        {
            return app.Map(path, (_app) => _app.UseMiddleware<CoreSocketMiddleWare>(handler));
        }

        public static IApplicationBuilder UseCoreSocket(this IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            foreach (var type in serviceProvider.GetServices<ICoreSocket>())
            {
                var attr = type.GetType().GetCustomAttributes(true).SingleOrDefault(x => x.GetType() == typeof(CoreSocketAttribute)) as CoreSocketAttribute;

                app.Map(attr?.Path ?? $"/{ type.GetType().Name }", _ => _.UseMiddleware<CoreSocketMiddleWare>(type));
            }
            return app;
        }

        public static IServiceCollection AddCoreSocket(this IServiceCollection services)
        {
            services.AddTransient<ICoreSocketConnection, CoreSocketConnection>();

            foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof(CoreSocket))
                {
                    services.AddSingleton(typeof(ICoreSocket), type);
                }
            }

            return services;
        }
    }
}

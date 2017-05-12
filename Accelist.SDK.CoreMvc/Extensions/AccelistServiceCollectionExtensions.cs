using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AccelistServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguration<T>(this IServiceCollection services, IConfiguration config) where T : class
        {
            services.Configure<T>(config);
            return services.AddScoped(di => di.GetService<IOptionsSnapshot<T>>().Value);
        }

        public static IMvcBuilder RemoveStringOutputFormatter(this IMvcBuilder builder)
        {
            return builder.AddMvcOptions(options =>
            {
                options.OutputFormatters.RemoveType<StringOutputFormatter>();
            });
        }

        public static IMvcBuilder RequireHttps(this IMvcBuilder builder, bool mode)
        {
            if (mode == false)
            {
                return builder;
            }

            return builder.AddMvcOptions(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });
        }
    }
}

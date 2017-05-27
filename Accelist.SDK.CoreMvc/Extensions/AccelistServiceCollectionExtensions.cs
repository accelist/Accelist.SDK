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
    /// <summary>
    /// Contains extension methods for populating IServiceCollection and IMvcBuilder.
    /// </summary>
    public static class AccelistServiceCollectionExtensions
    {
        /// <summary>
        /// Add strongly-typed auto-reloading configuration accessor as a scoped service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddConfiguration<T>(this IServiceCollection services, IConfiguration config) where T : class
        {
            services.Configure<T>(config);
            return services.AddScoped(di => di.GetService<IOptionsSnapshot<T>>().Value);
        }

        /// <summary>
        /// Fluent builder for MVC service, for removing buggy plain text output formatter.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder RemoveStringOutputFormatter(this IMvcBuilder builder)
        {
            return builder.AddMvcOptions(options =>
            {
                options.OutputFormatters.RemoveType<StringOutputFormatter>();
            });
        }

        /// <summary>
        /// Fluent builder for MVC service, for conditionally adding global RequireHttps filter based on flag input parameter.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public static IMvcBuilder RequireHttps(this IMvcBuilder builder, bool enable)
        {
            if (enable == false)
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

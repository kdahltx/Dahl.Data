using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Dahl.Data.Tests.Common
{
    public static class Extensions
    {
        public static IServiceCollection AddServices( this IServiceCollection services )
        {
            services.AddSingleton<TestRepository, TestRepository>();


            return services;
        }
    }
}

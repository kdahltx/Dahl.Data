#if NETCOREAPP2_0 || NETCOREAPP2_1
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dahl.Data.Common
{
    public static class ConfigurationManager
    {
        public static ConnectionStringSettingsCollection ConnectionStrings { get; set; }
    }
}
#endif

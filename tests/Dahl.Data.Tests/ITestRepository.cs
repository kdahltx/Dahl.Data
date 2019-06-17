using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dahl.Data.Common;

namespace Dahl.Data.Tests
{
    public interface ITestRepository
    {
        IDatabase Database { get; set; }

    }
}

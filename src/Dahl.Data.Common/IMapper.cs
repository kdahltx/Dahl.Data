using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;

namespace Dahl.Data.Common
{
    public interface IMapper<TEntity> where TEntity : class, new()
    {
        void SetFieldOrdinals( ConcurrentDictionary<string, IFieldInfo> columns );
        void InitFieldOrdinals(IDataRecord dr);
        void InitFieldOrdinals(DataColumnCollection cols);

        TEntity Map(IDataRecord reader);
        TEntity Map(DataRow dr);
        List<TEntity> Map(DataSet ds);
    }

    //-----------------------------------------------------------------------------------------
    public interface IFieldInfo
    {
        string Name { get; set; }
        Type FieldType { get; set; }
        string DataTypeName { get; set; }
        int Ordinal { get; set; }
    }
}

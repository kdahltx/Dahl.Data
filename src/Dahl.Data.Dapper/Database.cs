using Dapper;

namespace Dahl.Data.Dapper
{
    public class Database : Data.Common.Database
    {
#if false
        public override bool BulkCopy<TEntity>(IEnumerable<TEntity> list, IBulkMapper bulkMapper)
        {
            return base.BulkCopy(list, bulkMapper);
        }

        public override bool BulkUpdate<TEntity>(IEnumerable<TEntity> list, IBulkMapper bulkMapper)
        {
            return base.BulkUpdate(list, bulkMapper);
        }

        public override string GetConnectionString()
        {
            return base.GetConnectionString();
        }

        public override string GetConnectionStringName()
        {
            return base.GetConnectionStringName();
        }

        public override IEnumerable<TEntity> Read<TEntity>(IMapper<TEntity> mapper = null)
        {
            return base.Read(mapper);
        }

        protected override DbProviderFactory CreateProviderFactory()
        {
            return base.CreateProviderFactory();
        }
#endif
        public bool CreateNamedQuery(string storedProcName, CommandParameter parameters = null)
        {
            CommandParameter parms = parameters;
            return base.CreateNamedQuery(storedProcName, parms);
        }

        public bool CreateQuery(string sqlCmd, CommandParameter parameters = null)
        {
            CommandParameter parms = parameters;
            return base.CreateQuery(sqlCmd, parms);
        }

        public int ExecuteQuery(string sqlCmd, DynamicParameters parameters)
        {
            return 0;
        }
    }
}

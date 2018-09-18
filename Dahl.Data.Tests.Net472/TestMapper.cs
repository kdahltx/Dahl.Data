namespace Dahl.Data.Tests.Net472
{
    public class TestMapper<TEntity> : Dahl.Data.Common.Mapper<TEntity> where TEntity : class, new()
    {
        public override TEntity Map( object[] values )
        {
            return null;
        }
    }
}

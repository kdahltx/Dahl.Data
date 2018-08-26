namespace Dahl.Data.Tests
{
    public class TestMapper<TEntity> : Common.Mapper<TEntity> where TEntity : class, new()
    {
        public override TEntity Map( object[] values )
        {
            return null;
        }
    }
}

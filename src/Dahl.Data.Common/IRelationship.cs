using System.Data;

namespace Dahl.Data.Common
{
    public interface IRelationship
    {
        DataSet Create(DataSet ds);
    }
}

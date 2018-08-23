using System.Data;

namespace Dahl.Data.Common
{
    public interface IParameter
    {
        void AddParameters(IDbCommand command);
    }
}

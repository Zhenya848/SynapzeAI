using System.Data;

namespace TestsService.Application.Abstractions
{
    public interface ISqlConnectionFactory
    {
        public IDbConnection Create();
    }
}
